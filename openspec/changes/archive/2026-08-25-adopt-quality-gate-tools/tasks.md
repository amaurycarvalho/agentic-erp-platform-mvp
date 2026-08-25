# Tasks

## Tasks

### quality-analyzers
- [x] Add root `Directory.Build.props` enabling .NET SDK analyzers (`EnableNETAnalyzers`, `AnalysisLevel=latest`, `AnalysisMode`) and add `.editorconfig` with severity mappings
- [x] Add `make lint` target: `dotnet build` with analyzers + `dotnet format --verify-no-changes` (fail on violations)
- [x] Add `make metrics` target reporting Lines of Code via auditable shell tooling (complexity/code-smells/sqale/maintainability come from SonarCloud)
- [x] Run a baseline build to fix/drive minor analyzer warnings to zero

### coverage
- [x] Make `make test` collect coverage via `--collect:"XPlat Code Coverage"` (cobertura) per solution
- [x] Add `make coverage` target and `make coverage-check` parsing cobertura `line-rate` against `COVERAGE_THRESHOLD` (default 85)
- [x] Exclude test projects/test sources from the coverage and SonarQube coverage reports

### security-scan
- [x] Add `make security` target iterating solutions: `dotnet list package --vulnerable` (fail) + `--deprecated`/`--outdated` (report)
- [x] Add Semgrep SAST step (`semgrep ci` with a security ruleset) and fail on findings

### sonarqube-analysis
- [x] Configure SonarCloud per-service project keys and `sonar-project`/CLI args (organization + token from secrets)
- [x] Wire CI to run `dotnet sonarscanner begin` → `make build` → `make test` (coverage) → `dotnet sonarscanner end` for each service
- [x] Add `SonarSource/sonarcloud-github-action@v2` step for decoration; connect the SonarCloud GitHub App for PR decoration
- [x] Set the new-code/Leak Period definition in SonarCloud project settings (previous version / 30 days)
- [x] Add CI secrets documentation: `SONAR_TOKEN`, `SONAR_ORG`, `SONAR_PROJECT_KEY`

### mutation-testing (manual only)
- [x] Replace `stryker.config.json` with a Stryker.NET config (reporters html/json, thresholds high/low/break)
- [x] Add `make mutation` target running `dotnet-stryker` per test project
- [x] Verify `make mutation` fails on a score below the break threshold

### quality-gate wiring
- [x] Expand `quality-gate` to `install` + `lint` + `build` + `test` + `coverage-check` + `metrics` + `security` (mutation excluded)
- [x] Add `make install-quality-tools` for the global tools (`dotnet-stryker`, `semgrep`; `dotnet-format` is bundled with the SDK; no `dotnet-code-metrics`)
- [x] Update `make help` and README quality-gate sections for the new targets

### Verification
- [x] Validate change with `openspec validate adopt-quality-gate-tools`
- [x] Confirm `make lint`, `make metrics`, `make test` (coverage), `make coverage-check`, `make security` work locally
