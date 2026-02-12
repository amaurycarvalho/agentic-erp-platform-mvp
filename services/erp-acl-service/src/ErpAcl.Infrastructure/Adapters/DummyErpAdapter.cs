namespace ErpAcl.Infrastructure.Adapters;

using ErpAcl.Domain.Interfaces;
using ErpAcl.Domain.Models;

public class DummyErpAdapter : IOrderGateway, IInvoiceGateway
{
    private static readonly Dictionary<string, Invoice> Invoices = new();

    public Order Create(Order order)
    {
        // Simulates ERP order creation
        if (string.IsNullOrWhiteSpace(order.Id))
        {
            order.Id = $"ORD-{Guid.NewGuid():N}".ToUpperInvariant();
        }

        return order;
    }

    public void Cancel(string invoiceId, string reason)
    {
        if (Invoices.ContainsKey(invoiceId))
            Invoices[invoiceId].IsCancelled = true;
    }

    public Invoice? GetById(string invoiceId)
    {
        Invoices.TryGetValue(invoiceId, out var invoice);
        return invoice;
    }
}
