using ErpAcl.Application.UseCases;
using Grpc.Core;
using ErpAcl.Contracts.V1;

namespace ErpAcl.Api.Services.V1;

public class InvoiceGrpcV1Service(CancelInvoiceUseCase useCase) : InvoiceService.InvoiceServiceBase
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

