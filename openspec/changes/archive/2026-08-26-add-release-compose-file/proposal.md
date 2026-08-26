## Why

A user who wants to install the platform from the published images (instead of building from source) currently has to either hand-write a `docker-compose.yml` or reuse the repository one, which is source-oriented (`build:` blocks) and therefore does not run against images loaded from the release tarballs. The repository `docker-compose.yml` must stay untouched, so the release needs its own compose file that references the published images.

## What Changes

- Add a new compose file (e.g. `docker-compose.release.yml`) that runs the four services purely from the pre-built images (`image:` references to the release tags), without any `build:` blocks. It mirrors the ports, healthchecks, dependencies and network of the existing compose file.
- Publish that compose file as an asset of the GitHub release, alongside the four image tarballs, in the tag-triggered release workflow.
- Update `README.md`:
  - Remove the inline `docker-compose.yml` listing from the "Para Usuários / Como Instalar" section.
  - Add a short instruction telling users to download the compose file from the release and run it against the loaded images.
- Bump the release to version `1.0.2`.

## Capabilities

### New Capabilities

- `release-compose-asset`: A versioned compose file published as a release asset that starts the four services from the images loaded from the release tarballs.

### Modified Capabilities

- `release-pipeline`: the tag-triggered release now also attaches the release compose file as an asset, and the "artifacts are activatable with docker-compose" requirement is extended to cover the compose-file asset.

## Impact

- `.github/workflows/release.yml`: add the release compose file to the published assets.
- New file `docker-compose.release.yml` at the repository root (the existing `docker-compose.yml` is NOT modified).
- `README.md`: update the user installation instructions.
- `Makefile`: `VERSION ?=` bumped to `1.0.2` for release `1.0.2`.
