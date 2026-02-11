namespace Mcp.Application.UseCases.ExecuteTool;

public sealed class AclBusinessException(string message)
    : InvalidOperationException(message)
{
}
