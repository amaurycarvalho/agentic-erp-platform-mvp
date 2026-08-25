## ADDED Requirements

### Requirement: Correlation id propagates end-to-end

Each service SHALL propagate an end-to-end correlation id across the full chain (`agent-service` -> `mcp-service` -> `erp-acl-service`, and `rag-service`), reusing an incoming correlation id and generating one when absent, so a single request can be traced through all boundaries.

#### Scenario: Correlation id is propagated across services
- **WHEN** a request flows through agent, mcp, and acl services
- **THEN** the same correlation id is carried across all service boundaries

#### Scenario: Correlation id is generated when absent
- **WHEN** a request arrives without a correlation id
- **THEN** the receiving service generates one and propagates it downstream

### Requirement: Structured logs carry correlation

Every service SHALL emit structured logs that include the correlation id, enabling filtering and debugging of a single flow.

#### Scenario: Log lines include correlation
- **WHEN** a service logs during request processing
- **THEN** the log entry includes the correlation id

### Requirement: Metrics expose health and retry telemetry

The mcp-service SHALL consolidate its retry strategy with per-attempt telemetry (logs and metrics); erp-acl-service SHALL expose health metrics; rag-service SHALL define a baseline of metrics for its retrieval and generation pipeline.

#### Scenario: Retry telemetry is emitted per attempt
- **WHEN** the mcp-service retries an ACL call
- **THEN** each attempt is recorded as a log entry and a metric

#### Scenario: Health metrics are available
- **WHEN** health checks are queried
- **THEN** health and readiness metrics are reported

### Requirement: Distributed tracing across HTTP and gRPC

Services SHALL emit distributed traces that span HTTP and gRPC calls, so the flow between services is observable.

#### Scenario: A trace spans the full service chain
- **WHEN** an execution crosses HTTP (agent/mcp) and gRPC (mcp/acl) boundaries
- **THEN** the spans are linked into a single trace

### Requirement: Agent use cases are instrumented

The agent-service SHALL instrument its use-case execution with correlation id, metrics, and tracing.

#### Scenario: Agent execution emits observability data
- **WHEN** the agent-service processes a command
- **THEN** it emits traces and metrics tied to the correlation id
