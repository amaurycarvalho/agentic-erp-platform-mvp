using System.Text.Json;
using Mcp.Application.Ports;
using Mcp.Application.Tools;
using Mcp.Application.UseCases.ExecuteTool;
using Mcp.Application.UseCases.GetTool;
using Mcp.Application.UseCases.ListTools;
using Mcp.Application.UseCases.ValidatePayload;
using Mcp.Infrastructure.Adapters;
using Mcp.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8082");
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMcpToolCatalog, InMemoryMcpToolCatalog>();
builder.Services.Configure<ErpAclOptions>(builder.Configuration.GetSection(ErpAclOptions.SectionName));
builder.Services.AddScoped<IErpAclGateway, ErpAclGrpcGateway>();
builder.Services.AddScoped<ListToolsUseCase>();
builder.Services.AddScoped<GetToolUseCase>();
builder.Services.AddScoped<ValidatePayloadUseCase>();
builder.Services.AddScoped<ExecuteMcpToolUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/mcp/tools", (ListToolsUseCase useCase) =>
    Results.Ok(useCase.Execute()));

app.MapGet("/mcp/tools/{toolName}", (string toolName, GetToolUseCase useCase) =>
{
    try
    {
        return Results.Ok(useCase.Execute(toolName));
    }
    catch (ToolNotFoundException)
    {
        return Results.NotFound(new { error = "tool_not_found", tool = toolName });
    }
});

app.MapPost("/mcp/tools/{toolName}/execute", async (
    string toolName,
    JsonElement payload,
    ExecuteMcpToolUseCase useCase,
    CancellationToken cancellationToken) =>
{
    try
    {
        var result = await useCase.ExecuteAsync(toolName, payload, cancellationToken);
        return Results.Ok(result);
    }
    catch (ToolNotFoundException)
    {
        return Results.NotFound(new { error = "tool_not_found", tool = toolName });
    }
    catch (ToolValidationException exception)
    {
        return Results.BadRequest(new { error = "validation_error", detail = exception.Message });
    }
    catch (AclBusinessException exception)
    {
        return Results.UnprocessableEntity(new { error = "acl_business_error", detail = exception.Message });
    }
    catch (AclUnavailableException exception)
    {
        return Results.Json(
            new { error = "acl_unavailable", detail = exception.Message },
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }
});

app.MapGet("/", () => Results.Ok(new
{
    service = "mcp-service",
    status = "running",
    catalog = "/mcp/tools",
    execute = "/mcp/tools/{toolName}/execute"
}));

app.Run();
