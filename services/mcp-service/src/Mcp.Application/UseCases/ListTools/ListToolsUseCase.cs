using Mcp.Application.Tools;
using Mcp.Domain.Tools;

namespace Mcp.Application.UseCases.ListTools;

public sealed class ListToolsUseCase(IMcpToolCatalog toolCatalog)
{
    private readonly IMcpToolCatalog _toolCatalog = toolCatalog;

    public IReadOnlyCollection<McpToolContract> Execute() => _toolCatalog.GetAll();
}
