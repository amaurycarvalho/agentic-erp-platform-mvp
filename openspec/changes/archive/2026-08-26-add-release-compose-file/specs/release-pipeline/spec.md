# release-pipeline Specification

## Purpose
Delta for the release-pipeline capability: the tag-triggered release now also publishes the release compose file as an asset and extends the docker-compose activation requirement to cover it.

## ADDED Requirements

### Requirement: Release attaches the release compose file

The tag-triggered release SHALL publish the release compose file (`docker-compose.release.yml`) as a downloadable asset alongside the four service image tarballs.

#### Scenario: Compose file is attached to the release
- **WHEN** a `v*` tag is pushed and the release is created
- **THEN** the release assets include `docker-compose.release.yml` in addition to the four `*-service.tar.gz` tarballs

## MODIFIED Requirements

### Requirement: Artifacts are activatable with docker-compose

The exported image tarballs SHALL be loadable with `docker load` and usable by `docker-compose` (or another orchestrator) to run the services. The release SHALL also provide the release compose file so a user can start the full stack from the loaded images without writing a compose file.

#### Scenario: Tarballs load and run with docker-compose
- **WHEN** a user downloads a tarball and runs `docker load` on it
- **THEN** the image is available locally and can be referenced by `docker-compose` to run the corresponding service

#### Scenario: Full stack starts from the release compose file
- **WHEN** a user loads all four image tarballs, retags them to `:latest`, and runs `docker-compose -f docker-compose.release.yml up -d`
- **THEN** the four services start from the loaded images
