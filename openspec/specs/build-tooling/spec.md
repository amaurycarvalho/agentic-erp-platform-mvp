# build-tooling Specification

## Purpose
TBD - created by archiving change add-ci-release-pipeline. Update Purpose after archive.
## Requirements
### Requirement: Install restores .NET dependencies

The `install` Makefile target SHALL restore NuGet dependencies for all four service solutions so the codebase builds, using `dotnet restore`.

#### Scenario: Install restores all solutions
- **WHEN** `make install` is run
- **THEN** all four solutions (`Agent.sln`, `Mcp.sln`, `ErpAcl.sln`, `Rag.sln`) are restored with no unresolved package references

#### Scenario: Install needs no Node or Python
- **WHEN** `make install` is run
- **THEN** it does not invoke npm or any Node/Python tooling

### Requirement: Test runs the unit test suite

The `test` Makefile target SHALL run the unit test projects of all services and fail the build on any failing test, while excluding the live-infrastructure MCP cross-service integration tests from this gate.

#### Scenario: Unit tests are executed
- **WHEN** `make test` is run
- **THEN** the unit test projects of all four services run and failures are reported as a non-zero exit

#### Scenario: MCP live integration tests are not in the gate
- **WHEN** `make test` is run
- **THEN** tests requiring a live `mcp-service`/`erp-acl-service` (Category `Mcp.Integration`) are excluded

### Requirement: Clean removes build artifacts

The `clean` Makefile target SHALL remove generated build/test artifacts (bin/obj, TestResults, and image artifact output) and clean the solutions via `dotnet clean`.

#### Scenario: Clean removes outputs
- **WHEN** `make clean` is run
- **THEN** `bin/`, `obj/`, TestResults, and any exported image artifacts are removed and the solutions are cleaned

### Requirement: Build compiles solutions in Release

The `build` Makefile target SHALL compile all four solutions in Release configuration without running tests.

#### Scenario: Build compiles all solutions
- **WHEN** `make build` is run
- **THEN** all four solutions compile under Release configuration and produce runnable artifacts

### Requirement: Build images produces versioned service images

The `build-images` Makefile target SHALL build the four service Docker images, using the per-service build context required by each Dockerfile (mcp-service uses the repo root for the shared `ErpAcl.Contracts`), and tag each image with the `VERSION`.

#### Scenario: Images are built and tagged
- **WHEN** `make build-images` is run with a `VERSION`
- **THEN** `agent-service`, `mcp-service`, `erp-acl-service`, and `rag-service` images are built and tagged with `VERSION`

#### Scenario: MCP image uses the repo-root context
- **WHEN** the `mcp-service` image is built
- **THEN** the build uses the repository root context with `services/mcp-service/Dockerfile` so the cross-service `ErpAcl.Contracts` is available

