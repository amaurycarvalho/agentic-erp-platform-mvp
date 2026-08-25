# agent Specification

## Purpose
TBD - created by archiving change history-mvp-foundation. Update Purpose after archive.
## Requirements
### Requirement: Agent maps create-order intent to the MCP tool

The agent-service SHALL interpret a natural-language intent to create an order and invoke the `erp.create_order` tool in the mcp-service.

#### Scenario: Valid create-order request
- **WHEN** the user requests order creation and the required data is present
- **THEN** the agent calls `erp.create_order` on the mcp-service
- **AND** returns the `order_id` to the consumer

#### Scenario: Insufficient data is not forced into invalid execution
- **WHEN** the user requests order creation without required data
- **THEN** the agent requests completion or returns a validation error
- **AND** it does not force an invalid execution on the mcp-service

### Requirement: Agent maps cancel-invoice intent to the MCP tool

The agent-service SHALL interpret a natural-language intent to cancel an invoice and invoke the `erp.cancel_invoice` tool in the mcp-service.

#### Scenario: Valid cancel-invoice request
- **WHEN** the user requests invoice cancellation with a reason
- **THEN** the agent calls `erp.cancel_invoice` on the mcp-service
- **AND** returns the success result to the consumer

#### Scenario: Cancellation without reason is not forced
- **WHEN** the user requests cancellation without a reason
- **THEN** the agent requests completion or returns a validation error
- **AND** it does not force an invalid execution on the mcp-service

### Requirement: Agent obeys the MCP contract and never accesses ERP directly

The agent-service SHALL build payloads that obey the MCP tool contract and SHALL route all execution through the mcp-service, never accessing the ERP directly.

#### Scenario: Agent respects contract and mediated path
- **WHEN** the agent builds an MCP payload
- **THEN** it conforms to the MCP tool contract
- **AND** the execution path is agent-service -> mcp-service -> erp-acl-service

