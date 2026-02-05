namespace ErpAcl.Application.UseCases;

using ErpAcl.Domain.Interfaces;

public class CancelInvoiceUseCase(IInvoiceGateway invoiceGateway)
{
    private readonly IInvoiceGateway _invoiceGateway = invoiceGateway;

    public void Execute(string invoiceId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required.");

        var invoice = _invoiceGateway.GetById(invoiceId)
            ?? throw new InvalidOperationException("Invoice not found.");

        if (invoice.IsCancelled)
            throw new InvalidOperationException("Invoice already cancelled.");

        _invoiceGateway.Cancel(invoiceId, reason);
    }
}

