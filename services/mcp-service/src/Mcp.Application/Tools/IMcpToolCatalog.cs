using Mcp.Domain.Tools;

namespace Mcp.Application.Tools;

public interface IMcpToolCatalog
{
    IReadOnlyCollection<McpToolContract> GetAll();
    McpToolContract? GetByName(string toolName);
}
