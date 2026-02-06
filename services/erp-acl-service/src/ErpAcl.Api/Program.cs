using ErpAcl.Api.Services;
using ErpAcl.Application.UseCases;
using ErpAcl.Domain.Interfaces;
using ErpAcl.Infrastructure.Adapters;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// gRPC configuration
// --------------------------------------------------
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});

// Required for gRPC over HTTP/2
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

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
app.MapGrpcService<OrderGrpcService>();
app.MapGrpcService<InvoiceGrpcService>();

app.MapHealthChecks("/health");

app.MapGet("/", () =>
    "ERP ACL Service running with gRPC. Use a gRPC client to communicate."
);

// --------------------------------------------------
// Run
// --------------------------------------------------
app.Run();



