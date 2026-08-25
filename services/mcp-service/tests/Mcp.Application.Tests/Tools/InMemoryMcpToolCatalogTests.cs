using Mcp.Application.Tools;

namespace Mcp.Application.Tests.Tools;

[Trait("Category", "Mcp.Application")]
[Trait("REQ", "REQ-FUNC-003")]
[Trait("UC", "UC-MCP-001")]
public class InMemoryMcpToolCatalogTests
{
    [Fact]
    public void Should_Expose_Full_Metadata_For_Create_Order_Tool()
    {
        var catalog = new InMemoryMcpToolCatalog();

        var tool = catalog.GetByName("erp.create_order");

        Assert.NotNull(tool);
        Assert.Equal("erp.create_order", tool!.Name);
        Assert.Equal("Cria pedido no ERP via erp-acl-service.", tool.Description);
        Assert.Equal("grpc", tool.InternalTransport);
        Assert.Equal("erpacl.v1.OrderService/CreateOrder", tool.InternalRoute);

        Assert.Equal(2, tool.InputSchema.Count);
        var customerId = tool.InputSchema[0];
        Assert.Equal("customer_id", customerId.Name);
        Assert.Equal("string", customerId.Type);
        Assert.True(customerId.Required);
        Assert.Equal("Identificador do cliente.", customerId.Description);
        Assert.Equal("must_not_be_empty", customerId.Constraints);
        var totalAmount = tool.InputSchema[1];
        Assert.Equal("total_amount", totalAmount.Name);
        Assert.Equal("number", totalAmount.Type);
        Assert.True(totalAmount.Required);
        Assert.Equal("Valor total do pedido.", totalAmount.Description);
        Assert.Equal("greater_than_zero", totalAmount.Constraints);

        var orderId = Assert.Single(tool.OutputSchema);
        Assert.Equal("order_id", orderId.Name);
        Assert.Equal("string", orderId.Type);
        Assert.True(orderId.Required);
        Assert.Equal("Identificador do pedido criado.", orderId.Description);
        Assert.Null(orderId.Constraints);
    }

    [Fact]
    public void Should_Expose_Full_Metadata_For_Cancel_Invoice_Tool()
    {
        var catalog = new InMemoryMcpToolCatalog();

        var tool = catalog.GetByName("erp.cancel_invoice");

        Assert.NotNull(tool);
        Assert.Equal("erp.cancel_invoice", tool!.Name);
        Assert.Equal("Cancela fatura no ERP via erp-acl-service.", tool.Description);
        Assert.Equal("grpc", tool.InternalTransport);
        Assert.Equal("erpacl.v1.InvoiceService/CancelInvoice", tool.InternalRoute);

        Assert.Equal(2, tool.InputSchema.Count);
        var invoiceId = tool.InputSchema[0];
        Assert.Equal("invoice_id", invoiceId.Name);
        Assert.Equal("string", invoiceId.Type);
        Assert.True(invoiceId.Required);
        Assert.Equal("Identificador da fatura.", invoiceId.Description);
        Assert.Equal("must_not_be_empty", invoiceId.Constraints);
        var reason = tool.InputSchema[1];
        Assert.Equal("reason", reason.Name);
        Assert.Equal("string", reason.Type);
        Assert.True(reason.Required);
        Assert.Equal("Motivo do cancelamento.", reason.Description);
        Assert.Equal("must_not_be_empty", reason.Constraints);

        var success = Assert.Single(tool.OutputSchema);
        Assert.Equal("success", success.Name);
        Assert.Equal("boolean", success.Type);
        Assert.True(success.Required);
        Assert.Equal("Indica se a operação foi concluída com sucesso.", success.Description);
        Assert.Null(success.Constraints);
    }

    [Fact]
    public void Should_Return_Null_When_Tool_Is_Not_Found()
    {
        var catalog = new InMemoryMcpToolCatalog();

        Assert.Null(catalog.GetByName("unknown.tool"));
    }
}
