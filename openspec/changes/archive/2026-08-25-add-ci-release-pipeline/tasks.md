# Tasks

## Tasks

### build-tooling (Makefile)
- [x] Rewrite Makefile to dotnet-native targets: `install` (dotnet restore ×4), `test` (dotnet test, exclude `Category=Mcp.Integration`), `clean` (dotnet clean + rm bin/obj/TestResults/images), `build` (dotnet build -c Release ×4)
- [x] Add `build-images` target building the 4 images with per-service context (mcp uses repo-root + `services/mcp-service/Dockerfile`), tagged `<name>:$(VERSION)` and `:latest`
- [x] Redefine `quality-gate` = `install` + `test` only; remove/neut the lint/security/complexity/mutation wiring
- [x] Keep `VERSION` as the single image/release version variable

### ci-pipeline (ci.yml)
- [x] Replace setup-node/setup-python with `actions/setup-dotnet@v4` (8.0.x)
- [x] CI jobs run `make install` then `make test` on push to main and pull requests
- [x] Remove semgrep/Stryker/quality-tool steps from CI (deferred to future change)
- [x] Upload test/coverage artifacts (TestResults) on completion

### release-pipeline (release.yml)
- [x] Trigger on `v*` tag; resolve release version from the tag (e.g. `v1.2.3` → `1.2.3`)
- [x] `make build` then `make build-images` with the resolved version
- [x] Run MCP cross-service integration tests against the built stack (docker compose up + `MCP_BASE_URL`) before publishing
- [x] `docker save | gzip` each image to `images/*.tar.gz` and attach via `softprops/action-gh-release` with `files: images/`
- [x] Remove the broken `CHANGELOG.md`/`dist/` coupling; stop reading CHANGELOG
- [x] Grant `contents: write` (and `packages: write` if a registry is later added)

### documentation (README.md)
- [x] Complete the empty README sections: "Para Usuários" (`Como Instalar`, `Como Usar`) and "Para Desenvolvedores" `Docker Compose`, reflecting the new build/test/run flow
- [x] Rewrite the README developer instructions to the new Makefile targets (`install`, `test`, `clean`, `build`, `build-images`) and the docker-compose workflow
- [x] Remove/replace stale README instructions for `make lint test`, `make complexity`, `make duplication`, `make mutation-check`, `make security`, and `make install-quality-tools` (deferred to the future quality-gate change)
- [x] Add README instructions to build service images and load/activate the tarball assets (`docker save | gzip`, `docker load`, `docker-compose up`)
- [x] Fix the SDD badge link in the README to point to an existing spec under `openspec/specs/` (or remove it if the path is invalid)

### Verification
- [x] Validate change with `openspec validate add-ci-release-pipeline`
- [x] Confirm `make install`, `make test`, `make clean`, `make build`, `make build-images` are wired and documented via `make help`
