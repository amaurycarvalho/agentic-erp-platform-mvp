# ci-pipeline Specification

## Purpose
TBD - created by archiving change add-ci-release-pipeline. Update Purpose after archive.
## Requirements
### Requirement: CI runs a test-only quality gate

On push to `main` and on pull requests, the CI workflow SHALL run the quality gate, which consists of `install` and `test` only.

#### Scenario: CI gate on push and PR
- **WHEN** code is pushed to `main` or a pull request targets `main`
- **THEN** the CI workflow runs `make install` and `make test`
- **AND** the pipeline fails if either step fails

#### Scenario: Test is the gate scope
- **WHEN** the CI workflow runs the quality gate
- **THEN** no lint, security, complexity, or mutation step is executed

### Requirement: CI uses the .NET SDK

The CI workflow SHALL use the .NET 8 SDK for building and testing, and SHALL not set up Node.js or Python.

#### Scenario: Dotnet is the only toolchain
- **WHEN** CI runs
- **THEN** the .NET 8 SDK is installed and Node.js/Python are not set up

### Requirement: MCP cross-service integration is not in the CI gate

The CI quality gate SHALL NOT run the `mcp-service` cross-service integration tests that require a live stack; those are deferred to the release pipeline.

#### Scenario: Live-stack MCP tests are deferred
- **WHEN** CI runs the quality gate
- **THEN** the `mcp-service` integration tests against a live stack are not executed

