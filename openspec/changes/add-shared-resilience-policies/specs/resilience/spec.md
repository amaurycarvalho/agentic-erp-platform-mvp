## ADDED Requirements

### Requirement: Safe idempotent re-execution

When an action is replayed after a failure, services SHALL recognize a duplicate request and return the result of the original execution instead of performing the action again, making re-execution safe (EC-005, US-SHARED-002).

#### Scenario: Replay returns original result
- **WHEN** a previously completed execution is replayed with the same idempotency key
- **THEN** the service returns the original result
- **AND** it does not mutate the ERP again

#### Scenario: New request with a fresh key executes normally
- **WHEN** an execution arrives with a new idempotency key
- **THEN** the service proceeds with the action

### Requirement: Standardized internal error responses

Internal services SHALL return a common, standardized error envelope that preserves the taxonomy (`validation_error`, `acl_business_error`, `acl_unavailable`, and not-found) so consumers can react consistently (US-SHARED-003).

#### Scenario: Business rejection uses the shared envelope
- **WHEN** the ACL rejects an action on business rules
- **THEN** the error is returned in the standardized envelope with `acl_business_error`
- **AND** the taxonomy survives translation from ACL to MCP to agent

### Requirement: Controlled retry and compensation for partial failures

Services SHALL apply a controlled retry policy and compensate partial failures so that a partially executed plan is handled without leaving inconsistent state (US-SHARED-004, EC-004, NFR-003).

#### Scenario: Retry is bounded and additive
- **WHEN** a downstream call fails transiently
- **THEN** the service retries with a bounded policy
- **AND** each attempt is observable (per-attempt telemetry)

#### Scenario: Partial failure is compensated
- **WHEN** one step of a multi-step plan fails after prior steps succeeded
- **THEN** the service applies a defined compensation (e.g., reversing prior steps) to avoid inconsistent state
