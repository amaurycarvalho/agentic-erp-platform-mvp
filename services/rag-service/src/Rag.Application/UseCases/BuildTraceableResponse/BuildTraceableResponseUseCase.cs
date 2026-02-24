using Rag.Domain.Consistency;
using Rag.Domain.Entities;

namespace Rag.Application.UseCases.SearchPoliciesByContext;

public sealed class BuildTraceableResponseUseCase
{
    public RagSearchResponse Execute(
        RagSearchRequest request,
        string requestId,
        string correlationId,
        IReadOnlyCollection<PolicyDocument> sources,
        ConsistencyEvaluation consistency)
    {
        var retrievedAtUtc = DateTimeOffset.UtcNow;

        var mappedSources = sources
            .Select(source => new RagSourceResponse(
                SourceId: source.SourceId,
                PolicyCode: source.PolicyCode,
                Version: source.Version,
                UpdatedAtUtc: source.UpdatedAtUtc,
                Excerpt: BuildExcerpt(source.Content)))
            .OrderBy(source => source.PolicyCode, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new RagSearchResponse(
            OperationContext: request.OperationContext.Trim(),
            CorrelationId: correlationId,
            RequestId: requestId,
            RetrievedAtUtc: retrievedAtUtc,
            Consistency: new ConsistencyResponse(
                Status: consistency.Status.ToString().ToLowerInvariant(),
                Detail: consistency.Detail,
                ErpSnapshotVersion: consistency.ErpSnapshotVersion),
            Sources: mappedSources);
    }

    private static string BuildExcerpt(string content)
    {
        const int excerptSize = 220;
        var normalized = content.Trim();

        return normalized.Length <= excerptSize
            ? normalized
            : string.Concat(normalized[..excerptSize], "...");
    }
}
