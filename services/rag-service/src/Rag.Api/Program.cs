using Rag.Application.UseCases.ResolveVersionedSources;
using Rag.Application.UseCases.SearchPoliciesByContext;
using Rag.Application.UseCases.ValidateConsistencyAgainstErpState;
using Rag.Domain.Interfaces;
using Rag.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8083");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPolicyDocumentRepository, InMemoryPolicyDocumentRepository>();
builder.Services.AddScoped<ResolveVersionedSourcesUseCase>();
builder.Services.AddScoped<ValidateConsistencyAgainstErpStateUseCase>();
builder.Services.AddScoped<BuildTraceableResponseUseCase>();
builder.Services.AddScoped<SearchPoliciesByContextUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/rag/search", async (
    RagSearchRequest request,
    SearchPoliciesByContextUseCase useCase,
    HttpContext httpContext,
    CancellationToken cancellationToken) =>
{
    try
    {
        var requestId = string.IsNullOrWhiteSpace(httpContext.TraceIdentifier)
            ? Guid.NewGuid().ToString("N")
            : httpContext.TraceIdentifier;

        var response = await useCase.ExecuteAsync(request, requestId, cancellationToken);
        return Results.Ok(response);
    }
    catch (RagValidationException exception)
    {
        return Results.BadRequest(new { error = "validation_error", detail = exception.Message });
    }
});

app.MapGet("/", () => Results.Ok(new
{
    service = "rag-service",
    status = "running",
    search = "/rag/search"
}));

app.Run();

public partial class Program;
