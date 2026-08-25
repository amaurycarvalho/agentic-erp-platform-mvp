using Rag.Application.UseCases.SearchPoliciesByContext;
using Rag.Domain.Consistency;
using Rag.Domain.Entities;

namespace Rag.Application.Tests.BuildTraceableResponse;

[Trait("Category", "Rag.Application")]
[Trait("UC", "US-RAG-004")]
public class BuildTraceableResponseUseCaseTests
{
    private readonly BuildTraceableResponseUseCase _useCase = new();

    [Fact]
    public void Should_Truncate_Excerpt_For_Long_Content()
    {
        var content = new string('a', 300);

        var result = _useCase.Execute(Request(), "REQ", "CORR", [Policy("S-1", "POL", content)], Fresh());

        var excerpt = Assert.Single(result.Sources).Excerpt;
        Assert.Equal(223, excerpt.Length);
        Assert.EndsWith("...", excerpt);
    }

    [Fact]
    public void Should_Keep_Full_Excerpt_At_Exact_Boundary()
    {
        var content = new string('b', 220);

        var result = _useCase.Execute(Request(), "REQ", "CORR", [Policy("S-1", "POL", content)], Fresh());

        var excerpt = Assert.Single(result.Sources).Excerpt;
        Assert.Equal(content, excerpt);
    }

    private static RagSearchRequest Request() => new("order.create", "CORR", "2026.02", 30);

    private static ConsistencyEvaluation Fresh() =>
        new(ConsistencyStatus.Fresh, "ok", "2026.02");

    private static PolicyDocument Policy(string id, string policyCode, string content) =>
        new(id, policyCode, "1.0", content, DateTimeOffset.UtcNow, ["ctx"], null);
}
