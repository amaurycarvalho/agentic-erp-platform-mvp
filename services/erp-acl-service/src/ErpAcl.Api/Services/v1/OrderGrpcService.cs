using ErpAcl.Application.UseCases;
using ErpAcl.Domain.Models;
using Grpc.Core;
using ErpAcl.Contracts.V1;

namespace ErpAcl.Api.Services.V1;

public class OrderGrpcV1Service(CreateOrderUseCase useCase) : OrderService.OrderServiceBase
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

