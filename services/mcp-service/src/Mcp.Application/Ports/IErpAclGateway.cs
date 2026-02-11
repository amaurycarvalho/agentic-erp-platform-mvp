namespace Mcp.Application.Ports;

public interface IErpAclGateway
{
    Task<string> CreateOrderAsync(string customerId, decimal totalAmount, CancellationToken cancellationToken);
    Task<bool> CancelInvoiceAsync(string invoiceId, string reason, CancellationToken cancellationToken);
}
