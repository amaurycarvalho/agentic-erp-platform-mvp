using Rag.Domain.Entities;
using Rag.Domain.Interfaces;

namespace Rag.Infrastructure.Repositories;

public sealed class InMemoryPolicyDocumentRepository : IPolicyDocumentRepository
{
    private static readonly IReadOnlyCollection<PolicyDocument> SeedDocuments =
    [
        new PolicyDocument(
            SourceId: "POL-ORDER-APPROVAL-V1",
            PolicyCode: "ORDER_APPROVAL",
            Version: "1.0",
            Content: "Orders above 5000 require manual manager approval before submission to ERP.",
            UpdatedAtUtc: DateTimeOffset.UtcNow.AddDays(-90),
            OperationContexts: ["order.create"],
            ErpSnapshotVersion: "2026.02"),
        new PolicyDocument(
            SourceId: "POL-ORDER-APPROVAL-V2",
            PolicyCode: "ORDER_APPROVAL",
            Version: "2.0",
            Content: "Orders above 3000 require manager approval and reason code in the request.",
            UpdatedAtUtc: DateTimeOffset.UtcNow.AddDays(-3),
            OperationContexts: ["order.create"],
            ErpSnapshotVersion: "2026.02"),
        new PolicyDocument(
            SourceId: "POL-INVOICE-CANCEL-V1",
            PolicyCode: "INVOICE_CANCELLATION",
            Version: "1.0",
            Content: "Invoice cancellation requires reason and can happen only within 30 days after issuance.",
            UpdatedAtUtc: DateTimeOffset.UtcNow.AddDays(-7),
            OperationContexts: ["invoice.cancel"],
            ErpSnapshotVersion: "2026.02"),
        new PolicyDocument(
            SourceId: "POL-GENERAL-COMPLIANCE",
            PolicyCode: "GENERAL_COMPLIANCE",
            Version: "1.0",
            Content: "All operations must include correlation identifiers and audit metadata.",
            UpdatedAtUtc: DateTimeOffset.UtcNow.AddDays(-4),
            OperationContexts: ["global", "order.create", "invoice.cancel"],
            ErpSnapshotVersion: null)
    ];

    public Task<IReadOnlyCollection<PolicyDocument>> SearchByOperationContextAsync(
        string operationContext,
        CancellationToken cancellationToken)
    {
        var normalizedContext = operationContext.Trim();

        var contextSpecificResults = SeedDocuments
            .Where(document => document.OperationContexts.Any(context =>
                string.Equals(context, normalizedContext, StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        if (contextSpecificResults.Length == 0)
        {
            return Task.FromResult<IReadOnlyCollection<PolicyDocument>>([]);
        }

        var globalResults = SeedDocuments
            .Where(document => document.OperationContexts.Any(context =>
                string.Equals(context, "global", StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        var results = contextSpecificResults
            .Concat(globalResults)
            .DistinctBy(document => document.SourceId, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<PolicyDocument>>(results);
    }
}
