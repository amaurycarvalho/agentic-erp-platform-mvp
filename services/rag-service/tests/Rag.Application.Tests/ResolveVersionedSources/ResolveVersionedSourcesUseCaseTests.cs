using Rag.Application.UseCases.ResolveVersionedSources;
using Rag.Domain.Entities;

namespace Rag.Application.Tests.ResolveVersionedSources;

[Trait("Category", "Rag.Application")]
[Trait("UC", "US-RAG-001")]
public class ResolveVersionedSourcesUseCaseTests
{
    private readonly ResolveVersionedSourcesUseCase _useCase = new();

    [Fact]
    public void Should_Select_Highest_Version_Even_When_It_Is_Older()
    {
        var v2 = Doc("2.0", olderDays: 20);
        var v1 = Doc("1.0", olderDays: 1);

        var result = _useCase.Execute([v2, v1]);

        var selected = Assert.Single(result);
        Assert.Equal("2.0", selected.Version);
    }

    [Fact]
    public void Should_Compare_Tokens_Numerically_By_Segment()
    {
        var v110 = Doc("1.10", olderDays: 20);
        var v19 = Doc("1.9", olderDays: 1);

        var result = _useCase.Execute([v110, v19]);

        var selected = Assert.Single(result);
        Assert.Equal("1.10", selected.Version);
    }

    [Fact]
    public void Should_Consider_Extra_Token_When_Left_Version_Is_Shorter()
    {
        var v12 = Doc("1.2", olderDays: 1);
        var v123 = Doc("1.2.3", olderDays: 20);

        var result = _useCase.Execute([v12, v123]);

        var selected = Assert.Single(result);
        Assert.Equal("1.2.3", selected.Version);
    }

    [Fact]
    public void Should_Consider_Extra_Token_When_Right_Version_Is_Shorter()
    {
        var v123 = Doc("1.2.3", olderDays: 20);
        var v12 = Doc("1.2", olderDays: 1);

        var result = _useCase.Execute([v123, v12]);

        var selected = Assert.Single(result);
        Assert.Equal("1.2.3", selected.Version);
    }

    [Fact]
    public void Should_Handle_Null_Version()
    {
        var nullVersion = Doc(null!, olderDays: 1);
        var v1 = Doc("1.0", olderDays: 20);

        var result = _useCase.Execute([nullVersion, v1]);

        var selected = Assert.Single(result);
        Assert.Equal("1.0", selected.Version);
    }

    [Fact]
    public void Should_Break_Tie_By_Newest_Update()
    {
        var old = Doc("1.0", olderDays: 20);
        var fresh = Doc("1.0", olderDays: 1);

        var result = _useCase.Execute([old, fresh]);

        var selected = Assert.Single(result);
        Assert.Equal(fresh.SourceId, selected.SourceId);
    }

    private static PolicyDocument Doc(string version, int olderDays) =>
        new($"S-{version}-{olderDays}", "POL", version, "content", DateTimeOffset.UtcNow.AddDays(-olderDays), ["ctx"], null);
}
