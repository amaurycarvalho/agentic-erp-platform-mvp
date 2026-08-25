## Context

Already in place: the `mcp-service` has a bounded retry with resilience integration tests and an error taxonomy (`tool_not_found`, `validation_error`, `acl_business_error`, `acl_unavailable`). The ACL has domain rejections (invalid order value, already-canceled invoice, missing reason). Missing: a shared idempotency contract (EC-005), a standardized internal error envelope across services (US-SHARED-003), and an explicit compensation strategy for partial plan failures (EC-004).

## Goals / Non-Goals

**Goals:**
- Common idempotency policy allowing safe re-execution.
- Shared internal error envelope preserving the error taxonomy.
- Controlled retry bounded per service and compensation for partial failures.

**Non-Goals:**
- No change to the MCP tool contract or the ACL gRPC contract shape.
- No observability/metrics (covered by the `observability` change), though retry telemetry hooks align with it.

## Decisions

- **Idempotency key** carried on execution requests (reuse correlation id or a dedicated key) and persisted to replay the original result.
- **Shared error envelope** in `shared/` contracts; the MCP taxonomy remains the outward-facing taxonomy, mapped to the envelope internally.
- **Compensation as explicit steps** in the plan model: each actionable step declares a compensating action; on partial failure, the runbook reverses completed steps.

## Risks / Trade-offs

- [Idempotency persistence adds storage] → Mitigation: lightweight in-memory key->result for the MVP, with a note to move to durable storage.
- [Compensation can be under-specified] → Mitigation: only define compensation for steps that mutate the ERP, and align with cancellation/invoice order semantics.
