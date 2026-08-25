## Context

The repo has 4 .NET 8 services (agent-service, mcp-service, erp-acl-service, rag-service), each an independent `.sln` with layered `src/` + `tests/`. The quality gate is test-only today. Reconfigured facts:

- Coverlet (`coverlet.collector 6.0.0`) is already referenced by all 6 test projects, but `make test` does not collect coverage.
- `stryker.config.json` is a **StrykerJS (Node)** config (jest, `node_modules`, `@stryker-mutator/jest-runner`) and must be replaced by a Stryker.NET config.
- No `Directory.Build.props`, `Directory.Packages.props`, or `.editorconfig` exist, so SDK analyzers/formatting are not enabled/constrained.
- `make build` reports 0 warnings, so raising analysis strictness is achievable in principle (may surface new analyzer warnings on `AnalysisLevel=latest`).
- 4 independent `.sln` → analysis and gating are per-service.
- CI runs on GitHub (`amaurycarvalho/agentic-erp-platform-mvp`); SonarCloud is chosen (SaaS) for native PR decoration + Leak Period.

## Goals / Non-Goals

**Goals:**
- Linter via .NET SDK analyzers + `dotnet format --verify-no-changes`.
- Code metrics via `dotnet-code-metrics` (MI, LOC, cyclomatic complexity) with an MI threshold.
- Coverage collection via Coverlet + threshold enforcement.
- Security via `dotnet list package` (vulnerable/deprecated/outdated) + Semgrep.
- SonarCloud analysis of all services with PR decoration and a new-code (Leak Period) gate.
- Mutation via Stryker.NET, exposed via Makefile only (not in CI).

**Non-Goals:**
- Mutation is NOT part of the CI gate; a dedicated/nightly mutation job is a separate future change.
- No self-hosted SonarQube; SonarCloud only.
- No change to application code or contracts beyond what analyzers/metrics demand (fixes are tasks, not spec).

## Decisions

- **Analyzers**: central `Directory.Build.props` (`EnableNETAnalyzers`, `AnalysisLevel=latest`, `AnalysisMode`) + `.editorconfig` severity map. `lint` target = `dotnet build` with analyzers + `dotnet format --verify-no-changes`.
- **Metrics**: Lines of Code via auditable shell tooling; complexity, code smells, `sqale_index` and maintainability rating come from **SonarCloud** (no installable `dotnet-code-metrics` global tool exists; community alternatives violate the no-unverified-third-party rule).
- **Coverage**: `make test` uses `--collect:"XPlat Code Coverage"` (cobertura). `coverage-check` parses the cobertura `line-rate` per solution against `COVERAGE_THRESHOLD` (default 85) and fails below. SonarQube reads the same cobertura files. Test sources excluded (`sonar.coverage.exclusions` / coverage filter).
- **Security**: `security` target loops solutions running `dotnet list package --vulnerable` (fail) + `--deprecated`/`--outdated` (report), then Semgrep SAST. `dotnet-stryker` and `semgrep` are installed by `install-quality-tools` (bundled `dotnet-format`; no `dotnet-code-metrics`).
- **SonarCloud**: one SonarCloud project per service (independent gates, matches the `.sln` split). CI runs `dotnet sonarscanner begin` → build → test (coverage) → `end`; the `SonarSource/sonarcloud-github-action@v2` orchestrates and decorates PRs. Leak Period / new-code definition is set in the SonarCloud project settings (30 days or previous version). Analysis is gated on new code only.
- **Mutation**: `stryker-config.json` (Stryker.NET) per test project via reporter low/high/break thresholds; `mutation` target runs `dotnet-stryker`. Not wired into `quality-gate` or CI.
- **Quality gate ordering**: `install` → `lint` → `build` → `test` (coverage) → `coverage-check` → `metrics` → `security`; SonarCloud runs as a CI step. Mutation excluded.

## Risks / Trade-offs

- [Stricter analyzers may surface many warnings] → Mitigation: baseline pass; sweep with `.editorconfig` to drive to zero before treating as errors.
- [Coverage threshold across 4 solutions may be uneven] → Mitigation: threshold per solution/aggregate; start lower and tighten.
- [Stryker.NET is slow and heavy] → Mitigation: manual-only now, dedicated job later; scoped to a single test project at a time.
- [SonarCloud per-service projects are more projects to maintain] → Mitigation: acceptable for independent service gates; modules could consolidate later.
