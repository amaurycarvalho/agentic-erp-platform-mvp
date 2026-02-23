using Agent.Application.Ports;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Agent.Application.UseCases.ProcessAgentCommand;

public sealed class ProcessAgentCommandUseCase(
    IMcpGateway mcpGateway,
    ILogger<ProcessAgentCommandUseCase> logger)
{
    private readonly IMcpGateway _mcpGateway = mcpGateway;
    private readonly ILogger<ProcessAgentCommandUseCase> _logger = logger;

    public async Task<AgentCommandResult> ExecuteAsync(
        AgentCommandRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            throw new AgentValidationException("message is required.");
        }

        var intent = DetectIntent(request);

        return intent switch
        {
            "create_order" => await ExecuteCreateOrderAsync(request, cancellationToken),
            "cancel_invoice" => await ExecuteCancelInvoiceAsync(request, cancellationToken),
            _ => throw new UnsupportedIntentException("Could not infer supported intent from request.")
        };
    }

    private async Task<AgentCommandResult> ExecuteCreateOrderAsync(
        AgentCommandRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerId))
        {
            throw new AgentValidationException("customer_id is required for create_order.");
        }

        if (request.TotalAmount is null || request.TotalAmount <= 0)
        {
            throw new AgentValidationException("total_amount must be greater than zero for create_order.");
        }

        var payload = new
        {
            customer_id = request.CustomerId,
            total_amount = request.TotalAmount.Value
        };

        _logger.LogInformation("Calling MCP tool {ToolName} inferred from user request", "erp.create_order");

        var output = await _mcpGateway.ExecuteToolAsync("erp.create_order", payload, cancellationToken);
        var parsedOutput = JsonSerializer.Deserialize<object>(output.GetRawText())!;

        return new AgentCommandResult("create_order", "erp.create_order", parsedOutput);
    }

    private async Task<AgentCommandResult> ExecuteCancelInvoiceAsync(
        AgentCommandRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.InvoiceId))
        {
            throw new AgentValidationException("invoice_id is required for cancel_invoice.");
        }

        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            throw new AgentValidationException("reason is required for cancel_invoice.");
        }

        var payload = new
        {
            invoice_id = request.InvoiceId,
            reason = request.Reason
        };

        _logger.LogInformation("Calling MCP tool {ToolName} inferred from user request", "erp.cancel_invoice");

        var output = await _mcpGateway.ExecuteToolAsync("erp.cancel_invoice", payload, cancellationToken);
        var parsedOutput = JsonSerializer.Deserialize<object>(output.GetRawText())!;

        return new AgentCommandResult("cancel_invoice", "erp.cancel_invoice", parsedOutput);
    }

    private static string DetectIntent(AgentCommandRequest request)
    {
        if (request.CustomerId is not null || request.TotalAmount is not null)
        {
            return "create_order";
        }

        if (request.InvoiceId is not null || request.Reason is not null)
        {
            return "cancel_invoice";
        }

        var normalizedMessage = request.Message.Trim().ToLowerInvariant();

        var indicatesOrder = normalizedMessage.Contains("pedido") || normalizedMessage.Contains("order");
        var indicatesCreate = normalizedMessage.Contains("criar") || normalizedMessage.Contains("create");
        if (indicatesOrder && indicatesCreate)
        {
            return "create_order";
        }

        var indicatesCancel = normalizedMessage.Contains("cancel");
        var indicatesInvoice = normalizedMessage.Contains("fatura") || normalizedMessage.Contains("invoice");
        if (indicatesCancel && indicatesInvoice)
        {
            return "cancel_invoice";
        }

        return "unsupported";
    }
}
