# Tasks

## Tasks

- [ ] Add shared observability library (correlation middleware, structured logging, tracing)
- [ ] Instrument `erp-acl-service`: structured logs, health metrics, gRPC call tracing
- [ ] Instrument `mcp-service`: consolidate retry strategy with per-attempt telemetry (logs and metrics)
- [ ] Instrument `agent-service`: use-case execution with correlation id, metrics, tracing
- [ ] Instrument `rag-service`: baseline metrics, tracing, and logs for the retrieval/generation pipeline
- [ ] Propagate correlation id end-to-end (agent -> mcp -> acl) and through rag
- [ ] Add integration tests validating correlation propagation and tracing across the chain
