using Mcp.Domain.Tools;

namespace Mcp.Application.Tools;

public sealed class InMemoryMcpToolCatalog : IMcpToolCatalog
{
    private static readonly IReadOnlyList<McpToolContract> Tools =
    [
        new McpToolContract(
            Name: "erp.create_order",
            Description: "Cria pedido no ERP via erp-acl-service.",
            InternalTransport: "grpc",
            InternalRoute: "erpacl.v1.OrderService/CreateOrder",
            InputSchema:
            [
                new McpToolFieldContract(
                    Name: "customer_id",
                    Type: "string",
                    Required: true,
                    Description: "Identificador do cliente.",
                    Constraints: "must_not_be_empty"
                ),
                new McpToolFieldContract(
                    Name: "total_amount",
                    Type: "number",
                    Required: true,
                    Description: "Valor total do pedido.",
                    Constraints: "greater_than_zero"
                )
            ],
            OutputSchema:
            [
                new McpToolFieldContract(
                    Name: "order_id",
                    Type: "string",
                    Required: true,
                    Description: "Identificador do pedido criado."
                )
            ]
        ),
        new McpToolContract(
            Name: "erp.cancel_invoice",
            Description: "Cancela fatura no ERP via erp-acl-service.",
            InternalTransport: "grpc",
            InternalRoute: "erpacl.v1.InvoiceService/CancelInvoice",
            InputSchema:
            [
                new McpToolFieldContract(
                    Name: "invoice_id",
                    Type: "string",
                    Required: true,
                    Description: "Identificador da fatura.",
                    Constraints: "must_not_be_empty"
                ),
                new McpToolFieldContract(
                    Name: "reason",
                    Type: "string",
                    Required: true,
                    Description: "Motivo do cancelamento.",
                    Constraints: "must_not_be_empty"
                )
            ],
            OutputSchema:
            [
                new McpToolFieldContract(
                    Name: "success",
                    Type: "boolean",
                    Required: true,
                    Description: "Indica se a operação foi concluída com sucesso."
                )
            ]
        )
    ];

    public IReadOnlyCollection<McpToolContract> GetAll() => Tools;

    public McpToolContract? GetByName(string toolName)
    {
        if (string.IsNullOrWhiteSpace(toolName))
        {
            return null;
        }

        return Tools.FirstOrDefault(tool =>
            string.Equals(tool.Name, toolName, StringComparison.OrdinalIgnoreCase));
    }
}
