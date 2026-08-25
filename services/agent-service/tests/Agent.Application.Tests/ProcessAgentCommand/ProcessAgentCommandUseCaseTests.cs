using Agent.Application.Ports;
using Agent.Application.UseCases.ProcessAgentCommand;
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

        var logger = new RecordingLogger<ProcessAgentCommandUseCase>();
        var useCase = new ProcessAgentCommandUseCase(gateway, logger);

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

        var createLog = Assert.Single(logger.Entries);
        Assert.Contains("Calling MCP tool", createLog.Template);
        Assert.Equal("erp.create_order", Assert.Single(createLog.Args.Cast<string>()));
    }

    [Fact]
    public async Task ExecuteAsync_CreateOrderWithoutRequiredData_ThrowsValidation()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

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

        var logger = new RecordingLogger<ProcessAgentCommandUseCase>();
        var useCase = new ProcessAgentCommandUseCase(gateway, logger);

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

        var cancelLog = Assert.Single(logger.Entries);
        Assert.Contains("Calling MCP tool", cancelLog.Template);
        Assert.Equal("erp.cancel_invoice", Assert.Single(cancelLog.Args.Cast<string>()));
    }

    [Fact]
    public async Task ExecuteAsync_CancelInvoiceWithoutReason_ThrowsValidation()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ExecuteAsync_EmptyMessage_ThrowsValidation(string? message)
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: message!,
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<AgentValidationException>(action);
        Assert.Contains("message is required.", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_ZeroTotalAmount_ThrowsValidation()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "ola",
                CustomerId: "C-001",
                TotalAmount: 0m,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<AgentValidationException>(action);
        Assert.Contains("total_amount", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_NullTotalAmountWithValidCustomerId_ThrowsValidation()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "ola",
                CustomerId: "C-001",
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<AgentValidationException>(action);
        Assert.Contains("total_amount", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_MissingInvoiceId_ThrowsValidation()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "ola",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: "motivo"),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<AgentValidationException>(action);
        Assert.Contains("invoice_id", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_UnknownIntent_ThrowsUnsupported()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "olá mundo",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnsupportedIntentException>(action);
        Assert.Contains("Could not infer", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_CreateKeywordOnly_ThrowsUnsupported()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "criar estoque",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnsupportedIntentException>(action);
        Assert.Contains("Could not infer", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_OrderKeywordOnly_ThrowsUnsupported()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "pedido de compra",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnsupportedIntentException>(action);
        Assert.Contains("Could not infer", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_CancelKeywordOnly_ThrowsUnsupported()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "cancelar algo",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnsupportedIntentException>(action);
        Assert.Contains("Could not infer", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_InvoiceKeywordOnly_ThrowsUnsupported()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "fatura atrasada",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnsupportedIntentException>(action);
        Assert.Contains("Could not infer", exception.Message);
        Assert.Null(gateway.LastToolName);
    }

    [Fact]
    public async Task ExecuteAsync_CancelAndInvoiceKeywords_ThrowsInvoiceIdValidation()
    {
        var gateway = new FakeMcpGateway();
        var useCase = new ProcessAgentCommandUseCase(gateway, new RecordingLogger<ProcessAgentCommandUseCase>());

        var action = () => useCase.ExecuteAsync(
            new AgentCommandRequest(
                Message: "cancelar fatura",
                CustomerId: null,
                TotalAmount: null,
                InvoiceId: null,
                Reason: null),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<AgentValidationException>(action);
        Assert.Contains("invoice_id", exception.Message);
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
