## Context

This is a history/consolidation change, not an implementation change. It reverse-engineers the specifications in `specs/` that are already implemented in the codebase, and folds them into OpenSpec capability specs under `openspec/specs/`.

Current state (from `specs/plan.md` and `specs/tasks.md`): `erp-acl-service` and `mcp-service` are implemented with gRPC contracts and explicit tool catalogs; `agent-service` and `rag-service` have their use cases implemented; compose with health checks is in place; tests are structured with traceability back to specs. Cross-cutting observability and the shared resilience backlog remain for future work.

## Goals / Non-Goals

**Goals:**
- Consolidate all implemented MVP behavior into five capability specs (`architecture-foundation`, `erp-acl`, `mcp`, `agent`, `rag`).
- Preserve the reverse-engineered contracts and invariants exactly as implemented.
- Establish `openspec/specs/` as the OpenSpec main source of truth for this baseline.

**Non-Goals:**
- No behavior changes or new functionality.
- No observability (correlation, metrics, tracing) — that is a separate future change.
- No shared resilience policies (idempotency, error standardization, retry/compensation) — separate future change.
- No MCP catalog expansion or per-tool authorization — separate future change.

## Decisions

- **Capability boundaries follow service domains.** Each service maps to one capability so the specs align with the deployed services and existing tests.
- **Invariants live in `architecture-foundation`.** Constitution principles and REQ-FUNC cross-cutting rules stay in a dedicated capability rather than being duplicated per service.
- **Contracts are captured as requirements.** gRPC contracts (ACL) and MCP/RAG tool/search contracts are expressed as normative requirements with scenarios, since they define testable behavior.
- **Archiving promotes these to main specs.** Because the change is historical, archiving will sync the five delta specs into `openspec/specs/`, completing the reverse-engineering.

## Risks / Trade-offs

- [Consolidation may under-specify] → Mitigation: each requirement carries one or more scenarios mirroring the original user-story test scenarios.
- [Duplication with untouched `specs/` folder] → Mitigation: `specs/` remains the SDD narrative; `openspec/specs/` becomes the OpenSpec-structured source; the README matrix already maps requirements to use cases.
