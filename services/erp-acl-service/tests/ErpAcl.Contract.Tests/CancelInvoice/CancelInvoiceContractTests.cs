using FluentAssertions;
using ErpAcl.Contract.Tests.Fixtures;
using ErpAcl.Api.Grpc;

namespace ErpAcl.Contract.Tests.CancelInvoice;

public class CancelInvoiceContractTests(GrpcTestFixture fixture) : IClassFixture<GrpcTestFixture>
{
    private readonly InvoiceService.InvoiceServiceClient _client = new InvoiceService.InvoiceServiceClient(fixture.Channel);

    [Fact]
    [Trait("Category", "Contract")]
    public async Task CancelInvoice_Should_Return_Success_When_Invoice_Exists()
    {
        var request = new CancelInvoiceRequest
        {
            InvoiceId = "INV-999",
            Reason = "Customer request"
        };

        var response = await _client.CancelInvoiceAsync(request);

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        //response.Message.Should().Contain("cancelled");
    }
}
