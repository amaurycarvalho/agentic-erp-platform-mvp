# add-local-sonarqube-per-service

## Why

SonarQube analysis is only available via SonarCloud in CI today, so developers cannot run an analysis locally without pushing to GitHub. Additionally, the existing SonarCloud setup analyzes a single project key, inconsistent with the per-service split used everywhere else in the repo (4 independent `.sln` files). We want a per-service local SonarQube workflow and a per-service SonarCloud CI pipeline, plus documentation of both.

## What Changes

- Add `sonar-install` Makefile target that installs the `dotnet-sonarscanner` global tool.
- Add `sonar-check` Makefile target that runs a per-service SonarQube analysis against a self-hosted SonarQube server, looping over the four solutions, each wrapped in `begin` → build + test (coverage) → `end`, with all analysis parameters driven by environment variables (`SONAR_HOST_URL`, `SONAR_TOKEN`, `SONAR_PROJECT_KEY_PREFIX`).
- Fix the coverage report property to consume the existing cobertura reports (`sonar.cs.cobertura.reportsPaths`), not opencover, and exclude test sources (`sonar.coverage.exclusions=**/*Tests/**`).
- Adjust the SonarCloud CI job to analyze each service under its own per-service project key, still using the SonarCloud GitHub Action (not the local Makefile targets), and only on pull requests.
- Ignore the SonarScanner local state directory (`.sonarqube/`) in `.gitignore`.
- Document in `README.md`: how to run `sonar-install`/`sonar-check` for local analysis, and that the SonarCloud and integration-test jobs run in CI only on pull requests.

## Capabilities

### New Capabilities
- `local-sonarqube-analysis`: Per-service local SonarQube analysis driven from the Makefile (`sonar-install`, `sonar-check`) against a self-hosted SonarQube server, with parameters provided via environment variables and coverage collected during the test run.

### Modified Capabilities
- `sonarqube-analysis`: Change the SonarCloud analysis from a single repository-level project key to per-service project keys, clarify that CI uses the SonarCloud GitHub Action (not the Makefile-local flow) and runs only on pull requests, and add requirements for the local SonarQube workflow.

## Impact

- `Makefile`: new `sonar-install` and `sonar-check` targets; variables `SONAR_HOST_URL`, `SONAR_TOKEN`, `SONAR_PROJECT_KEY_PREFIX`; `.PHONY` and `help` updates.
- `.github/workflows/ci.yml`: `sonarcloud` job reworked to loop per-service project keys (secrets `SONAR_ORG`, `SONAR_TOKEN` kept; per-service keys added); job stays PR-only.
- `.gitignore`: ignore `.sonarqube/`.
- `README.md`: quality tooling section documenting local SonarQube usage and CI-only-on-PR behavior.
- No change to application code or contracts.
