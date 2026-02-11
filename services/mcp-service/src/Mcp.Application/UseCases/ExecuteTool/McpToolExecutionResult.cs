namespace Mcp.Application.UseCases.ExecuteTool;

public sealed record McpToolExecutionResult(
    string Tool,
    object Output,
    string Status = "success"
);
