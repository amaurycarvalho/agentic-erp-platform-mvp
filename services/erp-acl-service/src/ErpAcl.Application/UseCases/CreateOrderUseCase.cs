namespace ErpAcl.Application.UseCases;

using ErpAcl.Domain.Interfaces;
using ErpAcl.Domain.Models;

public class CreateOrderUseCase(IOrderGateway orderGateway)
{
    private readonly IOrderGateway _orderGateway = orderGateway;

    public Order Execute(Order order)
    {
        if (order.TotalAmount <= 0)
            throw new ArgumentException("Order total must be greater than zero.");

        return _orderGateway.Create(order);
    }
}

