## ADDED Requirements

### Requirement: Mutation testing via Stryker.NET

The Makefile SHALL provide a `mutation` target that runs Stryker.NET mutation testing against the test projects, using a Stryker.NET configuration (replacing the stale StrykerJS config).

#### Scenario: Mutation run produces a report
- **WHEN** `make mutation` is run manually
- **THEN** Stryker.NET runs the mutation suite and produces JSON/HTML reports with a mutation score

### Requirement: Mutation score threshold gates the manual run

The mutation run SHALL apply break/high/low thresholds and fail when the mutation score falls below the configured break threshold.

#### Scenario: Below-threshold mutation score fails
- **WHEN** the mutation score is below the break threshold
- **THEN** the `make mutation` run fails

### Requirement: Mutation testing is manual-only for now

Mutation testing SHALL be excluded from the CI quality gate and available only via the Makefile (a dedicated/nightly job will be introduced in a later change).

#### Scenario: Mutation is not part of the CI gate
- **WHEN** CI runs the quality gate
- **THEN** the mutation target is not executed by CI
