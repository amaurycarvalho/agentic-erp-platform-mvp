namespace Mcp.Application.UseCases.ExecuteTool;

public sealed class ToolNotFoundException(string toolName)
    : InvalidOperationException($"Tool '{toolName}' is not available in MCP catalog.")
{
    public string ToolName { get; } = toolName;
}
