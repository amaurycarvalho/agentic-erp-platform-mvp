namespace Agent.Infrastructure.Clients;

public sealed class McpToolExecutionException(string errorCode, string detail, int statusCode)
    : Exception(detail)
{
    public string ErrorCode { get; } = errorCode;
    public int StatusCode { get; } = statusCode;
}
