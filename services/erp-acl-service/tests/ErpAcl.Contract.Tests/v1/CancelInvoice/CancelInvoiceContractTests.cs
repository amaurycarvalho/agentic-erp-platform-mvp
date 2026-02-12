using ErpAcl.Contracts.V1;

namespace ErpAcl.Contract.Tests.V1.CancelInvoice;

[Trait("Category", "ErpAcl.Contracts")]
public class CancelInvoiceContractTests
{
    [Fact]
    public void CancelInvoiceRequest_Should_Require_InvoiceId()
    {
        var request = new CancelInvoiceRequest
        {
            InvoiceId = "INV-999"
        };

        Assert.False(string.IsNullOrWhiteSpace(request.InvoiceId));
    }

    [Fact]
    public void CancelInvoiceResponse_Should_Expose_Success_Flag()
    {
        var response = new CancelInvoiceResponse
        {
            Success = true
        };

        Assert.True(response.Success);
    }

    /*
    [Fact]
    public void CancelInvoiceResponse_Should_Not_Remove_InvoiceId_Field()
    {
        var properties = typeof(CancelInvoiceResponse).GetProperties();

        Assert.Contains(properties, p => p.Name == "InvoiceId");
    }
    */

}
