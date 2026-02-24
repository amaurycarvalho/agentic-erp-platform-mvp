namespace Rag.Domain.Entities;

public sealed record PolicyDocument(
    string SourceId,
    string PolicyCode,
    string Version,
    string Content,
    DateTimeOffset UpdatedAtUtc,
    IReadOnlyCollection<string> OperationContexts,
    string? ErpSnapshotVersion);
