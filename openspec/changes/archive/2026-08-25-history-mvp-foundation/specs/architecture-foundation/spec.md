## ADDED Requirements

### Requirement: Decision is separated from execution

The platform SHALL keep agentic decision-making (interpretation and planning) separate from the services that execute business actions, so that no single component both decides and executes against the legacy ERP.

#### Scenario: Agent plans but never executes directly
- **WHEN** the agent-service interprets an intent and builds a plan
- **THEN** the execution is delegated to the mcp-service
- **AND** the agent-service does not touch the ERP directly

### Requirement: Knowledge does not execute actions

The RAG service SHALL only provide retrieved knowledge/context and SHALL never perform business actions in the ERP.

#### Scenario: RAG only supplies context
- **WHEN** the rag-service is queried for relevant policies
- **THEN** it returns source documents only
- **AND** it does not expose or invoke any ERP mutation operation

### Requirement: AI never accesses the ERP directly

All access to the legacy ERP SHALL be routed exclusively through the erp-acl-service; no AI service shall reach the ERP core directly.

#### Scenario: ERP access is always mediated
- **WHEN** any action needs to reach the ERP
- **THEN** the call goes through the erp-acl-service anti-corruption layer

### Requirement: Exposed AI capabilities are explicit

The set of executable capabilities exposed to the AI SHALL be defined explicitly through the MCP tool catalog; nothing outside the catalog is executable.

#### Scenario: Only catalogued tools are executable
- **WHEN** an agent requests an execution
- **THEN** the mcp-service accepts only tools present in the explicit catalog

### Requirement: Executions are auditable

The platform SHALL generate an auditable record for every action execution, preserving traceability.

#### Scenario: Every execution leaves a trace
- **WHEN** a tool is executed
- **THEN** an audit event with correlation information is recorded

### Requirement: Architecture follows ADR-001 foundations

The platform SHALL follow the architecture foundation of ADR-001: DDD, Clean Architecture, microservices, C#/.NET, Strangler Pattern, Spec-Driven Development, and internal communication prioritizing gRPC.

#### Scenario: New services follow the foundation
- **WHEN** a service is added or evolved
- **THEN** it complies with the ADR-001 foundation and keeps container-per-service with Docker Compose
