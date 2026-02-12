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
        try
        {
            _useCase.Execute(request.InvoiceId, request.Reason);
        }
        catch (ArgumentException exception)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, exception.Message));
        }
        catch (InvalidOperationException exception) when (exception.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            throw new RpcException(new Status(StatusCode.NotFound, exception.Message));
        }
        catch (InvalidOperationException exception)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, exception.Message));
        }

        return Task.FromResult(new CancelInvoiceResponse
        {
            Success = true
        });
    }
}
