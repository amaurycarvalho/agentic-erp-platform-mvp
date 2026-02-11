namespace Mcp.Application.UseCases.ExecuteTool;

public sealed class ToolValidationException(string message)
    : ArgumentException(message)
{
}
