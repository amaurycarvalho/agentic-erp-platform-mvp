namespace Agent.Infrastructure.Configuration;

public sealed class McpOptions
{
    public const string SectionName = "Mcp";

    public string BaseUrl { get; set; } = "http://mcp-service:8082";
}
