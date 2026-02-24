using Rag.Domain.Consistency;
using Rag.Domain.Entities;

namespace Rag.Application.UseCases.ValidateConsistencyAgainstErpState;

public sealed class ValidateConsistencyAgainstErpStateUseCase
{
    public ConsistencyEvaluation Execute(
        IReadOnlyCollection<PolicyDocument> sources,
        string? erpSnapshotVersion,
        int maxSourceAgeDays)
    {
        if (sources.Count == 0)
        {
            return new ConsistencyEvaluation(
                ConsistencyStatus.Unknown,
                "No policy sources were found for this context.",
                erpSnapshotVersion);
        }

        var latestSourceUpdate = sources.Max(source => source.UpdatedAtUtc);
        var freshnessThreshold = DateTimeOffset.UtcNow.AddDays(-maxSourceAgeDays);
        var isOutdated = latestSourceUpdate < freshnessThreshold;

        var hasVersionMismatch = !string.IsNullOrWhiteSpace(erpSnapshotVersion)
            && sources.Any(source =>
                !string.IsNullOrWhiteSpace(source.ErpSnapshotVersion)
                && !string.Equals(source.ErpSnapshotVersion, erpSnapshotVersion, StringComparison.OrdinalIgnoreCase));

        if (isOutdated)
        {
            return new ConsistencyEvaluation(
                ConsistencyStatus.Stale,
                "Latest policy source is older than the configured freshness window.",
                erpSnapshotVersion);
        }

        if (hasVersionMismatch)
        {
            return new ConsistencyEvaluation(
                ConsistencyStatus.Stale,
                "Policy source version does not match the informed ERP snapshot.",
                erpSnapshotVersion);
        }

        return new ConsistencyEvaluation(
            ConsistencyStatus.Fresh,
            "Policy sources are within the configured freshness window.",
            erpSnapshotVersion);
    }
}
