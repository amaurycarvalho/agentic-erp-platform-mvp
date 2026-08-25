## ADDED Requirements

### Requirement: SDK analyzers are enabled as the linter

The .NET SDK analyzers SHALL be enabled (via `Directory.Build.props` with `AnalysisLevel`/`EnableNETAnalyzers` and an `.editorconfig` for severity) so that source code is linted at build time.

#### Scenario: Lint gate enforces analyzer rules
- **WHEN** `make lint` (or build) runs with analyzers enabled
- **THEN** formatting and analyzer violations are reported
- **AND** the step fails when violations are found

### Requirement: Formatting is verified

The linter SHALL run `dotnet format --verify-no-changes` so style and analyzer findings are checked without auto-fixing.

#### Scenario: Format check gates clean code
- **WHEN** the code has style/analyzer violations
- **THEN** `dotnet format --verify-no-changes` exits non-zero and the gate fails

### Requirement: Code metrics are reported

`make metrics` SHALL report source-code metrics for the four services (Lines of Code and derived indicators) using an auditable mechanism, and complexity/code-smells/sqale/maintainability SHALL come from SonarCloud analysis rather than an unvalidated third-party metrics tool.

#### Scenario: LOC is reported locally
- **WHEN** `make metrics` is run
- **THEN** a LOC report is produced for each service using auditable shell tooling

#### Scenario: Complexity and maintainability come from SonarCloud
- **WHEN** quality analysis runs
- **THEN** complexity, code smells, sqale and the maintainability rating are provided by SonarCloud, not an external metrics dependency
