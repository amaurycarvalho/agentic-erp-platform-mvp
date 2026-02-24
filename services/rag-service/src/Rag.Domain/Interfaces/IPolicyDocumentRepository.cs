using Rag.Domain.Entities;

namespace Rag.Domain.Interfaces;

public interface IPolicyDocumentRepository
{
    Task<IReadOnlyCollection<PolicyDocument>> SearchByOperationContextAsync(
        string operationContext,
        CancellationToken cancellationToken);
}
