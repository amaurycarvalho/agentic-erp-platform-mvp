# sonarqube-analysis Specification

## Purpose
Delta spec for the sonarqube-analysis capability: SonarCloud analysis becomes per-service, CI uses the SonarCloud GitHub Action (not the Makefile-local flow) and runs only on pull requests.

## MODIFIED Requirements
### Requirement: SonarCloud analysis of all service solutions

The CI workflow SHALL analyze every service solution (agent, mcp, erp-acl, rag) under its own per-service SonarCloud project key and publish the results to SonarCloud, so issues (bugs, code smells, vulnerabilities, security hotspots, duplicated lines, technical debt) are captured per service.

#### Scenario: Each service solution is analyzed in its own project
- **WHEN** CI runs the analysis
- **THEN** each service solution is scanned and published to its own SonarCloud project key using coverage from the test run

#### Scenario: Analysis runs on pull requests only
- **WHEN** a push targets `main`
- **THEN** the SonarCloud analysis job is not run

### Requirement: Pull requests are decorated

SonarCloud SHALL decorate pull requests with comments about issues found in the changed code, supporting code review.

#### Scenario: PR decoration on changes
- **WHEN** a pull request is analyzed by SonarCloud
- **THEN** the PR is decorated with findings, and the analysis marks the PR status check accordingly

### Requirement: New-code (Leak Period) gate

The SonarCloud quality gate SHALL apply to new code only (Leak Period), so existing technical debt in the baseline does not block new contributions while new code must meet defined quality standards.

#### Scenario: New code must pass the new-code gate
- **WHEN** a change introduces new/modified code
- **THEN** the SonarCloud quality gate evaluates only the new-code (Leak Period) and fails the analysis if standards are not met

#### Scenario: Existing debt does not block new contributions
- **WHEN** a change modifies a codebase that already has technical debt outside the Leak Period
- **THEN** the analysis does not fail due to that pre-existing debt

### Requirement: SonarCloud configuration is explicit and per-service

SonarCloud analysis SHALL use explicit per-service project keys under a configured organization, driven by the SonarCloud GitHub Action with tokens provided via repository secrets, and SHALL NOT invoke the local Makefile SonarQube flow in CI.

#### Scenario: Analysis uses per-service project keys and org
- **WHEN** the SonarCloud action runs
- **THEN** it uses the configured `SONAR_ORG` and `SONAR_TOKEN` secrets together with a per-service project key for each of the four services

#### Scenario: CI does not use the Makefile-local SonarQube flow
- **WHEN** the SonarCloud analysis runs in CI
- **THEN** it uses the SonarCloud GitHub Action rather than the local `sonar-check`/`sonar-install` Makefile targets

### Requirement: CI SonarCloud and integration analysis are documented

The `README.md` SHALL document that the SonarCloud analysis job and the MCP cross-service integration-test job run in CI only on pull requests, while the quality-gate job runs on pushes to `main` and on pull requests.

#### Scenario: README explains CI job triggers
- **WHEN** a developer reads the CI section of `README.md`
- **THEN** it states that SonarCloud and integration-test run only on pull requests and the quality-gate runs on push to `main` and on pull requests
