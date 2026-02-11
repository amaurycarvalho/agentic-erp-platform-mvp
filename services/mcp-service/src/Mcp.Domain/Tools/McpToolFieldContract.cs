namespace Mcp.Domain.Tools;

public sealed record McpToolFieldContract(
    string Name,
    string Type,
    bool Required,
    string Description,
    string? Constraints = null
);
