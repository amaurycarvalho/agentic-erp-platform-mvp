using ErpAcl.Application.UseCases;
using ErpAcl.Domain.Interfaces;
using ErpAcl.Domain.Models;
using Moq;

namespace ErpAcl.Application.Tests.CreateOrder;

[Trait("Category", "ErpAcl.Application")]
public class CreateOrderUseCaseTests
{
    private readonly Mock<IOrderGateway> _orderGatewayMock;
    private readonly CreateOrderUseCase _useCase;

    public CreateOrderUseCaseTests()
    {
        _orderGatewayMock = new Mock<IOrderGateway>();
        _useCase = new CreateOrderUseCase(_orderGatewayMock.Object);
    }

    [Fact]
    public void Should_Create_Order_When_Data_Is_Valid()
    {
        var order = new Order
        {
            CustomerId = "CUST-001",
            TotalAmount = 100
        };

        _orderGatewayMock
            .Setup(g => g.Create(It.IsAny<Order>()))
            .Returns(order);

        var result = _useCase.Execute(order);

        Assert.NotNull(result);
        Assert.Equal(100, result.TotalAmount);
        _orderGatewayMock.Verify(g => g.Create(order), Times.Once);
    }

    [Fact]
    public void Should_Throw_Exception_When_Total_Is_Zero_Or_Less()
    {
        var order = new Order
        {
            CustomerId = "CUST-001",
            TotalAmount = 0
        };

        Assert.Throws<ArgumentException>(() =>
            _useCase.Execute(order)
        );
    }
}

