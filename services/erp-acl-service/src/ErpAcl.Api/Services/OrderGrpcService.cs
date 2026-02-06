using ErpAcl.Api.Grpc;
using ErpAcl.Application.UseCases;
using ErpAcl.Domain.Models;
using Grpc.Core;

namespace ErpAcl.Api.Services;

public class OrderGrpcService(CreateOrderUseCase useCase) : OrderService.OrderServiceBase
{
    private readonly CreateOrderUseCase _useCase = useCase;

    public override Task<CreateOrderResponse> CreateOrder(
        CreateOrderRequest request,
        ServerCallContext context)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            TotalAmount = (decimal)request.TotalAmount
        };

        var created = _useCase.Execute(order);

        return Task.FromResult(new CreateOrderResponse
        {
            OrderId = created.Id
        });
    }
}

