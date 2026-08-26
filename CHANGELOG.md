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

## [1.0.2] - 2026-08-26

### [2026-08-26-add-release-compose-file](openspec/changes/archive/2026-08-26-add-release-compose-file) Add a new compose file (e.g. `docker-compose.release.yml`) that runs the four services purely from the pre-built images (`image:` references to the release tags), without any `build:` blocks. It mirrors the ports, healthchecks, dependencies and network of the existing compose file.

#### Added
- Add a new compose file (e.g. `docker-compose.release.yml`) that runs the four services purely from the pre-built images (`image:` references to the release tags), without any `build:` blocks. It mirrors the ports, healthchecks, dependencies and network of the existing compose file.
- Publish that compose file as an asset of the GitHub release, alongside the four image tarballs, in the tag-triggered release workflow.

#### Changed
- Update `README.md` to remove the inline `docker-compose.yml` listing from the "Para Usuários / Como Instalar" section.
- Update `README.md` to add a short instruction telling users to download the compose file from the release and run it against the loaded images.
- Bump the release to version `1.0.2`.

[Unreleased]: https://github.com/amaurycarvalho/agentic-erp-platform-mvp/compare/v1.0.2...HEAD
[1.0.2]: https://github.com/amaurycarvalho/agentic-erp-platform-mvp/releases/tag/v1.0.2

See [CHANGELOG Archive](CHANGELOG-ARCHIVE.md) for older releases.
