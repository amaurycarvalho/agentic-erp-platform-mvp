using System.Text.Json;
using Mcp.Application.Tools;
using Mcp.Application.UseCases.ExecuteTool;
using Mcp.Application.UseCases.ValidatePayload;

namespace Mcp.Application.Tests.ValidatePayload;

[Trait("Category", "Mcp.Application")]
[Trait("REQ", "REQ-FUNC-003")]
[Trait("UC", "UC-MCP-001")]
[Trait("UC", "UC-MCP-002")]
public class ValidatePayloadUseCaseTests
{
    private readonly ValidatePayloadUseCase _useCase = new();
    private readonly InMemoryMcpToolCatalog _catalog = new();

    [Fact]
    public void Should_Validate_Create_Order_Payload_When_Valid()
    {
        var tool = _catalog.GetByName("erp.create_order")!;
        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "customer_id": "CUST-001", "total_amount": 100.5 }
        """);

        _useCase.Execute(tool, payload);
    }

    [Fact]
    public void Should_Throw_When_Required_Field_Is_Missing()
    {
        var tool = _catalog.GetByName("erp.create_order")!;
        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "total_amount": 100.5 }
        """);

        Assert.Throws<ToolValidationException>(() => _useCase.Execute(tool, payload));
    }

    [Fact]
    public void Should_Throw_When_Field_Is_Empty_String()
    {
        var tool = _catalog.GetByName("erp.cancel_invoice")!;
        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "invoice_id": "INV-1", "reason": "" }
        """);

        Assert.Throws<ToolValidationException>(() => _useCase.Execute(tool, payload));
    }

    [Fact]
    public void Should_Throw_When_Number_Is_Not_Greater_Than_Zero()
    {
        var tool = _catalog.GetByName("erp.create_order")!;
        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "customer_id": "CUST-001", "total_amount": 0 }
        """);

        Assert.Throws<ToolValidationException>(() => _useCase.Execute(tool, payload));
    }
}
