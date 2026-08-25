## Why

The quality gate currently is test-only (`install` + `test`). The project needs analyzers/lint, coverage, code metrics, security scanning, and SonarCloud analysis (with PR decoration and a new-code/Leak Period gate) so new contributions uphold quality without being blocked by existing technical debt. Mutation testing (Stryker.NET) is adopted but kept out of CI for now.

## What Changes

- Enable .NET SDK analyzers + `dotnet format` as the linter; add `Directory.Build.props` and `.editorconfig`.
- Add `dotnet-code-metrics` to report Maintainability Index, Lines of Code, and Cyclomatic Complexity.
- Collect coverage with Coverlet during `make test` and enforce a coverage threshold.
- Add `make security` (`dotnet list package --vulnerable/--deprecated/--outdated` + Semgrep).
- Integrate SonarCloud analysis of all service solutions with PR decoration and a new-code (Leak Period) quality gate. SonarCloud is used instead of self-hosted SonarQube.
- Add `make mutation` using Stryker.NET (manual-only for now; a dedicated/nightly job will be a later change). Replace the stale StrykerJS config.

## Capabilities

### New Capabilities
- `quality-analyzers`: .NET SDK analyzers + `dotnet format` lint and `dotnet-code-metrics` (MI, LOC, complexity).
- `coverage`: Coverlet coverage collection and threshold enforcement.
- `security-scan`: package vulnerability/deprecated/outdated checks and Semgrep SAST.
- `sonarqube-analysis`: SonarCloud analysis of the service solutions, PR decoration, and new-code gate.
- `mutation-testing`: Stryker.NET mutation testing run manually via the Makefile (not in CI).

### Modified Capabilities
<!-- No existing spec-level behavior changes; this introduces new quality tooling. -->

## Impact

- Add `Directory.Build.props`, `.editorconfig`, `stryker-config.json`, SonarCloud config (per-service project keys + `sonar-project.properties`/CLI args).
- Extend `Makefile` with `lint`, `metrics`, `coverage`, `coverage-check`, `security`, `mutation`; expand `quality-gate`.
- CI (`ci.yml`) runs the gate incl. analyses, coverage, metrics, security and SonarCloud analysis/decorate; mutation excluded.
- Adds CI secrets (`SONAR_TOKEN`, `SONAR_ORG`, `SONAR_PROJECT_KEY`).
