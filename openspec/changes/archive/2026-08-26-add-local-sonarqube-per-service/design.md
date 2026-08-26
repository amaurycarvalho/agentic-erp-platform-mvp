# add-local-sonarqube-per-service — Design

## Context

The repo has 4 independent .NET 8 solutions (agent, mcp, erp-acl, rag), each with `src/` + `tests/`, and the Makefile already splits test/coverage per solution via `SOLUTIONS` + `TestResults/<Name>`. Today SonarQube analysis exists only in CI, via `SonarSource/sonarcloud-github-action@v2` (`ci.yml:42-67`), which analyzes a single project key and does not run the .NET SonarScanner build/test between `begin`/`end`.

Two facts surfaced during exploration:

- The `sonarcloud-github-action@v2` is **deprecated** (archived Oct 2025) and its docs state it does **not** work for .NET. The correct SonarCloud flow for .NET is `dotnet sonarscanner begin → build → test → dotnet sonarscanner end`.
- The repo produces **cobertura** coverage (`TestResults/<Name>/**/coverage.cobertura.xml`) via `--collect:"XPlat Code Coverage"`; the CI already consumes that (`sonar.cs.cobertura.reportsPaths`).

## Goals / Non-Goals

**Goals:**
- Per-service SonarQube analysis runnable locally via Makefile (`sonar-install`, `sonar-check`) against a self-hosted SonarQube server, parameterized by environment variables.
- Per-service SonarCloud analysis in CI, using the SonarCloud-hosted flow (`begin`/`build`+`test`/`end` per service), not the deprecated single-key action.
- Coverage imported from the existing cobertura reports; test sources excluded.
- README documents local `sonar-install`/`sonar-check` and the CI-only-on-PR behavior for the SonarCloud and integration-test jobs.

**Non-Goals:**
- No change to application code or contracts.
- No dedicated SonarQube docker-compose service (local users run their own self-hosted SonarQube; `SONAR_HOST_URL` is configurable so SonarCloud can be targeted too).
- No mutation-testing or other quality-gate changes.

## Decisions

### D1 — Per-service project keys everywhere

Local and CI each use one SonarQube project per service, key = `$(SONAR_PROJECT_KEY_PREFIX)` + solution name (e.g. `agentic-erp-agent-service`). This matches the 4-solution split already used by `SOLUTIONS`, `TestResults/<Name>`, and `coverage-check`, and delivers independent quality gates per service (the intent of the archived `adopt-quality-gate-tools` design.md).
- *Alternative considered:* one aggregate project scanning all 4 solutions in a single `begin`/`end`. Rejected: it contradicts the per-service split, mixes cross-solution coverage (mcp references shared `ErpAcl.Contracts`), and would later need module refactoring.

### D2 — Refactor `make test` into a reusable `test-sln` helper

Extract the current per-solution test+coverage recipe into `test-sln` (parameterized by `SLN`), and make `test` a loop over `SOLUTIONS` calling `$(MAKE) test-sln SLN=$$sln`. `sonar-check` reuses `test-sln` per service so the analysis runs the exact same build + test + coverage command as the normal gate — no drift between `make test` and the SonarQube run.
- *Alternative considered:* duplicate the `dotnet test` recipe inline in `sonar-check`. Rejected: duplicated flags (`--filter`, `--results-directory`, `--collect`, `--settings`, `--logger`) would drift.

### D3 — `begin → build+test → end` with guaranteed `end`

SonarScanner for .NET requires `begin` before any build and `end` after. If a build/test fails, `end` must still run or the server keeps a dangling analysis session. Local: wrap each service run in a subshell with an `EXIT` trap that calls `dotnet sonarscanner end`, so it runs on success **and** failure. CI: the `end` step uses `if: always()` for the same reason.

### D4 — Coverage from existing cobertura reports, scoped per service

`sonar-check` passes `-d:sonar.cs.cobertura.reportsPaths=TestResults/<name>/**/coverage.cobertura.xml` (per-service scope) and `-d:sonar.coverage.exclusions=**/*Tests/**`. The CI job keeps the repo-wide glob `TestResults/**/coverage.cobertura.xml`. **Not** `sonar.cs.opencover.reportsPaths` — the repo produces cobertura only, so opencover would find nothing.

