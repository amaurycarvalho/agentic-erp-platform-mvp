# Makefile
# Dotnet-native build/test/release tooling for the agentic-erp-platform-mvp.

.PHONY: install test test-integration clean build build-images quality-gate help

# ---------- Variables ----------

# Single source of truth for image and release version.
VERSION ?= 1.0.0

# All four service solutions (each handles its own src/ + tests/).
SOLUTIONS := services/agent-service/Agent.sln \
             services/mcp-service/Mcp.sln \
             services/erp-acl-service/ErpAcl.sln \
             services/rag-service/Rag.sln

# Service container images produced by `build-images`.
IMAGES := agent-service mcp-service erp-acl-service rag-service

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
	@echo "$(GREEN)🧪 Running unit tests (excluding live-stack MCP integration)...$(NC)"
	@for sln in $(SOLUTIONS); do \
		echo "  -> dotnet test $$sln"; \
		dotnet test "$$sln" \
			--filter "Category!=Mcp.Integration" \
			--results-directory TestResults \
			--logger trx || exit 1; \
	done
	@echo "$(GREEN)✅ Tests passed$(NC)"

test-integration:
	@echo "$(GREEN)🔗 Running MCP cross-service integration tests (requires a running stack via MCP_BASE_URL)...$(NC)"
	@dotnet test services/mcp-service/Mcp.sln \
		--filter "Category=Mcp.Integration" \
		--results-directory TestResults \
		--logger trx || exit 1
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

# ---------- Quality gate ----------
# For now the gate is test-only; lint/security/mutation will be added later.

quality-gate:
	@echo "$(GREEN)🚀 Running quality gate...$(NC)"
	@$(MAKE) install
	@$(MAKE) test
	@echo "$(GREEN)🎉 All quality checks passed!$(NC)"

# ---------- Help ----------

help:
	@echo "$(GREEN)📋 Available commands:$(NC)"
	@echo ""
	@echo "  make install          - Restore .NET dependencies (all solutions)"
	@echo "  make test             - Run unit tests (excluding live MCP integration)"
	@echo "  make test-integration - Run MCP cross-service integration tests (requires MCP_BASE_URL)"
	@echo "  make clean            - Clean build artifacts and outputs"
	@echo "  make build            - Compile all solutions in Release"
	@echo "  make build-images VERSION=x.x.x - Build and tag the 4 service images"
	@echo "  make quality-gate     - Run the quality gate (install + test)"
	@echo "  make help             - Show this help message"
	@echo ""
	@echo "$(YELLOW)Examples:$(NC)"
	@echo "  make build-images VERSION=2.0.0"
	@echo "  MCP_BASE_URL=http://localhost:8082 make test-integration"
