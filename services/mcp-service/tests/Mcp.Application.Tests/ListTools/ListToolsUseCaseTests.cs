using Mcp.Application.Tools;
using Mcp.Application.UseCases.ListTools;

namespace Mcp.Application.Tests.ListTools;

public class ListToolsUseCaseTests
{
    [Fact]
    public void Should_Return_All_Tools_From_Catalog()
    {
        var catalog = new InMemoryMcpToolCatalog();
        var useCase = new ListToolsUseCase(catalog);

        var result = useCase.Execute();

        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
        Assert.Contains(result, tool => tool.Name == "erp.create_order");
        Assert.Contains(result, tool => tool.Name == "erp.cancel_invoice");
    }
}
