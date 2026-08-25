## ADDED Requirements

### Requirement: Dependency vulnerabilities and health are scanned

`make security` SHALL run `dotnet list package --vulnerable`, `--deprecated`, and `--outdated` for all service solutions and fail on any vulnerable package.

#### Scenario: Vulnerable packages fail the gate
- **WHEN** `dotnet list package --vulnerable` reports a vulnerability
- **THEN** the security step fails

#### Scenario: Deprecated/outdated packages are reported
- **WHEN** `dotnet list package --deprecated`/`--outdated` is run
- **THEN** deprecated and outdated packages are reported for review

### Requirement: SAST scanning with Semgrep

`make security` SHALL run Semgrep SAST rules (a security ruleset) against the source code and fail on findings.

#### Scenario: Semgrep findings fail the gate
- **WHEN** Semgrep reports a security finding
- **THEN** the security step fails
