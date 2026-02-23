using Agent.Application.Ports;
using Agent.Application.UseCases.ProcessAgentCommand;
using Agent.Infrastructure.Clients;
using Agent.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<McpOptions>(builder.Configuration.GetSection(McpOptions.SectionName));
builder.Services.AddHttpClient<IMcpGateway, McpHttpGateway>((serviceProvider, httpClient) =>
{
    var options = serviceProvider
        .GetRequiredService<Microsoft.Extensions.Options.IOptions<McpOptions>>()
        .Value;

    httpClient.BaseAddress = new Uri(options.BaseUrl);
    httpClient.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddScoped<ProcessAgentCommandUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/agent/execute", async (
    AgentCommandRequest request,
    ProcessAgentCommandUseCase useCase,
    CancellationToken cancellationToken) =>
{
    try
    {
        var result = await useCase.ExecuteAsync(request, cancellationToken);
        return Results.Ok(result);
    }
    catch (AgentValidationException exception)
    {
        return Results.BadRequest(new { error = "validation_error", detail = exception.Message });
    }
    catch (UnsupportedIntentException exception)
    {
        return Results.BadRequest(new { error = "unsupported_intent", detail = exception.Message });
    }
    catch (McpToolExecutionException exception) when (exception.StatusCode == 422)
    {
        return Results.UnprocessableEntity(new { error = exception.ErrorCode, detail = exception.Message });
    }
    catch (McpToolExecutionException exception) when (exception.StatusCode == 503)
    {
        return Results.Json(
            new { error = exception.ErrorCode, detail = exception.Message },
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }
    catch (McpToolExecutionException exception)
    {
        return Results.Json(
            new { error = exception.ErrorCode, detail = exception.Message },
            statusCode: StatusCodes.Status502BadGateway);
    }
});

app.MapGet("/", () => Results.Ok(new
{
    service = "agent-service",
    status = "running",
    execute = "/agent/execute"
}));

app.Run();
