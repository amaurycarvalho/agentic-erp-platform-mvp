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
            var response = await ExecuteWithResilienceAsync(async (channel, ct) =>
            {
                var client = new OrderService.OrderServiceClient(channel);
                return await client.CreateOrderAsync(
                    new CreateOrderRequest
                    {
                        CustomerId = customerId,
                        TotalAmount = (double)totalAmount
                    },
                    deadline: DateTime.UtcNow.AddMilliseconds(_options.CallTimeoutMs),
                    cancellationToken: ct);
            }, cancellationToken);

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
            var response = await ExecuteWithResilienceAsync(async (channel, ct) =>
            {
                var client = new InvoiceService.InvoiceServiceClient(channel);
                return await client.CancelInvoiceAsync(
                    new CancelInvoiceRequest
                    {
                        InvoiceId = invoiceId,
                        Reason = reason
                    },
                    deadline: DateTime.UtcNow.AddMilliseconds(_options.CallTimeoutMs),
                    cancellationToken: ct);
            }, cancellationToken);

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

    private async Task<TResponse> ExecuteWithResilienceAsync<TResponse>(
        Func<GrpcChannel, CancellationToken, Task<TResponse>> call,
        CancellationToken cancellationToken)
    {
        var maxAttempts = Math.Max(1, _options.MaxRetries + 1);
        RpcException? lastTransientException = null;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(_options.GrpcAddress);
                return await call(channel, cancellationToken);
            }
            catch (RpcException rpcException) when (IsTransient(rpcException))
            {
                lastTransientException = rpcException;

                if (attempt >= maxAttempts || cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var delay = TimeSpan.FromMilliseconds(Math.Max(0, _options.RetryDelayMs) * attempt);
                await Task.Delay(delay, cancellationToken);
            }
        }

        if (lastTransientException is not null)
        {
            throw new RpcException(
                new Status(
                    lastTransientException.StatusCode,
                    $"Transient failure after {maxAttempts} attempts. {lastTransientException.Status.Detail}"),
                lastTransientException.Trailers);
        }

        throw new RpcException(new Status(StatusCode.Unavailable, "Unknown transient failure."));
    }

    private static bool IsTransient(RpcException rpcException) =>
        rpcException.StatusCode is StatusCode.Unavailable or StatusCode.DeadlineExceeded;
}
