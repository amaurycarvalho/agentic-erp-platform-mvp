using Rag.Application.UseCases.ResolveVersionedSources;
using Rag.Application.UseCases.SearchPoliciesByContext;
using Rag.Application.UseCases.ValidateConsistencyAgainstErpState;
using Rag.Domain.Entities;
using Rag.Domain.Interfaces;

namespace Rag.Application.Tests.SearchPoliciesByContext;

[Trait("Category", "Rag.Application")]
[Trait("UC", "US-RAG-001")]
[Trait("UC", "US-RAG-002")]
[Trait("UC", "US-RAG-003")]
[Trait("UC", "US-RAG-004")]
public class SearchPoliciesByContextUseCaseTests
{
    [Fact]
    public async Task Should_Return_Only_Relevant_And_Latest_Policy_Versions()
    {
        var repository = new FakePolicyDocumentRepository(
        [
            new PolicyDocument("POL-1-V1", "ORDER_APPROVAL", "1.0", "v1", DateTimeOffset.UtcNow.AddDays(-12), ["order.create"], "2026.02"),
            new PolicyDocument("POL-1-V2", "ORDER_APPROVAL", "2.0", "v2", DateTimeOffset.UtcNow.AddDays(-2), ["order.create"], "2026.02"),
            new PolicyDocument("POL-2-V1", "GENERAL_COMPLIANCE", "1.0", "global", DateTimeOffset.UtcNow.AddDays(-1), ["global"], null),
            new PolicyDocument("POL-3-V1", "INVOICE_CANCELLATION", "1.0", "invoice", DateTimeOffset.UtcNow.AddDays(-1), ["invoice.cancel"], "2026.02")
        ]);

        var useCase = BuildUseCase(repository);

        var result = await useCase.ExecuteAsync(
            new RagSearchRequest("order.create", "CORR-001", "2026.02", 30),
            requestId: "REQ-001",
            cancellationToken: CancellationToken.None);

        Assert.Equal("order.create", result.OperationContext);
        Assert.Equal("CORR-001", result.CorrelationId);
        Assert.Equal("fresh", result.Consistency.Status);
        Assert.Equal(2, result.Sources.Count);

        var orderPolicy = Assert.Single(result.Sources.Where(source => source.PolicyCode == "ORDER_APPROVAL"));
        Assert.Equal("2.0", orderPolicy.Version);
        Assert.DoesNotContain(result.Sources, source => source.PolicyCode == "INVOICE_CANCELLATION");
    }

    [Fact]
    public async Task Should_Return_Unknown_Consistency_When_No_Policy_Is_Found()
    {
        var useCase = BuildUseCase(new FakePolicyDocumentRepository([]));

        var result = await useCase.ExecuteAsync(
            new RagSearchRequest("unknown.context", null, "2026.02", 30),
            requestId: "REQ-EMPTY",
            cancellationToken: CancellationToken.None);

        Assert.Equal("unknown", result.Consistency.Status);
        Assert.Empty(result.Sources);
        Assert.Equal("REQ-EMPTY", result.CorrelationId);
    }

    [Fact]
    public async Task Should_Return_Stale_When_Sources_Are_Older_Than_Allowed_Window()
    {
        var repository = new FakePolicyDocumentRepository(
        [
            new PolicyDocument("POL-OLD", "ORDER_APPROVAL", "1.0", "old", DateTimeOffset.UtcNow.AddDays(-180), ["order.create"], "2026.02")
        ]);

        var useCase = BuildUseCase(repository);

        var result = await useCase.ExecuteAsync(
            new RagSearchRequest("order.create", null, "2026.02", 30),
            requestId: "REQ-STALE",
            cancellationToken: CancellationToken.None);

        Assert.Equal("stale", result.Consistency.Status);
    }

    [Fact]
    public async Task Should_Throw_When_Operation_Context_Is_Empty()
    {
        var useCase = BuildUseCase(new FakePolicyDocumentRepository([]));

        await Assert.ThrowsAsync<RagValidationException>(() =>
            useCase.ExecuteAsync(
                new RagSearchRequest("", null, null, 30),
                requestId: "REQ-INVALID",
                cancellationToken: CancellationToken.None));
    }

    private static SearchPoliciesByContextUseCase BuildUseCase(IPolicyDocumentRepository repository)
    {
        return new SearchPoliciesByContextUseCase(
            repository,
            new ResolveVersionedSourcesUseCase(),
            new ValidateConsistencyAgainstErpStateUseCase(),
            new BuildTraceableResponseUseCase());
    }

    private sealed class FakePolicyDocumentRepository(IReadOnlyCollection<PolicyDocument> documents) : IPolicyDocumentRepository
    {
        private readonly IReadOnlyCollection<PolicyDocument> _documents = documents;

        public Task<IReadOnlyCollection<PolicyDocument>> SearchByOperationContextAsync(
            string operationContext,
            CancellationToken cancellationToken)
        {
            var normalizedContext = operationContext.Trim();

            var contextSpecific = _documents
                .Where(document => document.OperationContexts.Any(context =>
                    string.Equals(context, normalizedContext, StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            if (contextSpecific.Length == 0)
            {
                return Task.FromResult<IReadOnlyCollection<PolicyDocument>>([]);
            }

            var global = _documents
                .Where(document => document.OperationContexts.Any(context =>
                    string.Equals(context, "global", StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            var matched = contextSpecific
                .Concat(global)
                .DistinctBy(document => document.SourceId, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return Task.FromResult<IReadOnlyCollection<PolicyDocument>>(matched);
        }
    }
}
