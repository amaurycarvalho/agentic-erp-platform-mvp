## Why

This change is a reverse-engineering artifact: it consolidates the specifications contained in `specs/` that have already been implemented, capturing the delivered MVP foundation as an archivable history record. The `specs/` folder remains the copy of the SDD source; this change carries the implemented behavior into `openspec/specs/` as the OpenSpec main source of truth.

## What Changes

- Capture the architecture foundation decisions and cross-cutting invariants (ADR-001/002/003, constitution principles, REQ-FUNC-001..005).
- Capture the implemented `erp-acl-service` behavior (US-ACL-001/002) and its gRPC contract.
- Capture the implemented `mcp-service` behavior (US-MCP-001/002, MCP-TOOL contract, error taxonomy).
- Capture the implemented `agent-service` behavior (US-AGENT-001/002).
- Capture the implemented `rag-service` behavior (US-RAG-001..004, RAG-SEARCH contract).
- No breaking changes: this is a documentation/history consolidation of behavior already present in the codebase.

## Capabilities

### New Capabilities
- `architecture-foundation`: cross-cutting invariants and the decision that decision is separated from execution and the ERP is isolated.
- `erp-acl`: anti-corruption layer exposing order creation and invoice cancellation via gRPC contracts.
- `mcp`: explicit tool catalog, payload validation, ACL-only execution, standardized errors, and auditable execution.
- `agent`: intent interpretation mapping natural language to MCP tool calls without direct ERP access.
- `rag`: policy retrieval by operation context, versioning, traceability metadata, and RAG-vs-ERP consistency classification.

### Modified Capabilities
<!-- No existing spec-level behavior is changing; this is a new baseline. -->

## Impact

- Adds main specs under `openspec/specs/` for the implemented MVP baseline.
- No application code changes; strictly a spec reverse-engineering and consolidation.
- Affects the spec traceability surface only (constitution, requirements, user stories, ADRs are folded into capability specs).
