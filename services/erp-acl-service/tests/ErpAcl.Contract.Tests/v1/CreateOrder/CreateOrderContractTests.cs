using ErpAcl.Contracts.V1;

namespace ErpAcl.Contract.Tests.V1.CreateOrder;

[Trait("Category", "ErpAcl.Contracts")]
public class CreateOrderContractTests
{
    [Fact]
    public void CreateOrderRequest_Should_Have_Required_Fields()
    {
        var request = new CreateOrderRequest
        {
            CustomerId = "CUST-001",
            TotalAmount = 1.00
        };

        Assert.False(string.IsNullOrWhiteSpace(request.CustomerId));
        Assert.True(request.TotalAmount > 0);
    }

    [Fact]
    public void CreateOrderResponse_Should_Expose_OrderId()
    {
        var response = new CreateOrderResponse
        {
            OrderId = "ORD-123"
        };

        Assert.False(string.IsNullOrWhiteSpace(response.OrderId));
    }

    [Fact]
    public void CreateOrderResponse_Should_Not_Remove_OrderId_Field()
    {
        var properties = typeof(CreateOrderResponse).GetProperties();

        Assert.Contains(properties, p => p.Name == "OrderId");
    }

}
