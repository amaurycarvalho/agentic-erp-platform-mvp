# local-sonarqube-analysis Specification

## Purpose
Delta spec for the local-sonarqube-analysis capability: add a reproducible local SonarQube server via Docker Compose and `sonar-up` / `sonar-down` Makefile targets, plus first-use README guidance.

## MODIFIED Requirements
### Requirement: sonar-install installs the SonarScanner

The `sonar-install` Makefile target SHALL install the `dotnet-sonarscanner` global tool so the local analysis can run.

#### Scenario: Scanner is installed
- **WHEN** `make sonar-install` is run
- **THEN** `dotnet-sonarscanner` is installed as a .NET global tool

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
