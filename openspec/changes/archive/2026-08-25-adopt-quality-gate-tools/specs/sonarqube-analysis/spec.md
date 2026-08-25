## ADDED Requirements

### Requirement: SonarCloud analysis of all service solutions

The CI workflow SHALL analyze every service solution (agent, mcp, erp-acl, rag) and publish the results to SonarCloud, so issues (bugs, code smells, vulnerabilities, security hotspots, duplicated lines, technical debt) are captured.

#### Scenario: Each service solution is analyzed
- **WHEN** CI runs the analysis
- **THEN** each service solution is scanned and published to its SonarCloud project using coverage from the test run

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

### Requirement: SonarCloud configuration is explicit

SonarCloud analysis SHALL use explicit per-service project keys and organization, driven by the SonarCloud GitHub Action, with tokens provided via repository secrets.

#### Scenario: Analysis uses configured project and org
- **WHEN** the SonarCloud action runs
- **THEN** it uses the configured `SONAR_ORG`, `SONAR_PROJECT_KEY`, and `SONAR_TOKEN` secrets
