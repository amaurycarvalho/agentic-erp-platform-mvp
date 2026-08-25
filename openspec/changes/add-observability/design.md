## Context

Current state: services expose only health checks (`erp-acl-service` has `/health`) and the `rag-service` returns RAG search traceability metadata (`correlation_id`, `request_id`, `retrieved_at_utc`). There is no shared correlation propagation, no structured logging of correlation, no metrics beyond health, and no distributed tracing across the HTTP/gRPC chain.

## Goals / Non-Goals

**Goals:**
- Establish a correlation id that flows end-to-end (NFR-002, US-SHARED-001).
- Add structured logging with correlation in all services.
- Add health/readiness and retry telemetry metrics (mcp, erp-acl, rag baselines).
- Add distributed tracing across HTTP (agent/mcp) and gRPC (mcp/acl) boundaries.
- Instrument agent use-case execution with correlation, metrics, and tracing.

**Non-Goals:**
- No business/behavioral change to the flows.
- No change to the MCP contract or the ACL contract.
- No idempotency, error-standardization, or retry/compensation policies (covered by the `resilience` change).

## Decisions

- **Use OpenTelemetry** as the observability framework (metrics + tracing), consistent with the no-third-party-risk rule and active maintenance.
- **Correlation id via shared middleware/header**: a correlation id header flows inbound HTTP and gRPC metadata; generated at the first boundary when absent. The RAG search `request_id`/`correlation_id` semantics remain compatible.
- **Shared instrumentation library** across services to avoid duplication, placed in the shared project.
- **Per-service baselines**: mcp = retry telemetry; erp-acl = health/readiness metrics + gRPC tracing; rag = pipeline retrieval/generation metrics; agent = use-case execution metrics.

## Risks / Trade-offs

- [Adding instrumentation to every service increases surface] → Mitigation: use a shared library and keep configuration centralized.
- [Correlation propagation across gRPC metadata is easy to miss] → Mitigation: rely on OpenTelemetry trace correlation plus explicit correlation id propagation; validate with integration tests.
