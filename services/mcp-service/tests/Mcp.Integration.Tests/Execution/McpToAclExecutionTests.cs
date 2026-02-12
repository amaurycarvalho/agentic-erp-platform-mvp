using System.Net.Http.Json;
using System.Text.Json;

namespace Mcp.Integration.Tests.Execution;

[Trait("Category", "Mcp.Integration")]
[Trait("REQ", "REQ-FUNC-003")]
[Trait("REQ", "REQ-FUNC-004")]
[Trait("REQ", "REQ-FUNC-005")]
[Trait("UC", "UC-MCP-001")]
[Trait("UC", "UC-MCP-002")]
public class McpToAclExecutionTests
{
    private static readonly string McpBaseUrl =
        Environment.GetEnvironmentVariable("MCP_BASE_URL") ?? "http://localhost:8082";

    [Fact]
    public async Task Should_Execute_Create_Order_Via_Mcp_And_Acl()
    {
        using var client = new HttpClient { BaseAddress = new Uri(McpBaseUrl) };

        var request = new
        {
            customer_id = "CUST-IT-001",
            total_amount = 250.75
        };

        var response = await client.PostAsJsonAsync("/mcp/tools/erp.create_order/execute", request);

        var body = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, $"Unexpected status {(int)response.StatusCode}: {body}");

        using var json = JsonDocument.Parse(body);
        var root = json.RootElement;

        Assert.Equal("erp.create_order", root.GetProperty("tool").GetString());
        Assert.Equal("success", root.GetProperty("status").GetString());

        var orderId = root.GetProperty("output").GetProperty("order_id").GetString();
        Assert.False(string.IsNullOrWhiteSpace(orderId));
    }

    [Fact]
    public async Task Should_Return_Acl_Business_Error_When_Canceling_Unknown_Invoice()
    {
        using var client = new HttpClient { BaseAddress = new Uri(McpBaseUrl) };

        var request = new
        {
            invoice_id = "INV-UNKNOWN-IT",
            reason = "Integration test"
        };

        var response = await client.PostAsJsonAsync("/mcp/tools/erp.cancel_invoice/execute", request);
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(422, (int)response.StatusCode);

        using var json = JsonDocument.Parse(body);
        var root = json.RootElement;

        Assert.Equal("acl_business_error", root.GetProperty("error").GetString());
        Assert.Contains("Invoice", root.GetProperty("detail").GetString(), StringComparison.OrdinalIgnoreCase);
    }
}
