namespace Rag.Domain.Consistency;

public sealed record ConsistencyEvaluation(
    ConsistencyStatus Status,
    string Detail,
    string? ErpSnapshotVersion);
