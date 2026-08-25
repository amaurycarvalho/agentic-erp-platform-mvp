## ADDED Requirements

### Requirement: RAG tests cover traceable-response excerpt and ordering

The Rag.Application test suite SHALL assert the ordering of sources and the excerpt truncation behavior in `BuildTraceableResponseUseCase` so that mutations to ordering and excerpt logic are killed.

#### Scenario: Sources are ordered by policy code

- **WHEN** a search response is built from multiple sources with different policy codes
- **THEN** the sources are returned ordered ascending (case-insensitive) by `PolicyCode`

#### Scenario: Long content is truncated

- **WHEN** a policy source has content longer than the excerpt size
- **THEN** the excerpt ends with `...` and its length equals the excerpt size plus three

#### Scenario: Content at the exact excerpt boundary is not truncated

- **WHEN** a policy source has content exactly equal to the excerpt size
- **THEN** the excerpt equals the full content without a `...` suffix

### Requirement: RAG tests cover version comparison

The Rag.Application test suite SHALL exercise `ResolveVersionedSourcesUseCase` version comparison (numeric multi-segment tokens, differing token lengths, empty/null versions, tie-break by update date) so that mutations to version parsing and comparison are killed.

#### Scenario: Highest numeric version wins even when older

- **WHEN** two versions of the same policy exist and the numerically higher version has an older update date
- **THEN** the higher version is selected

#### Scenario: Multi-segment tokens compare numerically

- **WHEN** versions `1.10` and `1.9` compete
- **THEN** `1.10` is selected

#### Scenario: Extra version segments are considered

- **WHEN** versions with different token counts compete (e.g. `1.2` and `1.2.3`)
- **THEN** the longer version is selected when its extra segment is greater than zero

#### Scenario: Null version is handled

- **WHEN** a policy source has a null version
- **THEN** it does not throw and loses to any parseable version

#### Scenario: Equal versions break the tie by newest update

- **WHEN** two sources share the same version
- **THEN** the source with the newest `UpdatedAtUtc` is selected

### Requirement: RAG tests cover search request validation

The Rag.Application test suite SHALL assert the search-request validation messages and the explicit freshness window so that mutations to validation and the default `MaxSourceAgeDays` are killed.

#### Scenario: Zero or negative max source age is rejected

- **WHEN** `ExecuteAsync` is called with `MaxSourceAgeDays` of 0 or a negative value
- **THEN** a `RagValidationException` with a message containing `max_source_age_days` is thrown

#### Scenario: Empty operation context message is asserted

- **WHEN** `ExecuteAsync` is called with an empty operation context
- **THEN** a `RagValidationException` with a message containing `operation_context` is thrown

#### Scenario: Explicit freshness window is honored

- **WHEN** `ExecuteAsync` is called with `MaxSourceAgeDays` of 5 and a source updated 10 days ago
- **THEN** the consistency status is `stale`

### Requirement: RAG tests cover consistency evaluation branches

The Rag.Application test suite SHALL exercise every branch and detail message of `ValidateConsistencyAgainstErpStateUseCase` (unknown, fresh, stale-old, version mismatch, mixed sources, differing ages, empty source versions) so that mutations to the evaluation logic are killed.

#### Scenario: Unknown detail message is asserted

- **WHEN** no sources are found
- **THEN** the consistency status is `Unknown` and the detail mentions no policy sources

#### Scenario: Fresh detail message is asserted

- **WHEN** sources are within the freshness window and versions match
- **THEN** the consistency status is `Fresh` and the detail mentions the freshness window

#### Scenario: Stale-old detail message is asserted

- **WHEN** the most recent source is older than the freshness window
- **THEN** the consistency status is `Stale` and the detail mentions the source being older

#### Scenario: Version mismatch detail message is asserted

- **WHEN** a source version does not match the informed ERP snapshot
- **THEN** the consistency status is `Stale` and the detail mentions the version not matching

#### Scenario: Any mismatch is enough for staleness

- **WHEN** some sources match the ERP snapshot and at least one does not
- **THEN** the consistency status is `Stale`

#### Scenario: Freshness is based on the most recent source

- **WHEN** sources have very different ages
- **THEN** the consistency status is based on the most recent source's age

#### Scenario: Empty source versions do not cause a mismatch

- **WHEN** a source has an empty ERP snapshot version
- **THEN** it is not treated as a version mismatch
