using Mcp.Application.Tools;
using Mcp.Application.UseCases.ExecuteTool;
using Mcp.Application.UseCases.GetTool;
using Mcp.Domain.Tools;
using Moq;

namespace Mcp.Application.Tests.GetTool;

[Trait("Category", "Mcp.Application")]
[Trait("REQ", "REQ-FUNC-003")]
[Trait("UC", "UC-MCP-001")]
[Trait("UC", "UC-MCP-002")]
public class GetToolUseCaseTests
{
    [Fact]
    public void Should_Return_Tool_When_It_Exists()
    {
        var catalogMock = new Mock<IMcpToolCatalog>();
        var expectedTool = new McpToolContract(
            Name: "erp.create_order",
            Description: "desc",
            InternalTransport: "grpc",
            InternalRoute: "route",
            InputSchema: [],
            OutputSchema: []);

        catalogMock.Setup(c => c.GetByName("erp.create_order")).Returns(expectedTool);

        var useCase = new GetToolUseCase(catalogMock.Object);

        var result = useCase.Execute("erp.create_order");

        Assert.Equal("erp.create_order", result.Name);
    }

    [Fact]
    public void Should_Throw_When_Tool_Does_Not_Exist()
    {
        var catalogMock = new Mock<IMcpToolCatalog>();
        catalogMock.Setup(c => c.GetByName("unknown.tool")).Returns((McpToolContract?)null);

        var useCase = new GetToolUseCase(catalogMock.Object);

        Assert.Throws<ToolNotFoundException>(() => useCase.Execute("unknown.tool"));
    }
}
