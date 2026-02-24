using System.Text.Json.Serialization;

namespace Rag.Application.UseCases.SearchPoliciesByContext;

public sealed record RagSearchRequest(
    [property: JsonPropertyName("operation_context")] string OperationContext,
    [property: JsonPropertyName("correlation_id")] string? CorrelationId,
    [property: JsonPropertyName("erp_snapshot_version")] string? ErpSnapshotVersion,
    [property: JsonPropertyName("max_source_age_days")] int? MaxSourceAgeDays);
