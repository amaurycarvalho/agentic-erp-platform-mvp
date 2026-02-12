using ErpAcl.Api.Services.V1;
using ErpAcl.Application.UseCases;
using ErpAcl.Domain.Interfaces;
using ErpAcl.Infrastructure.Adapters;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Required for gRPC over HTTP/2
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
});

// --------------------------------------------------
// gRPC configuration
// --------------------------------------------------
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddGrpcReflection();

// --------------------------------------------------
// Dependency Injection
// --------------------------------------------------

// Gateways / Adapters (ACL to ERP)
builder.Services.AddScoped<IOrderGateway, DummyErpAdapter>();
builder.Services.AddScoped<IInvoiceGateway, DummyErpAdapter>();

// Use Cases
builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<CancelInvoiceUseCase>();

// --------------------------------------------------
// Observability & Health
// --------------------------------------------------
builder.Services.AddHealthChecks();

// --------------------------------------------------
// Build app
// --------------------------------------------------
var app = builder.Build();

// --------------------------------------------------
// Pipeline
// --------------------------------------------------
app.MapGrpcService<OrderGrpcV1Service>();
app.MapGrpcService<InvoiceGrpcV1Service>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapHealthChecks("/health");

app.MapGet("/", () =>
    "ERP ACL Service running with gRPC. Use a gRPC client to communicate."
).RequireHost("*:8080");

// --------------------------------------------------
// Run
// --------------------------------------------------
app.Run();



