using Mcp.Application.Tools;
using Mcp.Application.UseCases.ExecuteTool;
using Mcp.Domain.Tools;

namespace Mcp.Application.UseCases.GetTool;

public sealed class GetToolUseCase(IMcpToolCatalog toolCatalog)
{
    private readonly IMcpToolCatalog _toolCatalog = toolCatalog;

    public McpToolContract Execute(string toolName)
    {
        var tool = _toolCatalog.GetByName(toolName);
        return tool ?? throw new ToolNotFoundException(toolName);
    }
}
