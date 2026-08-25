## Why

The shared cross-cutting backlog (`specs/user-stories/shared/BACKLOG.md`) specifies common policies — idempotency for safe re-execution (`US-SHARED-002`), standardized internal errors (`US-SHARED-003`), and retry/compensation for partial failures (`US-SHARED-004`) — that are not yet implemented. These address `EC-004` (partial plan failure) and `EC-005` (idempotent re-execution) and `NFR-003` (controlled retry and partial-failure handling).

## What Changes

- Introduce a common idempotency policy for safe re-execution across services (US-SHARED-002, EC-005).
- Standardize error taxonomy/response shape between internal services (US-SHARED-003).
- Define a controlled retry and compensation strategy for partial failures (US-SHARED-004, EC-004, NFR-003).
- Align with existing MCP error taxonomy (`validation_error`, `acl_business_error`, `acl_unavailable`) and the ACL domain errors.

## Capabilities

### New Capabilities
- `resilience`: shared idempotency, standardized internal errors, and controlled retry/compensation policies.

### Modified Capabilities
<!-- No existing requirement behavior changes; this introduces a new cross-cutting capability. -->

## Impact

- Affects agent, mcp, and erp-acl services; may add shared contracts in `shared/`.
- Introduces idempotency keys and replay-safe execution.
- Adds a shared error envelope and retry/compensation handling.
