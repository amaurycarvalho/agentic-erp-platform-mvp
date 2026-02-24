using Rag.Application.UseCases.ResolveVersionedSources;
using Rag.Application.UseCases.ValidateConsistencyAgainstErpState;
using Rag.Domain.Interfaces;

namespace Rag.Application.UseCases.SearchPoliciesByContext;

public sealed class SearchPoliciesByContextUseCase(
    IPolicyDocumentRepository repository,
    ResolveVersionedSourcesUseCase resolveVersionedSourcesUseCase,
    ValidateConsistencyAgainstErpStateUseCase validateConsistencyAgainstErpStateUseCase,
    BuildTraceableResponseUseCase buildTraceableResponseUseCase)
{
    private readonly IPolicyDocumentRepository _repository = repository;
    private readonly ResolveVersionedSourcesUseCase _resolveVersionedSourcesUseCase = resolveVersionedSourcesUseCase;
    private readonly ValidateConsistencyAgainstErpStateUseCase _validateConsistencyAgainstErpStateUseCase = validateConsistencyAgainstErpStateUseCase;
    private readonly BuildTraceableResponseUseCase _buildTraceableResponseUseCase = buildTraceableResponseUseCase;

    public async Task<RagSearchResponse> ExecuteAsync(
        RagSearchRequest request,
        string requestId,
        CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var correlationId = string.IsNullOrWhiteSpace(request.CorrelationId)
            ? requestId
            : request.CorrelationId.Trim();

        var foundSources = await _repository.SearchByOperationContextAsync(
            request.OperationContext.Trim(),
            cancellationToken);

        var versionedSources = _resolveVersionedSourcesUseCase.Execute(foundSources);

        var maxSourceAgeDays = request.MaxSourceAgeDays ?? 30;

        var consistency = _validateConsistencyAgainstErpStateUseCase.Execute(
            versionedSources,
            request.ErpSnapshotVersion,
            maxSourceAgeDays);

        return _buildTraceableResponseUseCase.Execute(
            request,
            requestId,
            correlationId,
            versionedSources,
            consistency);
    }

    private static void ValidateRequest(RagSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OperationContext))
        {
            throw new RagValidationException("operation_context is required.");
        }

        if (request.MaxSourceAgeDays is <= 0)
        {
            throw new RagValidationException("max_source_age_days must be greater than zero.");
        }
    }
}
