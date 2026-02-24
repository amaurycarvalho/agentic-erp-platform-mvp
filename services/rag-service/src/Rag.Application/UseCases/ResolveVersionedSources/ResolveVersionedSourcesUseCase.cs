using Rag.Domain.Entities;

namespace Rag.Application.UseCases.ResolveVersionedSources;

public sealed class ResolveVersionedSourcesUseCase
{
    public IReadOnlyCollection<PolicyDocument> Execute(IReadOnlyCollection<PolicyDocument> sources)
    {
        return sources
            .GroupBy(source => source.PolicyCode, StringComparer.OrdinalIgnoreCase)
            .Select(group => group
                .OrderByDescending(source => ParseVersionTokens(source.Version), VersionTokenComparer.Instance)
                .ThenByDescending(source => source.UpdatedAtUtc)
                .First())
            .ToArray();
    }

    private static int[] ParseVersionTokens(string version)
    {
        if (string.IsNullOrWhiteSpace(version))
        {
            return [0];
        }

        return version
            .Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(token => int.TryParse(token, out var parsedToken) ? parsedToken : 0)
            .ToArray();
    }

    private sealed class VersionTokenComparer : IComparer<int[]>
    {
        public static VersionTokenComparer Instance { get; } = new();

        public int Compare(int[]? left, int[]? right)
        {
            var leftTokens = left ?? [0];
            var rightTokens = right ?? [0];
            var maxLength = Math.Max(leftTokens.Length, rightTokens.Length);

            for (var index = 0; index < maxLength; index++)
            {
                var leftToken = index < leftTokens.Length ? leftTokens[index] : 0;
                var rightToken = index < rightTokens.Length ? rightTokens[index] : 0;

                var comparison = leftToken.CompareTo(rightToken);
                if (comparison != 0)
                {
                    return comparison;
                }
            }

            return 0;
        }
    }
}
