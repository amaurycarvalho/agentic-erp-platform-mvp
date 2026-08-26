# add-sonarqube-docker-compose

## Why

The local SonarQube workflow (`make sonar-check`) currently requires a self-hosted SonarQube server that developers must set up manually. There is no first-class, reproducible way to spin one up locally. Providing the official `docker-compose` stack (SonarQube Community + PostgreSQL, with persistent volumes) plus `make sonar-up` / `make sonar-down` targets removes that friction and makes the local analysis flow work end-to-end out of the box.

## What Changes

- Add a `sonarqube/` folder containing a `docker-compose.yml` for SonarQube Community + PostgreSQL 17, following the official SonarSource reference (`sonarqube:community` image, hardened with `read_only: true`, `sonarqube_temp` volume and `tmpfs`, `container_name` `sonarqube`/`postgresql`, persistent named volumes, DB healthcheck + `depends_on` condition).
- Add `make sonar-up` (start the SonarQube stack and wait until ready) and `make sonar-down` (stop it, preserving volumes) targets to the Makefile, wired into `.PHONY` and `help`.
- Update `README.md` so the "Análise SonarQube local" section documents the full developer flow: `make sonar-up` → first-login `admin`/`admin` (password change) → generate a token → `SONAR_TOKEN=<token> make sonar-check` → `make sonar-down`, plus host requirements (Linux `vm.max_map_count`, Docker Desktop memory).

## Capabilities

### New Capabilities
- (none)

### Modified Capabilities
- `local-sonarqube-analysis`: extend the local SonarQube workflow with a reproducible Docker-based server (`sonarqube/docker-compose.yml`) and Makefile targets `sonar-up` / `sonar-down`, plus the README guidance for bringing the server up and using it with `sonar-check`.

## Impact

- New `sonarqube/docker-compose.yml` (SonarQube Community + PostgreSQL, volumes, hardening).
- `Makefile`: new `sonar-up` / `sonar-down` targets; `.PHONY` and `help` updates.
- `README.md`: expanded local SonarQube section with the `sonar-up`/`sonar-down` flow and first-use steps.
- No change to `sonar-install`/`sonar-check`, CI, or application code.
