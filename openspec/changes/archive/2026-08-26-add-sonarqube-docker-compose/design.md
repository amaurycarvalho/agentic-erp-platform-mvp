# add-sonarqube-docker-compose — Design

## Context

`add-local-sonarqube-per-service` (complete) added `sonar-install` / `sonar-check` (per-service, self-hosted, cobertura) but left the server provisioning to the developer. `sonar-check` defaults to `http://localhost:9000` — the same port the official SonarQube Community container exposes. This change adds a reproducible, persistent local stack via Docker Compose plus `sonar-up` / `sonar-down` convenience targets.

Verified facts from exploration:
- Docker Hub tags (2026): `sonarqube:community` is the current-latest Community image (`26.x`); `lts-community` also exists. The official `docker-sonarqube` example compose uses `sonarqube:community` + `postgres:17`.
- The `sonarqube:community` image (built from `eclipse-temurin:25-jdk-noble`) installs `curl`, so a SonarQube healthcheck via `curl http://localhost:9000/api/system/status` is available.
- The official example compose has no `version:` key (Compose v2), no SonarQube healthcheck, but includes: `read_only: true`, `sonarqube_temp` volume, `tmpfs` for `/tmp`, `container_name` `sonarqube`/`postgresql`, single PostgreSQL volume mount `/var/lib/postgresql`, `db` healthcheck (`pg_isready`) + `depends_on: condition: service_healthy`.

## Goals / Non-Goals

**Goals:**
- Reproducible local SonarQube Community + PostgreSQL 17 stack in `sonarqube/docker-compose.yml`, following the official reference (including hardening).
- `make sonar-up` (start + wait ready) and `make sonar-down` (stop, preserve data) Makefile targets.
- README documenting the full developer flow and host requirements.

**Non-Goals:**
- No change to `sonar-install`/`sonar-check`, CI/SonarCloud, or application code.
- No automated token/project provisioning (requires the interactive first-login password change; documented instead).
- No production use of this stack (local dev only).

## Decisions

### D1 — Compose file based on the official reference, decisions per user
- Image `sonarqube:community` (matches official example) with `postgres:17`.
- Keep the official hardening: `read_only: true`, `sonarqube_temp` volume, `tmpfs /tmp:size=256M,mode=1777`, `container_name` `sonarqube`/`postgresql`.
- Single PostgreSQL volume mount `postgresql:/var/lib/postgresql` (avoids the double-mount bug from the original Option 2 sketch).
- No `version:` key (Compose v2 format).
- Dev-friendly additions: a SonarQube healthcheck using `curl` (present in the image) so `sonar-up --wait` returns when ready; `SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true` to avoid the `vm.max_map_count` failure on Docker Desktop/Mac (where `sysctl` is not directly settable); the Linux-native `sysctl` fix documented in README.

### D2 — `sonar-up` / `sonar-down` targets
- Add `SONAR_COMPOSE_FILE ?= sonarqube/docker-compose.yml` variable.
- `sonar-up`: `docker compose -f $(SONAR_COMPOSE_FILE) up -d --wait` then print `http://localhost:9000`, `admin`/`admin`, password-change reminder, and the per-service project keys that `sonar-check` will create.
- `sonar-down`: `docker compose -f $(SONAR_COMPOSE_FILE) down` (no `-v` — volumes persist). A full reset is documented as `docker compose -f sonarqube/docker-compose.yml down -v`.
- Both targets added to `.PHONY` and `help`.

### D3 — README developer flow
Extend the "Análise SonarQube local" subsection with:
1. `make sonar-up`
2. First login `http://localhost:9000` → `admin`/`admin` → change password
3. My Account → Security → Tokens → Generate (admin token ⇒ the four `agentic-erp-<service>` projects are auto-created on first analysis)
4. `SONAR_TOKEN=<token> make sonar-check`
5. `make sonar-down` (data persists; reset with `down -v`)
Plus host notes: Linux `sysctl -w vm.max_map_count=262144`, Docker Desktop memory (≥ 2–4 GB), and that credentials are local-dev defaults (not for production).

## Risks / Trade-offs

- [SonarQube needs ~2 GB RAM and a writable `/tmp`/data dir] → Mitigation: Docker Desktop memory note in README; hardening uses `tmpfs` for `/tmp` and named volumes for data.
- [`vm.max_map_count` failure on Linux hosts] → Mitigation: `SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true` in the compose (dev), plus the `sysctl` fix documented for native-Linux users.
- [First analysis may fail if the token user lacks "Create Projects" permission] → Mitigation: README instructs generating the token as `admin` (has the permission by default), and notes the projects are auto-created on first analysis.
- [`container_name` collisions if another stack uses `sonarqube`/`postgresql`] → Mitigation: accepted for local dev; names match the official reference.
- [Compose file diverges slightly from the app stack's legacy `version: "3.9"` style] → Mitigation: intentional — new file uses modern Compose v2 format per the official reference.

## Migration Plan

1. Add `sonarqube/docker-compose.yml` (official-reference stack + SonarQube healthcheck + dev ES env).
2. Add `SONAR_COMPOSE_FILE`, `sonar-up`, `sonar-down` to the Makefile (`.PHONY` + `help`).
3. Update the README local SonarQube section with the full flow and host requirements.
4. Verify: `make sonar-up` brings the stack ready; first login works; `make sonar-check` with an admin token analyzes all four services; `make sonar-down` stops it preserving volumes.
5. Rollback: remove the `sonarqube/` folder and revert the Makefile/README edits — no data migration involved (volumes are local-only).
