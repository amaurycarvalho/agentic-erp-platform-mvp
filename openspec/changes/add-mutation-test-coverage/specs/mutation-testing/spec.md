## ADDED Requirements

### Requirement: Service test suites must meet the mutation score threshold

Each service test suite SHALL include enough tests that its mutation score, as reported by Stryker.NET, meets the configured break threshold for the mutation run to pass.

#### Scenario: Agent service suite meets the break threshold

- **WHEN** Stryker.NET runs the Agent.Application.Tests suite
- **THEN** its mutation score is at or above the configured break threshold
