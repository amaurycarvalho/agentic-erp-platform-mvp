using System.Text.Json.Serialization;

namespace Rag.Application.UseCases.SearchPoliciesByContext;

public sealed record RagSearchResponse(
    [property: JsonPropertyName("operation_context")] string OperationContext,
    [property: JsonPropertyName("correlation_id")] string CorrelationId,
    [property: JsonPropertyName("request_id")] string RequestId,
    [property: JsonPropertyName("retrieved_at_utc")] DateTimeOffset RetrievedAtUtc,
    [property: JsonPropertyName("consistency")] ConsistencyResponse Consistency,
    [property: JsonPropertyName("sources")] IReadOnlyCollection<RagSourceResponse> Sources);

public sealed record ConsistencyResponse(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("detail")] string Detail,
    [property: JsonPropertyName("erp_snapshot_version")] string? ErpSnapshotVersion);

public sealed record RagSourceResponse(
    [property: JsonPropertyName("source_id")] string SourceId,
    [property: JsonPropertyName("policy_code")] string PolicyCode,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("updated_at_utc")] DateTimeOffset UpdatedAtUtc,
    [property: JsonPropertyName("excerpt")] string Excerpt);
