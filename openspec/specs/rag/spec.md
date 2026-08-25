# rag Specification

## Purpose
TBD - created by archiving change history-mvp-foundation. Update Purpose after archive.
## Requirements
### Requirement: Retrieve policies by operation context

The rag-service SHALL accept a search by `operation_context` via `POST /rag/search` and return only sources relevant to that context.

#### Scenario: Return relevant policies
- **WHEN** a consumer calls `POST /rag/search` with a valid `operation_context`
- **THEN** the service returns only sources applicable to the context

#### Scenario: Return empty sources when none found
- **WHEN** there are no policies for the given context
- **THEN** the service returns an empty `sources` array without a technical error

#### Scenario: Reject empty operation_context
- **WHEN** `operation_context` is empty
- **THEN** the service returns `400` with the `validation_error` kind

### Requirement: Return only the latest version per policy

The rag-service SHALL return only the most recent version for a given `policy_code`, and each source SHALL expose `source_id`, `policy_code`, `version`, and `updated_at_utc`.

#### Scenario: Latest version wins
- **WHEN** multiple versions of the same policy exist
- **THEN** the service returns only the most recent version

#### Scenario: Version metadata is exposed
- **WHEN** sources are retrieved
- **THEN** each item contains `source_id`, `policy_code`, `version`, and `updated_at_utc`

### Requirement: Traceability metadata in responses

The rag-service SHALL include `correlation_id`, `request_id`, and `retrieved_at_utc` in every search response, generating correlation from the request id when none is provided.

#### Scenario: Return traceability metadata
- **WHEN** a valid search is processed
- **THEN** the response includes `correlation_id`, `request_id`, and `retrieved_at_utc`

#### Scenario: Correlation generated when absent
- **WHEN** the consumer does not send `correlation_id`
- **THEN** the service fills correlation using the `request_id`

### Requirement: Classify RAG vs ERP consistency

The rag-service SHALL classify consistency as `fresh`, `stale`, or `unknown`, reflecting `erp_snapshot_version`, where `fresh` is within `max_source_age_days` (default 30), `stale` is outside the window or diverging from `erp_snapshot_version`, and `unknown` is when no sources exist to evaluate.

#### Scenario: Classify fresh
- **WHEN** sources are within the configured `max_source_age_days`
- **THEN** the consistency status is `fresh`

#### Scenario: Classify stale
- **WHEN** the most recent source is outside `max_source_age_days` or diverges from `erp_snapshot_version`
- **THEN** the consistency status is `stale`

#### Scenario: Classify unknown
- **WHEN** there are no sources for the given context
- **THEN** the consistency status is `unknown`

