using Agent.Application.Ports;
using Agent.Application.UseCases.ProcessAgentCommand;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace Agent.Application.Tests.ProcessAgentCommand;

public sealed class ProcessAgentCommandUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_CreateOrderWithValidData_CallsCreateOrderTool()
    {
        var gateway = new FakeMcpGateway();
        gateway.SetResponse("""
        { "order_id": "ORD-100" }
        """);

        var useCase = new ProcessAgentCommandUseCase(gateway, NullLogger<ProcessAgentCommandUseCase>.Instance);

        var result = await useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "criar pedido para cliente C-001",
                CustomerId: "C-001",
                TotalAmount: 500.75m,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        Assert.Equal("create_order", result.Intent);
        Assert.Equal("erp.create_order", result.Tool);
        Assert.Equal("erp.create_order", gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_CreateOrderWithoutRequiredData_ThrowsValidation()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, NullLogger<ProcessAgentCommandUseCase>.Instance);

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "criar pedido",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<AgentValidationException>(action);
        Assert.Contains("customer_id", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_CancelInvoiceWithValidData_CallsCancelInvoiceTool()
    {
        var gateway = new FakeMcpGateway();
        gateway.SetResponse("""
        { "success": true }
        """);

        var useCase = new ProcessAgentCommandUseCase(gateway, NullLogger<ProcessAgentCommandUseCase>.Instance);

        var result = await useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "cancelar fatura INV-9 com motivo",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: "INV-9",
                Reason: "Duplicidade"),
            CancellationToken.None);

        Assert.Equal("cancel_invoice", result.Intent);
        Assert.Equal("erp.cancel_invoice", result.Tool);
        Assert.Equal("erp.cancel_invoice", gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_CancelInvoiceWithoutReason_ThrowsValidation()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, NullLogger<ProcessAgentCommandUseCase>.Instance);

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "cancelar fatura INV-9",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: "INV-9",
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<AgentValidationException>(action);
        Assert.Contains("reason", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    private sealed class FakeMcpGateway : IMcpGateway
    {
        private JsonElement _response = JsonDocument.Parse("{}").RootElement.Clone();

        public string? LastToolName { get; private set; }

        public void SetResponse(string json)
        {
            _response = JsonDocument.Parse(json).RootElement.Clone();
        }

        public Task<JsonElement> ExecuteToolAsync(string toolName, object payload, CancellationToken cancellationToken)
        {
            LastToolName = toolName;
            return Task.FromResult(_response);
        }
    }
}
