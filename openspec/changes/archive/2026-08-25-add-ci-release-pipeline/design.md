## Context

Current state: the `Makefile`, `ci.yml`, and `release.yml` are a Node/VSIX template (npm ci, semgrep, Stryker, VSIX `dist/` output, `CHANGELOG.md` reference) that does not match the .NET 8 repo. The repo has 4 services (`agent-service`, `mcp-service`, `erp-acl-service`, `rag-service`), each with its own `.sln` and layered `src/` + `tests/`, plus a `docker-compose.yml` that already encodes the correct per-service Docker build context.

Constraints discovered:
- `mcp-service` is the only cross-service reference: `Mcp.Infrastructure` → `erp-acl-service/src/ErpAcl.Contracts`. Its Dockerfile uses **repo-root context**; the other three use their own service folder.
- `shared/Shared` is unused; ignore it.
- `Mcp.Integration.Tests` POST to a live `http://localhost:8082` (`MCP_BASE_URL`), so it cannot run in the CI gate. `Rag.Integration.Tests` is self-contained (`WebApplicationFactory<Program>`); `Agent/App.Tests`, `ErpAcl/App.Tests`, `ErpAcl/Contract.Tests` are self-contained.
- `CHANGELOG.md` and `dist/` do not exist.

## Goals / Non-Goals

**Goals:**
- Dotnet-native `install`/`test`/`clean`/`build`/`build-images` Makefile targets.
- CI quality gate = `install` + unit `test` only, using .NET 8.
- Release builds service images and exports downloadable tarball assets activatable with docker-compose, running MCP cross-service integration first.

**Non-Goals:**
- No lint, security, complexity, or mutation gates here (deferred to a later change).
- No change to application code, `*.proto` contracts, or `docker-compose.yml` runtime definitions.
- No change to `shared/Shared`.

## Decisions

- **Build via `docker-compose build` or explicit per-service build.** Reuse the build context mapping already present: 3 services use their own folder, `mcp-service` uses the repo root + `services/mcp-service/Dockerfile`. The `build-images` target builds and tags each image as `<name>:$(VERSION)` (and `:latest`).
- **`test` = unit tests only.** Use `dotnet test --filter "Category!=Mcp.Integration"` (or a property/`Trait` exclusion). Live-stack MCP tests are excluded from CI per D1=C.
- **`install` = `dotnet restore`** for all four solutions; drop npm/Node/Python.
- **CI = setup-dotnet@v4 (8.0.x)**, checkout, make install, make test. Remove setup-node/setup-python and semgrep/Stryker.
- **Release = tarball assets (D2=B).** On tag, `make build-images`, run MCP integration tests against the built stack (`docker compose up` + `MCP_BASE_URL`), then `docker save | gzip` each image into `images/*.tar.gz` and attach via `softprops/action-gh-release` with `files: images/`. Version derived from the tag (repo tag `vX.Y.Z` → `X.Y.Z`), not an empty `env`.
- **`clean`** = `dotnet clean` per solution + `rm -rf **/bin **/obj TestResults images`.
- **Remove the broken `CHANGELOG.md`/`dist/` coupling** in release.yml; stop reading CHANGELOG, retarget `files:` to the tarball dir.

## Risks / Trade-offs

- [Deferring MCP live-stack tests from CI reduces pre-merge chain confidence] → Mitigation: they run in release (D1=C) against the actual built stack; unit + RAG/ACL self-contained tests still gate CI.
- [Building images in both release and (optionally) local duplicates work] → Mitigation: `build-images` is a single reusable target; CI does not build images.
- [Tarball assets are heavy and not incrementally pullable] → Mitigation: acceptable for this MVP; registry distribution can be added later without changing targets.
