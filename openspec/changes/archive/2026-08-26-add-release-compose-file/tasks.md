## 1. Release compose file

- [x] 1.1 Create `docker-compose.release.yml` at the repository root mirroring `docker-compose.yml` topology (container names, ports, healthchecks, `depends_on`, `ErpAcl__GrpcAddress` for `mcp-service`, `agentic-network`) but replacing every `build:` block with an `image: <service>:latest` reference
- [x] 1.2 Validate the file parses with `docker-compose -f docker-compose.release.yml config` (no build directives, topology matches the dev file)

## 2. Release workflow

- [x] 2.1 Update `.github/workflows/release.yml` so the `softprops/action-gh-release` step attaches `docker-compose.release.yml` alongside `images/*.tar.gz`

## 3. Documentation

- [x] 3.1 In `README.md`, remove the inline `docker-compose.yml` listing from the "Para Usuários / Como Instalar" section
- [x] 3.2 Add a short instruction in the same section telling users to download `docker-compose.release.yml` from the release and run `docker-compose -f docker-compose.release.yml up -d` after loading and retagging the images to `:latest`

## 4. Version bump

- [x] 4.1 Bump `VERSION ?=` to `1.0.2` in the `Makefile`
