using System.Net.Http.Json;
using System.Text.Json;

namespace Mcp.Integration.Tests.Execution;

[Trait("Category", "Mcp.Integration.Resilience")]
[Trait("REQ", "REQ-FUNC-003")]
[Trait("REQ", "NFR-003")]
[Trait("UC", "UC-MCP-001")]
public class McpResilienceExecutionTests
{
    private static readonly string? UnavailableBaseUrl =
        Environment.GetEnvironmentVariable("MCP_UNAVAILABLE_BASE_URL");

    [Fact]
    public async Task Should_Return_Acl_Unavailable_After_Timeout_And_Retries()
    {
        if (string.IsNullOrWhiteSpace(UnavailableBaseUrl))
        {
            return;
        }

        using var client = new HttpClient { BaseAddress = new Uri(UnavailableBaseUrl) };

        var request = new
        {
            customer_id = "CUST-RESILIENCE-001",
            total_amount = 50.0
        };

        var response = await client.PostAsJsonAsync("/mcp/tools/erp.create_order/execute", request);
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(503, (int)response.StatusCode);

        using var json = JsonDocument.Parse(body);
        var root = json.RootElement;

        Assert.Equal("acl_unavailable", root.GetProperty("error").GetString());
        Assert.Contains("attempt", root.GetProperty("detail").GetString(), StringComparison.OrdinalIgnoreCase);
    }
}
