using System.Text.Json;
using Mcp.Application.Ports;
using Mcp.Application.Tools;
using Mcp.Application.UseCases.ExecuteTool;
using Mcp.Application.UseCases.GetTool;
using Mcp.Application.UseCases.ValidatePayload;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Mcp.Application.Tests.ExecuteTool;

[Trait("Category", "Mcp.Application")]
[Trait("REQ", "REQ-FUNC-003")]
[Trait("REQ", "REQ-FUNC-004")]
[Trait("REQ", "REQ-FUNC-005")]
[Trait("UC", "UC-MCP-001")]
[Trait("UC", "UC-MCP-002")]
public class ExecuteMcpToolUseCaseTests
{
    [Fact]
    public async Task Should_Execute_Create_Order_Tool_When_Payload_Is_Valid()
    {
        var gatewayMock = new Mock<IErpAclGateway>();
        gatewayMock
            .Setup(g => g.CreateOrderAsync("CUST-001", 100.50m, It.IsAny<CancellationToken>()))
            .ReturnsAsync("ORD-001");

        var useCase = BuildUseCase(gatewayMock.Object, new InMemoryMcpToolCatalog());

        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "customer_id": "CUST-001", "total_amount": 100.50 }
        """);

        var result = await useCase.ExecuteAsync("erp.create_order", payload, CancellationToken.None);

        var outputJson = JsonSerializer.Serialize(result.Output);

        Assert.Equal("erp.create_order", result.Tool);
        Assert.Contains("ORD-001", outputJson);
        gatewayMock.Verify(g => g.CreateOrderAsync("CUST-001", 100.50m, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Execute_Cancel_Invoice_Tool_When_Payload_Is_Valid()
    {
        var gatewayMock = new Mock<IErpAclGateway>();
        gatewayMock
            .Setup(g => g.CancelInvoiceAsync("INV-001", "Customer request", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var useCase = BuildUseCase(gatewayMock.Object, new InMemoryMcpToolCatalog());

        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "invoice_id": "INV-001", "reason": "Customer request" }
        """);

        var result = await useCase.ExecuteAsync("erp.cancel_invoice", payload, CancellationToken.None);

        var outputJson = JsonSerializer.Serialize(result.Output);

        Assert.Equal("erp.cancel_invoice", result.Tool);
        Assert.Contains("true", outputJson, StringComparison.OrdinalIgnoreCase);
        gatewayMock.Verify(g => g.CancelInvoiceAsync("INV-001", "Customer request", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_Tool_Is_Not_Found()
    {
        var gatewayMock = new Mock<IErpAclGateway>();
        var catalogMock = new Mock<IMcpToolCatalog>();
        catalogMock.Setup(c => c.GetByName("unknown.tool")).Returns((Mcp.Domain.Tools.McpToolContract?)null);

        var useCase = BuildUseCase(gatewayMock.Object, catalogMock.Object);
        var payload = JsonSerializer.Deserialize<JsonElement>("""{ }""");

        await Assert.ThrowsAsync<ToolNotFoundException>(() =>
            useCase.ExecuteAsync("unknown.tool", payload, CancellationToken.None));
    }

    [Fact]
    public async Task Should_Throw_When_Payload_Is_Invalid_And_Not_Call_Gateway()
    {
        var gatewayMock = new Mock<IErpAclGateway>();

        var useCase = BuildUseCase(gatewayMock.Object, new InMemoryMcpToolCatalog());

        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "customer_id": "", "total_amount": 100 }
        """);

        await Assert.ThrowsAsync<ToolValidationException>(() =>
            useCase.ExecuteAsync("erp.create_order", payload, CancellationToken.None));

        gatewayMock.Verify(g => g.CreateOrderAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private static ExecuteMcpToolUseCase BuildUseCase(
        IErpAclGateway gateway,
        IMcpToolCatalog toolCatalog)
    {
        var getToolUseCase = new GetToolUseCase(toolCatalog);
        var validatePayloadUseCase = new ValidatePayloadUseCase();

        return new ExecuteMcpToolUseCase(
            getToolUseCase,
            validatePayloadUseCase,
            gateway,
            NullLogger<ExecuteMcpToolUseCase>.Instance);
    }
}
