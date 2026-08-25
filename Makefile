# Makefile
# Dotnet-native build/test/release + quality tooling for the agentic-erp-platform-mvp.

.PHONY: install test test-integration clean build build-images \
        lint metrics coverage coverage-check security mutation \
        install-quality-tools quality-gate help

# ---------- Variables ----------

# Single source of truth for image and release version.
VERSION ?= 1.0.0

# Coverage floor (start lower and tighten). Measured baseline: 83-94% per solution.
COVERAGE_THRESHOLD ?= 80

# All four service solutions (each handles its own src/ + tests/).
SOLUTIONS := services/agent-service/Agent.sln \
             services/mcp-service/Mcp.sln \
             services/erp-acl-service/ErpAcl.sln \
             services/rag-service/Rag.sln

# Service container images produced by `build-images`.
IMAGES := agent-service mcp-service erp-acl-service rag-service

# Service source directories (for metrics).
SERVICE_DIRS := agent-service mcp-service erp-acl-service rag-service

# Solution names used as per-service test-result subdirectories.
TEST_RESULT_DIRS := Agent Mcp ErpAcl Rag

# Output colors.
GREEN := \033[0;32m
RED := \033[0;31m
YELLOW := \033[0;33m
NC := \033[0m

# ---------- Main commands ----------

install:
	@echo "$(GREEN)📦 Restoring .NET dependencies...$(NC)"
	@for sln in $(SOLUTIONS); do \
		echo "  -> dotnet restore $$sln"; \
		dotnet restore "$$sln" || exit 1; \
	done
	@echo "$(GREEN)✅ Restore complete$(NC)"

test:
	@echo "$(GREEN)🧪 Running unit tests + coverage (excluding live-stack MCP integration)...$(NC)"
	@for sln in $(SOLUTIONS); do \
		name="$$(basename "$$sln" .sln)"; \
		echo "  -> dotnet test $$sln"; \
		dotnet test "$$sln" \
			--filter "Category!=Mcp.Integration" \
			--results-directory "TestResults/$$name" \
			--collect:"XPlat Code Coverage" \
			--logger "trx;LogFileName=results.trx" || exit 1; \
	done
	@echo "$(GREEN)✅ Tests passed$(NC)"

test-integration:
	@echo "$(GREEN)🔗 Running MCP cross-service integration tests (requires a running stack via MCP_BASE_URL)...$(NC)"
	@dotnet test services/mcp-service/Mcp.sln \
		--filter "Category=Mcp.Integration" \
		--results-directory "TestResults/Mcp" \
		--collect:"XPlat Code Coverage" \
		--logger "trx;LogFileName=results.trx" || exit 1
	@echo "$(GREEN)✅ Integration tests passed$(NC)"

clean:
	@echo "$(YELLOW)🧹 Cleaning build artifacts...$(NC)"
	@for sln in $(SOLUTIONS); do \
		dotnet clean "$$sln" || exit 1; \
	done
	@find . -type d \( -name bin -o -name obj \) -not -path './.git/*' -prune -exec rm -rf {} + 2>/dev/null || true
	@rm -rf TestResults images
	@echo "$(GREEN)✅ Clean complete$(NC)"

build:
	@echo "$(GREEN)📦 Building all solutions (Release)...$(NC)"
	@for sln in $(SOLUTIONS); do \
		echo "  -> dotnet build $$sln"; \
		dotnet build "$$sln" -c Release || exit 1; \
	done
	@echo "$(GREEN)✅ Build complete$(NC)"

build-images:
	@echo "$(GREEN)🐳 Building service images (VERSION=$(VERSION))...$(NC)"
	@$(MAKE) image-agent-service
	@$(MAKE) image-erp-acl-service
	@$(MAKE) image-rag-service
	@$(MAKE) image-mcp-service
	@echo "$(GREEN)✅ Images built and tagged$(NC)"

# Per-service image builds.
# mcp-service uses the repo-root context because Mcp.Infrastructure references the
# shared ErpAcl.Contracts project (services/erp-acl-service/src/ErpAcl.Contracts).
image-agent-service:
	docker build -t agent-service:$(VERSION) -t agent-service:latest \
		-f services/agent-service/Dockerfile services/agent-service

image-erp-acl-service:
	docker build -t erp-acl-service:$(VERSION) -t erp-acl-service:latest \
		-f services/erp-acl-service/Dockerfile services/erp-acl-service

image-rag-service:
	docker build -t rag-service:$(VERSION) -t rag-service:latest \
		-f services/rag-service/Dockerfile services/rag-service

image-mcp-service:
	docker build -t mcp-service:$(VERSION) -t mcp-service:latest \
		-f services/mcp-service/Dockerfile .

# ---------- Quality targets ----------

lint:
	@echo "$(GREEN)🔍 Lint (dotnet format --verify-no-changes)...$(NC)"
	@for sln in $(SOLUTIONS); do \
		echo "  -> dotnet format $$sln"; \
		dotnet format "$$sln" --verify-no-changes || exit 1; \
	done
	@echo "$(GREEN)✅ Lint passed$(NC)"

metrics:
	@echo "$(GREEN)📊 Code metrics (Lines of Code)...$(NC)"
	@for svc in $(SERVICE_DIRS); do \
		count="$$(find services/$$svc/src -name '*.cs' -not -path '*/obj/*' -not -path '*/bin/*' -exec cat {} + 2>/dev/null | wc -l)"; \
		echo "  $$svc: $$count LOC"; \
	done
	@echo "  (complexity / code smells / sqale / maintainability: provided by SonarCloud)"
	@echo "$(GREEN)✅ Metrics complete$(NC)"

coverage: test coverage-check

coverage-check:
	@echo "$(GREEN)📊 Checking coverage against threshold (>= $(COVERAGE_THRESHOLD)%)...$(NC)"
	@python3 scripts/coverage_check.py "$(COVERAGE_THRESHOLD)"

security:
	@echo "$(GREEN)🔒 Security scan (dependencies + SAST)...$(NC)"
	@for sln in $(SOLUTIONS); do \
		dir="$$(dirname "$$sln")"; \
		echo "  -> $$sln --vulnerable"; \
		out="$$(cd "$$dir" && dotnet list package --vulnerable 2>&1)"; \
		echo "$$out"; \
		if echo "$$out" | grep -qi "vulnerab" && ! echo "$$out" | grep -qi "no vulnerable packages"; then \
			echo "$(RED)❌ Vulnerable packages found$(NC)"; exit 1; \
		fi; \
		echo "  -> $$sln --deprecated"; \
		(cd "$$dir" && dotnet list package --deprecated); \
		echo "  -> $$sln --outdated"; \
		(cd "$$dir" && dotnet list package --outdated); \
	done
	@echo "  -> semgrep (C# source)..."
	@semgrep ci --oss-only --quiet --config auto --include '*.cs' || exit 1
	@echo "$(GREEN)✅ Security scan complete$(NC)"

mutation:
	@echo "$(GREEN)🧬 Running mutation tests (Stryker.NET, manual)...$(NC)"
	@for tp in services/*/tests/*.Application.Tests; do \
		echo "  -> dotnet-stryker in $$tp"; \
		(cd "$$tp" && dotnet-stryker --test-runner mtp) || exit 1; \
	done
	@echo "$(GREEN)✅ Mutation tests complete$(NC)"

install-quality-tools:
	@echo "$(GREEN)🔧 Installing quality tools...$(NC)"
	@dotnet tool install --global dotnet-stryker || true
	@if ! command -v semgrep >/dev/null 2>&1; then python3 -m pip install --user semgrep; fi
	@echo "$(GREEN)✅ Quality tools installed (dotnet-stryker + semgrep; dotnet-format bundled)$(NC)"

# ---------- Quality gate ----------
# Lint + test(coverage) + coverage-check + metrics + security.
# Mutation (Stryker.NET) is excluded and run manually via `make mutation`.
# SonarCloud analysis + PR decoration run as a CI step.

quality-gate:
	@echo "$(GREEN)🚀 Running quality gate...$(NC)"
	@$(MAKE) install
	@$(MAKE) lint
	@$(MAKE) test
	@$(MAKE) coverage-check
	@$(MAKE) metrics
	@$(MAKE) security
	@echo "$(GREEN)🎉 All quality checks passed!$(NC)"

# ---------- Help ----------

help:
	@echo "$(GREEN)📋 Available commands:$(NC)"
	@echo ""
	@echo "  make install          - Restore .NET dependencies (all solutions)"
	@echo "  make test             - Run unit tests + collect coverage"
	@echo "  make test-integration - Run MCP cross-service integration tests (requires MCP_BASE_URL)"
	@echo "  make clean            - Clean build artifacts and outputs"
	@echo "  make build            - Compile all solutions in Release"
	@echo "  make build-images VERSION=x.x.x - Build and tag the 4 service images"
	@echo "  make lint             - Verify formatting/analyzers (dotnet format --verify-no-changes)"
	@echo "  make metrics          - Report Lines of Code per service"
	@echo "  make coverage         - Run tests and check coverage threshold"
	@echo "  make coverage-check   - Check coverage against COVERAGE_THRESHOLD (default 80)"
	@echo "  make security         - Check package vulnerabilities/deprecated/outdated + Semgrep SAST"
	@echo "  make mutation         - Run Stryker.NET mutation tests (manual, not in CI)"
	@echo "  make install-quality-tools - Install dotnet-stryker + semgrep"
	@echo "  make quality-gate     - Run the quality gate (lint + test + coverage + metrics + security)"
	@echo "  make help             - Show this help message"
	@echo ""
	@echo "$(YELLOW)Examples:$(NC)"
	@echo "  make build-images VERSION=2.0.0"
	@echo "  MCP_BASE_URL=http://localhost:8082 make test-integration"
	@echo "  COVERAGE_THRESHOLD=90 make coverage-check"
