namespace Mcp.Infrastructure.Configuration;

public sealed class ErpAclOptions
{
    public const string SectionName = "ErpAcl";

    public string GrpcAddress { get; init; } = "http://erp-acl-service:8081";
}
