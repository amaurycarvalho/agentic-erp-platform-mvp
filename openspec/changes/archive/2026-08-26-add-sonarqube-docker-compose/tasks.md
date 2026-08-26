# add-sonarqube-docker-compose — Tasks

## 1. Compose stack

- [x] 1.1 Create the `sonarqube/` folder and `sonarqube/docker-compose.yml` with the `sonarqube:community` image and `postgres:17`, following the official reference (no `version:` key)
- [x] 1.2 Configure the `db` service: `POSTGRES_USER/PASSWORD/DB=sonar`, single named-volume mount `/var/lib/postgresql`, healthcheck `pg_isready -d $$POSTGRES_DB -U $$POSTGRES_USER`
- [x] 1.3 Configure the `sonarqube` service: `container_name: sonarqube`, `depends_on: db: condition: service_healthy`, `read_only: true`, `tmpfs /tmp`, `SONAR_JDBC_URL`/credentials, ports `9000:9000`, and volumes `sonarqube_data/extensions/logs/temp`
- [x] 1.4 Add a SonarQube healthcheck using `curl http://localhost:9000/api/system/status` (curl is present in the image) so `--wait` is meaningful
- [x] 1.5 Add `SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true` for dev friendliness on Docker Desktop/Mac
- [x] 1.6 Declare the named volumes (`sonarqube_data`, `sonarqube_temp`, `sonarqube_extensions`, `sonarqube_logs`, `postgresql`) and container names matching the official reference
- [x] 1.7 Validate the compose file (`docker compose -f sonarqube/docker-compose.yml config`)

## 2. Makefile targets

- [x] 2.1 Add `SONAR_COMPOSE_FILE ?= sonarqube/docker-compose.yml` variable near the other sonar variables
- [x] 2.2 Add `sonar-up` target that runs `docker compose -f $(SONAR_COMPOSE_FILE) up -d --wait` and prints the URL, `admin`/`admin` credentials with password-change reminder, and the per-service project keys
- [x] 2.3 Add `sonar-down` target that runs `docker compose -f $(SONAR_COMPOSE_FILE) down` (preserving volumes)
- [x] 2.4 Add `sonar-up` and `sonar-down` to the `.PHONY` list and to the `help` output

## 3. Documentation

- [x] 3.1 In `README.md`, extend the local SonarQube section with the flow: `make sonar-up` → login `admin`/`admin` → change password → generate token → `SONAR_TOKEN=<token> make sonar-check` → `make sonar-down`
- [x] 3.2 In `README.md`, add host requirements (Linux `sysctl -w vm.max_map_count=262144`, Docker Desktop memory) and the full-reset command (`docker compose -f sonarqube/docker-compose.yml down -v`), noting projects are auto-created on first analysis with an admin token

## 4. Verification

- [x] 4.1 Run `make sonar-up` and confirm both containers are healthy and SonarQube reports ready on `http://localhost:9000`
- [x] 4.2 Confirm first login works with `admin`/`admin` and the password-change prompt appears (manual check)
- [x] 4.3 Run `make sonar-check` with `SONAR_TOKEN` from an admin user and confirm all four per-service analyses complete (auto-creating the projects) with coverage
- [x] 4.4 Run `make sonar-down` and confirm the stack stops and volumes persist (`docker compose -f sonarqube/docker-compose.yml ps -a` shows containers, and the volumes still exist)
