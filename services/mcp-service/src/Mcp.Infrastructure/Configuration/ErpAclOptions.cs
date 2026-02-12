namespace Mcp.Infrastructure.Configuration;

public sealed class ErpAclOptions
{
    public const string SectionName = "ErpAcl";

    public string GrpcAddress { get; init; } = "http://erp-acl-service:8081";
    public int CallTimeoutMs { get; init; } = 1500;
    public int MaxRetries { get; init; } = 2;
    public int RetryDelayMs { get; init; } = 100;
}
