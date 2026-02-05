using ErpAcl.Application.UseCases;
using ErpAcl.Domain.Interfaces;
using ErpAcl.Infrastructure.Adapters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOrderGateway, DummyErpAdapter>();
builder.Services.AddScoped<IInvoiceGateway, DummyErpAdapter>();

builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<CancelInvoiceUseCase>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();

