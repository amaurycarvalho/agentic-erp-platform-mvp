using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Rag.Integration.Tests.Search;

[Trait("Category", "Rag.Integration")]
[Trait("UC", "US-RAG-001")]
[Trait("UC", "US-RAG-002")]
[Trait("UC", "US-RAG-003")]
[Trait("UC", "US-RAG-004")]
[Trait("EC", "EC-003")]
public class SearchEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public SearchEndpointTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_Traceable_Response_With_Versioned_Sources()
    {
        var response = await _httpClient.PostAsJsonAsync("/rag/search", new
        {
            operation_context = "order.create",
            correlation_id = "CORR-SEARCH-001",
            erp_snapshot_version = "2026.02",
            max_source_age_days = 30
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadFromJsonAsync<JsonObject>();

        Assert.NotNull(json);
        Assert.Equal("order.create", json!["operation_context"]?.GetValue<string>());
        Assert.Equal("CORR-SEARCH-001", json["correlation_id"]?.GetValue<string>());
        Assert.False(string.IsNullOrWhiteSpace(json["request_id"]?.GetValue<string>()));
        Assert.False(string.IsNullOrWhiteSpace(json["retrieved_at_utc"]?.GetValue<string>()));

        var consistency = json["consistency"]?.AsObject();
        Assert.NotNull(consistency);
        Assert.False(string.IsNullOrWhiteSpace(consistency!["status"]?.GetValue<string>()));

        var sources = json["sources"]?.AsArray();
        Assert.NotNull(sources);
        Assert.NotEmpty(sources!);

        foreach (var sourceNode in sources!)
        {
            var source = sourceNode?.AsObject();
            Assert.NotNull(source);
            Assert.False(string.IsNullOrWhiteSpace(source!["source_id"]?.GetValue<string>()));
            Assert.False(string.IsNullOrWhiteSpace(source["version"]?.GetValue<string>()));
        }
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_Operation_Context_Is_Empty()
    {
        var response = await _httpClient.PostAsJsonAsync("/rag/search", new
        {
            operation_context = "",
            correlation_id = "CORR-INVALID-001"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var json = await response.Content.ReadFromJsonAsync<JsonObject>();

        Assert.NotNull(json);
        Assert.Equal("validation_error", json!["error"]?.GetValue<string>());
    }

    [Fact]
    public async Task Should_Return_Unknown_Consistency_When_No_Source_Is_Found()
    {
        var response = await _httpClient.PostAsJsonAsync("/rag/search", new
        {
            operation_context = "inventory.transfer",
            correlation_id = "CORR-UNKNOWN-001",
            erp_snapshot_version = "2026.02"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadFromJsonAsync<JsonObject>();

        Assert.NotNull(json);
        Assert.Equal("unknown", json!["consistency"]?["status"]?.GetValue<string>());
        Assert.Empty(json["sources"]?.AsArray() ?? []);
    }
}
