using Rag.Application.UseCases.ValidateConsistencyAgainstErpState;
using Rag.Domain.Consistency;
using Rag.Domain.Entities;

namespace Rag.Application.Tests.ValidateConsistencyAgainstErpState;

[Trait("Category", "Rag.Application")]
[Trait("UC", "US-RAG-002")]
[Trait("UC", "US-RAG-003")]
public class ValidateConsistencyAgainstErpStateUseCaseTests
{
    private readonly ValidateConsistencyAgainstErpStateUseCase _useCase = new();

    [Fact]
    public void Should_Report_Unknown_With_Detail_When_No_Sources()
    {
        var result = _useCase.Execute([], null, 30);

        Assert.Equal(ConsistencyStatus.Unknown, result.Status);
        Assert.Contains("No policy sources were found", result.Detail);
    }

    [Fact]
    public void Should_Report_Fresh_With_Detail_For_Recent_Sources()
    {
        var result = _useCase.Execute([Doc("2026.02", 1)], "2026.02", 30);

        Assert.Equal(ConsistencyStatus.Fresh, result.Status);
        Assert.Contains("within the configured freshness window", result.Detail);
    }

    [Fact]
    public void Should_Report_Stale_With_Detail_When_Sources_Are_Old()
    {
        var result = _useCase.Execute([Doc("2026.02", 200)], "2026.02", 30);

        Assert.Equal(ConsistencyStatus.Stale, result.Status);
        Assert.Contains("older than", result.Detail);
    }

    [Fact]
    public void Should_Report_Stale_When_Version_Does_Not_Match()
    {
        var result = _useCase.Execute([Doc("2026.01", 1)], "2026.02", 30);

        Assert.Equal(ConsistencyStatus.Stale, result.Status);
        Assert.Contains("does not match", result.Detail);
    }

    [Fact]
    public void Should_Report_Stale_When_Any_Source_Mismatches()
    {
        var result = _useCase.Execute([Doc("2026.02", 1), Doc("2026.01", 1)], "2026.02", 30);

        Assert.Equal(ConsistencyStatus.Stale, result.Status);
    }

    [Fact]
    public void Should_Consider_Most_Recent_Source_For_Freshness()
    {
        var result = _useCase.Execute([Doc("2026.02", 200), Doc("2026.02", 1)], "2026.02", 30);

        Assert.Equal(ConsistencyStatus.Fresh, result.Status);
    }

    [Fact]
    public void Should_Ignore_Empty_Source_Version_For_Mismatch()
    {
        var result = _useCase.Execute([Doc("", 1)], "2026.02", 30);

        Assert.Equal(ConsistencyStatus.Fresh, result.Status);
    }

    private static PolicyDocument Doc(string erpSnapshotVersion, int daysOld) =>
        new($"S-{daysOld}", "POL", "1.0", "content", DateTimeOffset.UtcNow.AddDays(-daysOld), ["ctx"], erpSnapshotVersion);
}
