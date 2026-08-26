# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### [add-observability](openspec/changes/add-observability) Establish end-to-end correlation propagation across services (agent, mcp, erp-acl, rag).

#### Added
- Add metrics for health and per-attempt retry telemetry.
- Add distributed tracing (OpenTelemetry) across HTTP and gRPC calls.

#### Changed
- Establish end-to-end correlation propagation across services (agent, mcp, erp-acl, rag).
- Introduce structured logging carrying correlation ids in every service.
- Standardize observability baselines per service as listed in `specs/tasks.md`.

### [add-shared-resilience-policies](openspec/changes/add-shared-resilience-policies) Introduce a common idempotency policy for safe re-execution across services (US-SHARED-002, EC-005).

#### Changed
- Introduce a common idempotency policy for safe re-execution across services (US-SHARED-002, EC-005).
- Standardize error taxonomy/response shape between internal services (US-SHARED-003).
- Define a controlled retry and compensation strategy for partial failures (US-SHARED-004, EC-004, NFR-003).
- Align with existing MCP error taxonomy (`validation_error`, `acl_business_error`, `acl_unavailable`) and the ACL domain errors.

### [expand-mcp-tool-catalog](openspec/changes/expand-mcp-tool-catalog) Extend the MCP tool catalog with new ERP capabilities (beyond `erp.create_order` and `erp.cancel_invoice`).

#### Changed
- Extend the MCP tool catalog with new ERP capabilities (beyond `erp.create_order` and `erp.cancel_invoice`).
- Introduce per-tool authorization so execution is allowed only for callers authorized for that tool.
- Formalize the tool contract versioning policy (incompatible changes require a new tool version).
- Preserve the existing catalog-gating, validation, and error-taxonomy behavior.

## [1.0.3] - 2026-08-26

### [2026-08-26-add-local-sonarqube-per-service](openspec/changes/archive/2026-08-26-add-local-sonarqube-per-service) SonarQube analysis is only available via SonarCloud in CI today, so developers cannot run an analysis locally without pushing to GitHub.

#### Added
- Add `sonar-install` Makefile target that installs the `dotnet-sonarscanner` global tool.
- Add `sonar-check` Makefile target that runs a per-service SonarQube analysis against a self-hosted SonarQube server, looping over the four solutions, each wrapped in `begin` → build + test (coverage) → `end`, with all analysis parameters driven by environment variables (`SONAR_HOST_URL`, `SONAR_TOKEN`, `SONAR_PROJECT_KEY_PREFIX`).

#### Changed
- Adjust the SonarCloud CI job to analyze each service under its own per-service project key, still using the SonarCloud GitHub Action (not the local Makefile targets), and only on pull requests.
- Ignore the SonarScanner local state directory (`.sonarqube/`) in `.gitignore`.
- Document in `README.md`: how to run `sonar-install`/`sonar-check` for local analysis, and that the SonarCloud and integration-test jobs run in CI only on pull requests.

#### Fixed
- Fix the coverage report property to consume the existing cobertura reports (`sonar.cs.cobertura.reportsPaths`), not opencover, and exclude test sources (`sonar.coverage.exclusions=**/*Tests/**`).

### [2026-08-26-add-sonarqube-docker-compose](openspec/changes/archive/2026-08-26-add-sonarqube-docker-compose) The local SonarQube workflow (`make sonar-check`) currently requires a self-hosted SonarQube server that developers must set up manually.

#### Added
- Add a `sonarqube/` folder containing a `docker-compose.yml` for SonarQube Community + PostgreSQL 17, following the official SonarSource reference (`sonarqube:community` image, hardened with `read_only: true`, `sonarqube_temp` volume and `tmpfs`, `container_name` `sonarqube`/`postgresql`, persistent named volumes, DB healthcheck + `depends_on` condition).
- Add `make sonar-up` (start the SonarQube stack and wait until ready) and `make sonar-down` (stop it, preserving volumes) targets to the Makefile, wired into `.PHONY` and `help`.

#### Changed
- Update `README.md` so the "Análise SonarQube local" section documents the full developer flow: `make sonar-up` → first-login `admin`/`admin` (password change) → generate a token → `SONAR_TOKEN=<token> make sonar-check` → `make sonar-down`, plus host requirements (Linux `vm.max_map_count`, Docker Desktop memory).

[Unreleased]: https://github.com/amaurycarvalho/agentic-erp-platform-mvp/compare/v1.0.3...HEAD
[1.0.3]: https://github.com/amaurycarvalho/agentic-erp-platform-mvp/releases/tag/v1.0.3

See [CHANGELOG Archive](CHANGELOG-ARCHIVE.md) for older releases.
