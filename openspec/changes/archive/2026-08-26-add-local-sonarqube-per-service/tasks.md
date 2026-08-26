# add-local-sonarqube-per-service — Tasks

## 1. Makefile refactor

- [x] 1.1 Extract the per-solution test+coverage recipe from `test` into a reusable `test-sln` target driven by `SLN` (keeping `--filter "Category!=Mcp.Integration"`, `--results-directory TestResults/<name>`, `--collect:"XPlat Code Coverage"`, `--settings CodeCoverage.runsettings`, `--logger "trx;LogFileName=results.trx"`)
- [x] 1.2 Redefine `test` as a loop over `SOLUTIONS` calling `$(MAKE) test-sln SLN=$$sln` with the same echo/failure behavior
- [x] 1.3 Verify `make test` still produces `TestResults/<Name>/**/coverage.cobertura.xml` identically and `make coverage-check` still passes

## 2. Makefile sonar targets

- [x] 2.1 Add variables `SONAR_HOST_URL ?= http://localhost:9000`, `SONAR_TOKEN ?=`, `SONAR_PROJECT_KEY_PREFIX ?= agentic-erp-` under the Quality targets section
- [x] 2.2 Add `sonar-install` target that installs the `dotnet-sonarscanner` global tool (`dotnet tool install --global dotnet-sonarscanner`)
- [x] 2.3 Add `sonar-check` target that fails fast with a clear message when `SONAR_TOKEN` is unset
- [x] 2.4 In `sonar-check`, loop over `SOLUTIONS` deriving `name` and per-service key (`$(SONAR_PROJECT_KEY_PREFIX)$$name`), and run per service a single-shell `begin → $(MAKE) test-sln → end` sequence guarded by an `EXIT` trap so `dotnet sonarscanner end /d:sonar.token="$(SONAR_TOKEN)"` runs on success and failure
- [x] 2.5 Pass to `begin`: `/k:<key>`, `-d:sonar.host.url="$(SONAR_HOST_URL)"`, `-d:sonar.token="$(SONAR_TOKEN)"`, `-d:sonar.projectVersion="$(VERSION)"`, `-d:sonar.cs.cobertura.reportsPaths="TestResults/$$name/**/coverage.cobertura.xml"`, `-d:sonar.coverage.exclusions="**/*Tests/**"`
- [x] 2.6 Add `sonar-install` and `sonar-check` to the `.PHONY` list and to the `help` output

## 3. CI SonarCloud per-service

- [x] 3.1 Rework the `sonarcloud` job in `.github/workflows/ci.yml` into a matrix over the four services (`agent-service`, `mcp-service`, `erp-acl-service`, `rag-service`), keeping `if: github.event_name == 'pull_request'` and the .NET SDK setup step
- [x] 3.2 Replace the `SonarSource/sonarcloud-github-action@v2` step with: `dotnet sonarscanner begin` using `-d:sonar.host.url=https://sonarcloud.io`, `-d:sonar.organization=${{ secrets.SONAR_ORG }}`, `/k:${{ secrets.SONAR_PROJECT_KEY_PREFIX }}<service>`, `-d:sonar.cs.cobertura.reportsPaths=TestResults/**/coverage.cobertura.xml`, `-d:sonar.coverage.exclusions=**/*Tests/**`, and `-d:sonar.token` from `SONAR_TOKEN`
- [x] 3.3 Add a build+test step between `begin` and `end` running `make test-sln SLN=services/<service>/<Service>.sln` for the matrix row
- [x] 3.4 Add the `dotnet sonarscanner end` step with `if: always()` and `SONAR_TOKEN` env so the session closes even when build/test fails

## 4. Ignore and document

- [x] 4.1 Add `.sonarqube/` to `.gitignore`
- [x] 4.2 In `README.md`, document the local SonarQube workflow: `make sonar-install`, then `make sonar-check` with `SONAR_HOST_URL` (default `http://localhost:9000`), `SONAR_TOKEN`, and `SONAR_PROJECT_KEY_PREFIX`, plus the requirement of a running self-hosted SonarQube server
- [x] 4.3 In `README.md`, document that the SonarCloud and integration-test CI jobs run only on pull requests, while the quality-gate job runs on push to `main` and on pull requests

## 5. Verification

- [x] 5.1 Run `make install` and `make test` to confirm the refactor preserves behavior and coverage output
- [x] 5.2 Run `make sonar-install` locally (installs `dotnet-sonarscanner`)
- [x] 5.3 Run `make sonar-check` with `SONAR_TOKEN` set against a running self-hosted SonarQube and confirm all four per-service analyses complete with coverage and test-source exclusions
- [x] 5.4 Run `make sonar-check` without `SONAR_TOKEN` and confirm it exits non-zero with the fail-fast message
