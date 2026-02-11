namespace Mcp.Domain.Tools;

public sealed record McpToolContract(
    string Name,
    string Description,
    string InternalTransport,
    string InternalRoute,
    IReadOnlyList<McpToolFieldContract> InputSchema,
    IReadOnlyList<McpToolFieldContract> OutputSchema
);
