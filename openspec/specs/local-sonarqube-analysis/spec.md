# local-sonarqube-analysis Specification

## Purpose
Local SonarQube analysis of the four service solutions via Makefile targets (per-service `sonar-check`), against a reproducible Docker Compose stack (SonarQube Community + PostgreSQL) started with `sonar-up` and stopped with `sonar-down`, so developers can analyze code locally without pushing to GitHub.

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

### Requirement: sonar-up starts the local SonarQube stack

The `sonar-up` Makefile target SHALL start the local SonarQube stack defined in `sonarqube/docker-compose.yml` (SonarQube Community + PostgreSQL) in detached mode and wait until the server reports ready.

#### Scenario: Local SonarQube is started
- **WHEN** `make sonar-up` is run
- **THEN** the SonarQube and PostgreSQL containers are started from the `sonarqube/docker-compose.yml` file and the target waits for SonarQube to become ready

#### Scenario: First login credentials are surfaced
- **WHEN** `make sonar-up` completes
- **THEN** the target prints the web URL (`http://localhost:9000`) and the default `admin`/`admin` credentials with a password-change reminder

### Requirement: sonar-down stops the local SonarQube stack

The `sonar-down` Makefile target SHALL stop the local SonarQube stack without removing its persistent volumes, so data survives restarts.

#### Scenario: Local SonarQube is stopped preserving data
- **WHEN** `make sonar-down` is run
- **THEN** the SonarQube and PostgreSQL containers are stopped and their named volumes are preserved

### Requirement: The local SonarQube compose file is official and hardened

The `sonarqube/docker-compose.yml` file SHALL follow the official SonarSource reference: `sonarqube:community` image with `postgres:17`, `read_only: true`, the `sonarqube_temp` volume, a `tmpfs` for `/tmp`, fixed `container_name`s, persistent named volumes for SonarQube and PostgreSQL, and a `db` healthcheck with `depends_on` ordering.

#### Scenario: Compose follows the official reference
- **WHEN** `sonarqube/docker-compose.yml` is inspected
- **THEN** it uses the `sonarqube:community` image, `postgres:17`, the official hardening (read-only root filesystem, temp volume, tmpfs), and a single PostgreSQL volume mount

#### Scenario: Database readiness gates SonarQube
- **WHEN** the stack starts
- **THEN** the `db` service healthcheck (`pg_isready`) gates SonarQube startup via `depends_on: condition: service_healthy`

### Requirement: Local SonarQube usage is documented

The `README.md` SHALL document the complete local flow: `make sonar-up`, first login (`admin`/`admin`) with password change, generating an analysis token, running `make sonar-check` with `SONAR_TOKEN`, and `make sonar-down`, plus host requirements (Linux `vm.max_map_count`, Docker Desktop memory).

#### Scenario: README explains the full local flow
- **WHEN** a developer reads the SonarQube section of `README.md`
- **THEN** it explains starting the stack, the first login and token generation, running the analysis, and stopping the stack, including host requirements