### D5 — Environment-driven parameters with defaults and fail-fast

| Variable | Local default | CI source |
|---|---|---|
| `SONAR_HOST_URL` | `http://localhost:9000` (self-hosted) | `https://sonarcloud.io` |
| `SONAR_TOKEN` | required — `sonar-check` fails fast with a clear message if unset | `secrets.SONAR_TOKEN` |
| `SONAR_PROJECT_KEY_PREFIX` | `agentic-erp-` | `secrets.SONAR_PROJECT_KEY_PREFIX` |
| `SONAR_ORG` | — (local SonarQube needs no org) | `secrets.SONAR_ORG` (SonarCloud only) |

`sonar-check` also passes `/v:$(VERSION)` (the modern scanner syntax for the project version; `-d:sonar.projectVersion` is rejected by scanner ≥ 11) so analyses are versioned consistently with images/releases.

### D6 — CI keeps SonarCloud, moves to the proper .NET flow, PR-only

Replace the deprecated single-key `sonarcloud-github-action@v2` with a matrix job over the 4 services running: `dotnet sonarscanner begin /k:<per-service-key>` (against `sonarcloud.io` + `sonar.organization`) → `make test-sln SLN=<service>.sln` → `dotnet sonarscanner end`. The job keeps `if: github.event_name == 'pull_request'` and `SONAR_ORG`/`SONAR_TOKEN`/`SONAR_PROJECT_KEY_PREFIX` secrets. This keeps analysis on SonarCloud (SaaS) — the local Makefile `sonar-check` flow is **not** used in CI.
- *Alternative considered:* keep the action and just add `-Dsonar.projectKey` per matrix row. Rejected: the action is deprecated and unsupported for .NET; it cannot run build/test between `begin`/`end`, so coverage and analysis would be empty.

### D7 — Ignore local scanner state

`.gitignore` gains `.sonarqube/` so the scanner state directory written to the repo root by local runs is not committed.

### D8 — README documentation

Add a "SonarQube" subsection under the quality tooling docs covering: installing the scanner (`make sonar-install`), running a per-service local analysis (`make sonar-check` with `SONAR_HOST_URL`/`SONAR_TOKEN`/`SONAR_PROJECT_KEY_PREFIX`, requirement of a running self-hosted SonarQube), and the CI note that the SonarCloud and integration-test jobs run **only on pull requests** while the quality-gate job runs on push to `main` and on PRs.

## Risks / Trade-offs

- [Deprecated GitHub Action used today] → Mitigation: replaced in this change with the supported `dotnet sonarscanner` begin/build/test/end flow against SonarCloud.
- [Per-service SonarCloud projects add maintenance] → Mitigation: acceptable for independent service gates (already the archived intent); keys derive from a single prefix secret.
- [Dangling SonarQube analysis session if `end` never runs] → Mitigation: local `EXIT` trap; CI `end` step with `if: always()`.
- [`dotnet-sonarscanner` must match the SonarQube server version family] → Mitigation: document that local SonarQube should be a recent version; `sonar-install` installs the latest scanner.
- [mcp-service coverage includes shared `ErpAcl.Contracts` sources] → Mitigation: acceptable; matches the per-solution test split already in place, and test sources are excluded from coverage.

## Migration Plan

1. Refactor Makefile: add `test-sln`, redefine `test` as a loop, add `sonar-install`/`sonar-check` and vars, update `.PHONY` + `help`.
2. Run `make test` to confirm the refactor preserves behavior/coverage output paths.
3. Update `ci.yml` sonarcloud job to the per-service matrix + begin/test-sln/end steps.
4. Add `.sonarqube/` to `.gitignore`; document in `README.md`.
5. Verify locally with `make sonar-install` and `make sonar-check` against a running self-hosted SonarQube; verify CI on the next pull request (SonarCloud + integration-test only on PRs).
6. Rollback: revert the Makefile refactor (keep `test`), the ci.yml job, `.gitignore`, and README edits — no schema/data migration involved.
