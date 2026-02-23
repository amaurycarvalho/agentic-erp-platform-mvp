using Agent.Application.Ports;
using System.Net.Http.Json;
using System.Text.Json;

namespace Agent.Infrastructure.Clients;

public sealed class McpHttpGateway(HttpClient httpClient) : IMcpGateway
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<JsonElement> ExecuteToolAsync(
        string toolName,
        object payload,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            $"/mcp/tools/{toolName}/execute",
            payload,
            cancellationToken);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = TryReadProperty(body, "error", "unknown_error");
            var detail = TryReadProperty(body, "detail", "MCP execution failed.");

            throw new McpToolExecutionException(error, detail, (int)response.StatusCode);
        }

        if (body.ValueKind != JsonValueKind.Object || !body.TryGetProperty("output", out var output))
        {
            throw new McpToolExecutionException("invalid_mcp_response", "Missing output field in MCP response.", 502);
        }

        return output;
    }

    private static string TryReadProperty(JsonElement body, string propertyName, string fallback)
    {
        if (body.ValueKind == JsonValueKind.Object
            && body.TryGetProperty(propertyName, out var value)
            && value.ValueKind == JsonValueKind.String)
        {
            return value.GetString() ?? fallback;
        }

        return fallback;
    }
}
