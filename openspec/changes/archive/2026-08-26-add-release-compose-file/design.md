## Context

The repository ships a source-oriented `docker-compose.yml` that builds every service from its `Dockerfile` (`build:` blocks). This file is used by developers and by the CI/integration flows. Users who install from the released images (downloaded tarballs + `docker load`) currently have no compose file that references the pre-built images — the repository file ignores loaded images and rebuilds from source instead.

The tag-triggered release workflow (`.github/workflows/release.yml`) publishes four `*-service.tar.gz` assets. The next release is `1.0.2`.

Constraints:
- The existing `docker-compose.yml` must NOT be modified (it is the source-of-truth for dev/CI usage).
- The release must remain activatable via `docker-compose` using only the published artifacts.

## Goals / Non-Goals

**Goals:**
- Provide a versioned compose file that runs the four services purely from the images loaded from the release tarballs (no `build:` blocks, no source checkout needed).
- Publish that compose file as a GitHub release asset alongside the four tarballs.
- Update `README.md` user-install docs to reference downloading the compose file instead of inlining a full listing.
- Target release `1.0.2`.

**Non-Goals:**
- Modifying the existing `docker-compose.yml`.
- Changing the CI/dev compose behavior, ports, healthchecks, or service topology.
- Publishing to a container registry (Docker Hub / GHCR); images remain distributed as tarballs.

## Decisions

### D1. New file `docker-compose.release.yml` at the repository root

A static compose file checked into the repo, named `docker-compose.release.yml`, that is a copy of the service topology (ports, healthchecks, `depends_on`, `ErpAcl__GrpcAddress`, `agentic-network`) but replaces every `build:` block with an `image:` reference to `:latest`.

Rationale: keeping the file at the root makes it easy to attach as a release asset and easy for users to run with `docker-compose -f docker-compose.release.yml up -d`. A dedicated name prevents confusion with the dev file.

Alternatives considered:
- Reusing `docker-compose.yml` by overlaying with `-f` and `docker-compose.override.yml` — rejected: `image` and `build` conflict when both are present, and overriding would pollute the dev file's intent.
- Generating the file in CI from a template with the version baked in (e.g. `envsubst`) — rejected for now: adds workflow complexity; a static file with `:latest` tags is simpler and the user-flow already retags loaded images to `latest` (same pattern used in the developer docs).

### D2. Images referenced as `:latest`; users retag loaded images

The tarballs export images tagged with the release version (e.g. `agent-service:1.0.2`). The compose file references `agent-service:latest` (and so on), and the README instructs users to retag each loaded image to `latest` before `up`.

Rationale: keeps the compose file static across releases while remaining compatible with the tarball distribution model. The retag is a single `docker tag` per service, consistent with the already-documented developer flow.

Alternative considered: referencing the exact version tag in the compose file — rejected because it would require templating the file per release in CI.

### D3. Attach the compose file as a release asset

Update `.github/workflows/release.yml` to publish `docker-compose.release.yml` in addition to `images/*.tar.gz`, so a user can download everything needed (four tarballs + compose file) from a single release.

### D4. README: instruct to download the compose file

Replace the inline `docker-compose.yml` listing in the "Para Usuários / Como Instalar" section with text explaining that the compose file is available as a release asset and must be downloaded alongside the tarballs, then run with `docker-compose -f docker-compose.release.yml up -d`.

## Risks / Trade-offs

- [Loaded images carry the version tag, not `latest`; users forget the retag step] → The README shows the retag loop explicitly before `docker-compose up`, matching the existing developer-flow pattern.
- [Static file references `latest`, which is a moving tag if users pull a newer release] → Each release's tarballs and compose file are versioned together under that release's assets; a user working from release N consistently gets `:latest` from that release's tarballs.
- [Two compose files could diverge over time] → `docker-compose.release.yml` is intentionally a mirror of the service topology; the implementation task includes mirroring ports/healthchecks/dependencies and the release workflow keeps the two in sync during this change.
