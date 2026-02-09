using FluentAssertions;
using ErpAcl.Contract.Tests.Fixtures;
using ErpAcl.Api.Grpc;

namespace ErpAcl.Contract.Tests.CreateOrder;

public class CreateOrderContractTests(GrpcTestFixture fixture) : IClassFixture<GrpcTestFixture>
{
    private readonly OrderService.OrderServiceClient _client = new OrderService.OrderServiceClient(fixture.Channel);

    [Fact]
    [Trait("Category", "Contract")]
    public async Task CreateOrder_Should_Return_OrderId_When_Request_Is_Valid()
    {
        var request = new CreateOrderRequest
        {
            CustomerId = "CUST-001",
            TotalAmount = 150.50
        };

        var response = await _client.CreateOrderAsync(request);

        response.Should().NotBeNull();
        response.OrderId.Should().NotBeNullOrWhiteSpace();
        //response.Status.Should().Be("CREATED");
    }
}
