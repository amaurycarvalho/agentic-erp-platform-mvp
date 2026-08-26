# release-pipeline Specification

## Purpose
TBD - created by archiving change add-ci-release-pipeline. Update Purpose after archive.
## Requirements
### Requirement: Tag-triggered release builds service images

On a `v*` tag, the release workflow SHALL build the service images (via `build-images`, tagged with the tag version) before publishing artifacts.

#### Scenario: Release builds images from a tag
- **WHEN** a `v*` tag is pushed
- **THEN** the release workflow builds the four service images tagged with the release version

### Requirement: MCP cross-service integration runs during release

Before publishing assets, the release pipeline SHALL execute the `mcp-service` cross-service integration tests against the built stack (mcp + erp-acl), so the agent -> mcp -> acl chain is validated for the release.

#### Scenario: MCP integration runs against the built stack
- **WHEN** the release pipeline publishes assets
- **THEN** the `mcp-service` integration tests run against the freshly built `mcp-service` and `erp-acl-service` before publishing

### Requirement: Release exports downloadable image tarballs

The release SHALL export each service image as a compressed tarball (`docker save | gzip`) and attach the tarballs to the GitHub release as downloadable assets.

#### Scenario: Tarball assets are attached
- **WHEN** the release is created
- **THEN** each service image is attached to the GitHub release as a `.tar.gz` asset

### Requirement: Release attaches the release compose file

The tag-triggered release SHALL publish the release compose file (`docker-compose.release.yml`) as a downloadable asset alongside the four service image tarballs.

#### Scenario: Compose file is attached to the release
- **WHEN** a `v*` tag is pushed and the release is created
- **THEN** the release assets include `docker-compose.release.yml` in addition to the four `*-service.tar.gz` tarballs

### Requirement: Artifacts are activatable with docker-compose

The exported image tarballs SHALL be loadable with `docker load` and usable by `docker-compose` (or another orchestrator) to run the services. The release SHALL also provide the release compose file so a user can start the full stack from the loaded images without writing a compose file.

#### Scenario: Tarballs load and run with docker-compose
- **WHEN** a user downloads a tarball and runs `docker load` on it
- **THEN** the image is available locally and can be referenced by `docker-compose` to run the corresponding service

#### Scenario: Full stack starts from the release compose file
- **WHEN** a user loads all four image tarballs, retags them to `:latest`, and runs `docker-compose -f docker-compose.release.yml up -d`
- **THEN** the four services start from the loaded images

### Requirement: Release version is derived from the tag

The release pipeline SHALL derive the version from the `v*` tag and use it to tag images and the GitHub release, without relying on any unset environment variable.

#### Scenario: Version comes from the tag
- **WHEN** a `v1.2.3` tag is pushed
- **THEN** the release is named and images are tagged with `1.2.3`

