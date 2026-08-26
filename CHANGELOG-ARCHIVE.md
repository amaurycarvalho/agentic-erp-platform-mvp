# Changelog Archive

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html)

## [1.0.1] - 2026-08-25

### [2026-08-25-add-ci-release-pipeline](openspec/changes/archive/2026-08-25-add-ci-release-pipeline) Rewrite Makefile targets to be dotnet-native: `install` (restore), `test` (unit), `clean`, `build` (compile Release), and `build-images` (build the 4 service Docker images tagged with `VERSION`).

#### Changed
- Rewrite Makefile targets to be dotnet-native: `install` (restore), `test` (unit), `clean`, `build` (compile Release), and `build-images` (build the 4 service Docker images tagged with `VERSION`).
- Make `quality-gate` = `install` + `test` only (test-only gate). Lint/security/mutation stays out of scope here (future change).
- Make CI run the quality gate as `install` + unit tests on push/PR, using the .NET SDK (no Node/Python/semgrep/Stryker).
- Defer `mcp-service` cross-service integration tests out of the CI gate; run them as a release step against the built stack.
- Make release (on `v*` tag) build the service images and export them as downloadable tarball assets (`docker save | gzip`) attached to the GitHub release, loadable and activatable with docker-compose or another orchestrator.

### [2026-08-25-add-mutation-test-coverage](openspec/changes/archive/2026-08-25-add-mutation-test-coverage) Add unit tests to `Agent.Application.Tests` covering the surviving mutant groups in `ProcessAgentCommandUseCase`:

#### Added
- Add unit tests to `Agent.Application.Tests` covering the surviving mutant groups in `ProcessAgentCommandUseCase`:

#### Changed
- Introduce a recording `ILogger<T>` test helper so log-statement mutations (message template and tool-name argument) become observable and assertable.
- Treat the `return "unsupported"` string mutation as an equivalent mutant (documented in design) — it is not killable without changing production design.
- Update the `mutation-testing` spec to require that the agent-service mutation score meets the configured break threshold.

### [2026-08-25-adopt-quality-gate-tools](openspec/changes/archive/2026-08-25-adopt-quality-gate-tools) Enable .NET SDK analyzers + `dotnet format` as the linter; add `Directory.Build.props` and `.editorconfig`.

#### Added
- Add `dotnet-code-metrics` to report Maintainability Index, Lines of Code, and Cyclomatic Complexity.
- Add `make security` (`dotnet list package --vulnerable/--deprecated/--outdated` + Semgrep).
- Add `make mutation` using Stryker.NET (manual-only for now; a dedicated/nightly job will be a later change). Replace the stale StrykerJS config.

#### Changed
- Enable .NET SDK analyzers + `dotnet format` as the linter; add `Directory.Build.props` and `.editorconfig`.
- Collect coverage with Coverlet during `make test` and enforce a coverage threshold.
- Integrate SonarCloud analysis of all service solutions with PR decoration and a new-code (Leak Period) quality gate. SonarCloud is used instead of self-hosted SonarQube.

## [1.0.0] - 2026-08-25

### [2026-08-25-history-mvp-foundation](openspec/changes/archive/2026-08-25-history-mvp-foundation) Capture the architecture foundation decisions and cross-cutting invariants (ADR-001/002/003, constitution principles, REQ-FUNC-001..005).

#### Changed
- Capture the architecture foundation decisions and cross-cutting invariants (ADR-001/002/003, constitution principles, REQ-FUNC-001..005).
- Capture the implemented `erp-acl-service` behavior (US-ACL-001/002) and its gRPC contract.
- Capture the implemented `mcp-service` behavior (US-MCP-001/002, MCP-TOOL contract, error taxonomy).
- Capture the implemented `agent-service` behavior (US-AGENT-001/002).
- Capture the implemented `rag-service` behavior (US-RAG-001..004, RAG-SEARCH contract).
- No breaking changes: this is a documentation/history consolidation of behavior already present in the codebase.

[1.0.0]: https://github.com/amaurycarvalho/agentic-erp-platform-mvp/releases/tag/v1.0.0
[1.0.1]: https://github.com/amaurycarvalho/agentic-erp-platform-mvp/releases/tag/v1.0.1

See main [CHANGELOG](CHANGELOG.md) for newer releases.
