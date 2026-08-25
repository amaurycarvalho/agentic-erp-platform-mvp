## Why

`NFR-002` (observability) and `US-SHARED-001` (correlation & audit end-to-end) are specified but not yet implemented (see `specs/tasks.md` unchecked observability items and `specs/user-stories/shared/BACKLOG.md`). The MVP currently has only health checks and RAG search traceability metadata; there is no end-to-end correlation, structured logging, metrics, or distributed tracing across `agent-service` -> `mcp-service` -> `erp-acl-service`.

## What Changes

- Establish end-to-end correlation propagation across services (agent, mcp, erp-acl, rag).
- Introduce structured logging carrying correlation ids in every service.
- Add metrics for health and per-attempt retry telemetry.
- Add distributed tracing (OpenTelemetry) across HTTP and gRPC calls.
- Standardize observability baselines per service as listed in `specs/tasks.md`.

## Capabilities

### New Capabilities
- `observability`: end-to-end correlation, structured logs, metrics, and tracing across all services.

### Modified Capabilities
<!-- No existing requirement behavior changes; this introduces a new cross-cutting capability. -->

## Impact

- Affects all four services (agent, mcp, erp-acl, rag) and `docker-compose.yml`.
- Adds OpenTelemetry dependency and shared logging/metrics/tracing instrumentation.
- Propagates a correlation id header/context through the MCP HTTP and ACL gRPC boundaries.
