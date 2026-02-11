using ErpAcl.Contracts.V1;
using Grpc.Core;
using Grpc.Net.Client;
using Mcp.Application.Ports;
using Mcp.Application.UseCases.ExecuteTool;
using Mcp.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Mcp.Infrastructure.Adapters;

public sealed class ErpAclGrpcGateway(IOptions<ErpAclOptions> options) : IErpAclGateway
{
    private readonly ErpAclOptions _options = options.Value;

    public async Task<string> CreateOrderAsync(
        string customerId,
        decimal totalAmount,
        CancellationToken cancellationToken)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_options.GrpcAddress);
            var client = new OrderService.OrderServiceClient(channel);

            var response = await client.CreateOrderAsync(
                new CreateOrderRequest
                {
                    CustomerId = customerId,
                    TotalAmount = (double)totalAmount
                },
                cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(response.OrderId))
            {
                throw new AclBusinessException("ERP ACL returned empty order_id.");
            }

            return response.OrderId;
        }
        catch (RpcException rpcException)
        {
            throw MapRpcException(rpcException);
        }
        catch (AclBusinessException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new AclUnavailableException("Failed to call ERP ACL CreateOrder.", exception);
        }
    }

    public async Task<bool> CancelInvoiceAsync(
        string invoiceId,
        string reason,
        CancellationToken cancellationToken)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_options.GrpcAddress);
            var client = new InvoiceService.InvoiceServiceClient(channel);

            var response = await client.CancelInvoiceAsync(
                new CancelInvoiceRequest
                {
                    InvoiceId = invoiceId,
                    Reason = reason
                },
                cancellationToken: cancellationToken);

            return response.Success;
        }
        catch (RpcException rpcException)
        {
            throw MapRpcException(rpcException);
        }
        catch (Exception exception)
        {
            throw new AclUnavailableException("Failed to call ERP ACL CancelInvoice.", exception);
        }
    }

    private static Exception MapRpcException(RpcException rpcException)
    {
        return rpcException.StatusCode switch
        {
            StatusCode.InvalidArgument or
            StatusCode.NotFound or
            StatusCode.AlreadyExists or
            StatusCode.FailedPrecondition =>
                new AclBusinessException($"ERP ACL business error: {rpcException.Status.Detail}"),

            StatusCode.Unavailable or
            StatusCode.DeadlineExceeded =>
                new AclUnavailableException($"ERP ACL unavailable: {rpcException.Status.Detail}", rpcException),

            _ => new AclUnavailableException(
                $"Unexpected ERP ACL error ({rpcException.StatusCode}): {rpcException.Status.Detail}",
                rpcException)
        };
    }
}
