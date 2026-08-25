## ADDED Requirements

### Requirement: Test runs collect coverage

`make test` SHALL collect line/branch coverage with Coverlet (XPlat/cobertura output) for the test projects.

#### Scenario: Coverage file is produced
- **WHEN** `make test` is run
- **THEN** a cobertura coverage file is produced per solution under `TestResults/`

### Requirement: Coverage threshold is enforced

The gate SHALL enforce a line coverage threshold (`COVERAGE_THRESHOLD`, default 85) against the collected coverage and fail when below it.

#### Scenario: Below threshold fails the gate
- **WHEN** measured line coverage is below the configured threshold
- **THEN** `make coverage-check` fails

#### Scenario: At or above threshold passes
- **WHEN** measured line coverage is at or above the threshold
- **THEN** `make coverage-check` passes

### Requirement: Test code is excluded from coverage

Coverage SHALL exclude test projects and test sources so the reported coverage reflects production code only.

#### Scenario: Test code is not counted
- **WHEN** coverage is computed
- **THEN** `**/Tests/**` and test source files are excluded from the coverage report
