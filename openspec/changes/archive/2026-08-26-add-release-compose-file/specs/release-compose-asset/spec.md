# release-compose-asset Specification

## Purpose
Defines the versioned compose file distributed with each release that runs the platform services from pre-built images loaded from the release tarballs.

## Requirements

### Requirement: Release compose file runs services from pre-built images

The release compose file SHALL define the four services (`agent-service`, `mcp-service`, `erp-acl-service`, `rag-service`) using only `image:` references to the pre-built images, without any `build:` blocks.

#### Scenario: Compose file has no build directives
- **WHEN** a user inspects the release compose file
- **THEN** every service references an `image:` and no service contains a `build:` block

#### Scenario: Services start from loaded images
- **WHEN** a user loads the four image tarballs and runs the release compose file with `docker-compose up -d`
- **THEN** all four services start from the loaded images without building from source

### Requirement: Release compose file mirrors the dev topology

The release compose file SHALL preserve the service topology of the repository compose file: the same container names, ports, healthchecks, `depends_on` ordering, `ErpAcl__GrpcAddress` configuration for `mcp-service`, and the shared `agentic-network` network.

#### Scenario: Topology matches the dev compose file
- **WHEN** a user compares the release compose file with the repository `docker-compose.yml`
- **THEN** the service topology (ports, healthchecks, dependencies, environment, network) matches, with only the `build:` blocks replaced by `image:` references

### Requirement: Release compose file references `:latest` image tags

The release compose file SHALL reference each service image by its `:latest` tag, so users retag the version-tagged images loaded from the tarballs (e.g. `docker tag agent-service:1.0.2 agent-service:latest`) and run the stack without editing the file.

#### Scenario: Default image tags are latest
- **WHEN** a user downloads the release compose file
- **THEN** every service `image:` value points to `<service-name>:latest`
