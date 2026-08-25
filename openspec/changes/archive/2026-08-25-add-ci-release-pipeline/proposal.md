## Why

The repo's build tooling does not match its real stack: the `Makefile`, `ci.yml`, and `release.yml` are a leftover Node/VSIX template (`npm ci`, semgrep, Stryker, `dist/`, `CHANGELOG.md`), while the project is a .NET 8 multi-service solution (4 services, each with its own `.sln`). There is no working `install`/`test`/`clean`/`build`, no working CI quality gate, and no way to build and distribute runnable container images of the services.

## What Changes

- Rewrite Makefile targets to be dotnet-native: `install` (restore), `test` (unit), `clean`, `build` (compile Release), and `build-images` (build the 4 service Docker images tagged with `VERSION`).
- Make `quality-gate` = `install` + `test` only (test-only gate). Lint/security/mutation stays out of scope here (future change).
- Make CI run the quality gate as `install` + unit tests on push/PR, using the .NET SDK (no Node/Python/semgrep/Stryker).
- Defer `mcp-service` cross-service integration tests out of the CI gate; run them as a release step against the built stack.
- Make release (on `v*` tag) build the service images and export them as downloadable tarball assets (`docker save | gzip`) attached to the GitHub release, loadable and activatable with docker-compose or another orchestrator.

## Capabilities

### New Capabilities
- `build-tooling`: dotnet-native Makefile targets (`install`, `test`, `clean`, `build`, `build-images`) and the per-service Docker build contexts/tags.
- `ci-pipeline`: CI quality gate driven by the .NET SDK, running install + unit tests only.
- `release-pipeline`: tag-triggered build of service images, execution of MCP cross-service integration tests against the built stack, and export of image tarball assets for download.

### Modified Capabilities
<!-- No existing spec-level behavior changes; this introduces new build/CI/release tooling. -->

## Impact

- Rewrite `Makefile`; update `.github/workflows/ci.yml` and `.github/workflows/release.yml`.
- Build/run story changes from Node/VSIX to .NET: remove npm/Node/Python/semgrep/Stryker wiring from CI.
- Docker build contexts per service remain as encoded in `docker-compose.yml` (mcp-service uses the repo root for the shared `ErpAcl.Contracts`).
- Container images become distributable as tarball assets instead of a `dist/` VSIX bundle.
