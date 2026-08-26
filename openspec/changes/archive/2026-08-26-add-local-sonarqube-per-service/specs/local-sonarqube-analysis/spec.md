# local-sonarqube-analysis Specification

## Purpose
Run SonarQube analysis locally, per-service, against a self-hosted SonarQube server via Makefile targets, so developers can analyze code without pushing to GitHub.

## Requirements
### Requirement: sonar-install installs the SonarScanner

The `sonar-install` Makefile target SHALL install the `dotnet-sonarscanner` global tool so the local analysis can run.

#### Scenario: Scanner is installed
- **WHEN** `make sonar-install` is run
- **THEN** `dotnet-sonarscanner` is installed as a .NET global tool

### Requirement: sonar-check runs a per-service SonarQube analysis

The `sonar-check` Makefile target SHALL analyze each of the four service solutions (`Agent.sln`, `Mcp.sln`, `ErpAcl.sln`, `Rag.sln`) against a self-hosted SonarQube server, one SonarQube project per service, wrapping each solution in `sonarscanner begin` → build + test → `sonarscanner end`.

#### Scenario: Each solution is analyzed in its own project
- **WHEN** `make sonar-check` is run
- **THEN** each service solution is analyzed and published to its own SonarQube project key under the configured host

#### Scenario: Analysis runs build and tests
- **WHEN** `make sonar-check` is run
- **THEN** both a build and the test run (with coverage collection) happen between `begin` and `end` for every solution

### Requirement: Analysis parameters come from environment variables

The local analysis SHALL take its server URL, token, and project key prefix from environment variables (`SONAR_HOST_URL`, `SONAR_TOKEN`, `SONAR_PROJECT_KEY_PREFIX`), with a sensible default for the host, and SHALL fail fast with a clear message when the required token is missing.

#### Scenario: Default host is local SonarQube
- **WHEN** `make sonar-check` is run without `SONAR_HOST_URL`
- **THEN** the analysis targets `http://localhost:9000`

#### Scenario: Missing token fails fast
- **WHEN** `make sonar-check` is run without `SONAR_TOKEN`
- **THEN** the target exits non-zero with a message indicating `SONAR_TOKEN` is required

### Requirement: Coverage comes from the cobertura reports

The local analysis SHALL consume the cobertura coverage reports produced by the test run (`sonar.cs.cobertura.reportsPaths`), and SHALL exclude test sources from the analysis (`sonar.coverage.exclusions=**/*Tests/**`).

#### Scenario: Cobertura coverage is imported
- **WHEN** `make sonar-check` runs the tests
- **THEN** the coverage data is read from the generated `coverage.cobertura.xml` reports under `TestResults/`

#### Scenario: Test sources are excluded
- **WHEN** `make sonar-check` publishes the analysis
- **THEN** test projects are not counted in the coverage analysis

### Requirement: Local scanner state is ignored

The local analysis SHALL write its scanner state under a directory that is ignored by version control (`.sonarqube/`), so local runs do not pollute the repository.

#### Scenario: Scanner state is not committed
- **WHEN** `make sonar-check` is run locally
- **THEN** the `.sonarqube/` directory it creates is ignored by git

### Requirement: Local SonarQube usage is documented

The `README.md` SHALL document how to install and run the local SonarQube analysis (`sonar-install` / `sonar-check`) including the environment variables and the expectation of a running self-hosted SonarQube server.

#### Scenario: README explains local analysis
- **WHEN** a developer reads the quality tooling section of `README.md`
- **THEN** it explains the `sonar-install` and `sonar-check` targets, the required environment variables, and that a self-hosted SonarQube server must be running
