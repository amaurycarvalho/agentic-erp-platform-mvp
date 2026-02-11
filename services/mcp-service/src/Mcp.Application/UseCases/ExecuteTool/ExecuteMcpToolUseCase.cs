using System.Text.Json;
using Mcp.Application.Ports;
using Mcp.Application.UseCases.GetTool;
using Mcp.Application.UseCases.ValidatePayload;
using Microsoft.Extensions.Logging;

namespace Mcp.Application.UseCases.ExecuteTool;

public sealed class ExecuteMcpToolUseCase(
    GetToolUseCase getToolUseCase,
    ValidatePayloadUseCase validatePayloadUseCase,
    IErpAclGateway erpAclGateway,
    ILogger<ExecuteMcpToolUseCase> logger)
{
    private readonly GetToolUseCase _getToolUseCase = getToolUseCase;
    private readonly ValidatePayloadUseCase _validatePayloadUseCase = validatePayloadUseCase;
    private readonly IErpAclGateway _erpAclGateway = erpAclGateway;
    private readonly ILogger<ExecuteMcpToolUseCase> _logger = logger;

    public async Task<McpToolExecutionResult> ExecuteAsync(
        string toolName,
        JsonElement payload,
        CancellationToken cancellationToken)
    {
        var tool = _getToolUseCase.Execute(toolName);
        _validatePayloadUseCase.Execute(tool, payload);

        _logger.LogInformation("Executing MCP tool {ToolName}", tool.Name);

        return tool.Name switch
        {
            "erp.create_order" => await ExecuteCreateOrderAsync(tool.Name, payload, cancellationToken),
            "erp.cancel_invoice" => await ExecuteCancelInvoiceAsync(tool.Name, payload, cancellationToken),
            _ => throw new ToolNotFoundException(toolName)
        };
    }

    private async Task<McpToolExecutionResult> ExecuteCreateOrderAsync(
        string toolName,
        JsonElement payload,
        CancellationToken cancellationToken)
    {
        var customerId = RequiredString(payload, "customer_id");
        var totalAmount = RequiredPositiveDecimal(payload, "total_amount");

        var orderId = await _erpAclGateway.CreateOrderAsync(customerId, totalAmount, cancellationToken);

        _logger.LogInformation("MCP tool {ToolName} executed successfully with order_id {OrderId}", toolName, orderId);

        return new McpToolExecutionResult(
            Tool: toolName,
            Output: new { order_id = orderId });
    }

    private async Task<McpToolExecutionResult> ExecuteCancelInvoiceAsync(
        string toolName,
        JsonElement payload,
        CancellationToken cancellationToken)
    {
        var invoiceId = RequiredString(payload, "invoice_id");
        var reason = RequiredString(payload, "reason");

        var success = await _erpAclGateway.CancelInvoiceAsync(invoiceId, reason, cancellationToken);

        _logger.LogInformation("MCP tool {ToolName} executed successfully with success {Success}", toolName, success);

        return new McpToolExecutionResult(
            Tool: toolName,
            Output: new { success });
    }

    private static string RequiredString(JsonElement payload, string fieldName) =>
        payload.GetProperty(fieldName).GetString()!;

    private static decimal RequiredPositiveDecimal(JsonElement payload, string fieldName) =>
        payload.GetProperty(fieldName).GetDecimal();
}
