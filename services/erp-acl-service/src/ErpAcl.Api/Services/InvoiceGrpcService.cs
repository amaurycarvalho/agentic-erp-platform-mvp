using ErpAcl.Api.Grpc;
using ErpAcl.Application.UseCases;
using Grpc.Core;

namespace ErpAcl.Api.Services;

public class InvoiceGrpcService(CancelInvoiceUseCase useCase) : InvoiceService.InvoiceServiceBase
{
    private readonly CancelInvoiceUseCase _useCase = useCase;

    public override Task<CancelInvoiceResponse> CancelInvoice(
        CancelInvoiceRequest request,
        ServerCallContext context)
    {
        _useCase.Execute(request.InvoiceId, request.Reason);

        return Task.FromResult(new CancelInvoiceResponse
        {
            Success = true
        });
    }
}

