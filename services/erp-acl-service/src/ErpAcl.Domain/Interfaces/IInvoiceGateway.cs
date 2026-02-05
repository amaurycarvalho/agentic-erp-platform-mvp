namespace ErpAcl.Domain.Interfaces;

using ErpAcl.Domain.Models;

public interface IInvoiceGateway
{
    void Cancel(string invoiceId, string reason);
    Invoice? GetById(string invoiceId);
}

