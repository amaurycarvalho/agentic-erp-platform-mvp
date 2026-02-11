namespace Mcp.Application.UseCases.ExecuteTool;

public sealed class AclUnavailableException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
}
