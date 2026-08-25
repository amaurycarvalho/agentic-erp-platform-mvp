using System.Text.Json;
using Mcp.Application.Tools;
using Mcp.Application.UseCases.ExecuteTool;
using Mcp.Application.UseCases.ValidatePayload;
using Mcp.Domain.Tools;

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

        var exception = Assert.Throws<ToolValidationException>(() => _useCase.Execute(tool, payload));
        Assert.Contains("is required.", exception.Message);
    }

    [Fact]
    public void Should_Throw_When_Field_Is_Empty_String()
    {
        var tool = _catalog.GetByName("erp.cancel_invoice")!;
        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "invoice_id": "INV-1", "reason": "" }
        """);

        var exception = Assert.Throws<ToolValidationException>(() => _useCase.Execute(tool, payload));
        Assert.Contains("must not be empty", exception.Message);
    }

    [Fact]
    public void Should_Throw_When_Number_Is_Not_Greater_Than_Zero()
    {
        var tool = _catalog.GetByName("erp.create_order")!;
        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "customer_id": "CUST-001", "total_amount": 0 }
        """);

        var exception = Assert.Throws<ToolValidationException>(() => _useCase.Execute(tool, payload));
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void Should_Throw_When_String_Field_Receives_Number()
    {
        var tool = _catalog.GetByName("erp.create_order")!;
        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "customer_id": 123, "total_amount": 100.5 }
        """);

        var exception = Assert.Throws<ToolValidationException>(() => _useCase.Execute(tool, payload));
        Assert.Contains("must be of type", exception.Message);
    }

    [Fact]
    public void Should_Throw_When_Number_Field_Receives_String()
    {
        var tool = _catalog.GetByName("erp.create_order")!;
        var payload = JsonSerializer.Deserialize<JsonElement>("""
        { "customer_id": "CUST-001", "total_amount": "100.5" }
        """);

        var exception = Assert.Throws<ToolValidationException>(() => _useCase.Execute(tool, payload));
        Assert.Contains("must be of type", exception.Message);
    }

    [Fact]
    public void Should_Accept_Valid_Boolean_Field()
    {
        var tool = new McpToolContract(
            Name: "custom.tool",
            Description: "desc",
            InternalTransport: "grpc",
            InternalRoute: "route",
            InputSchema:
            [
                new McpToolFieldContract(
                    Name: "active",
                    Type: "boolean",
                    Required: true,
                    Description: "flag",
                    Constraints: null)
            ],
            OutputSchema: []);

        var payload = JsonSerializer.Deserialize<JsonElement>("""{ "active": true }""");

        _useCase.Execute(tool, payload);
    }

    [Fact]
    public void Should_Accept_Unknown_Field_Type()
    {
        var tool = new McpToolContract(
            Name: "custom.tool",
            Description: "desc",
            InternalTransport: "grpc",
            InternalRoute: "route",
            InputSchema:
            [
                new McpToolFieldContract(
                    Name: "when",
                    Type: "date",
                    Required: true,
                    Description: "data",
                    Constraints: null)
            ],
            OutputSchema: []);

        var payload = JsonSerializer.Deserialize<JsonElement>("""{ "when": "2026-01-01" }""");

        _useCase.Execute(tool, payload);
    }
}
