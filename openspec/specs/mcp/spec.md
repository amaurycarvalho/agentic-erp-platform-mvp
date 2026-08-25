# mcp Specification

## Purpose
TBD - created by archiving change history-mvp-foundation. Update Purpose after archive.
## Requirements
### Requirement: Explicit tool catalog gates execution

The mcp-service SHALL maintain an explicit, versioned catalog of available tools and SHALL accept execution only for tools present in that catalog.

#### Scenario: List available tools
- **WHEN** a consumer queries the tool catalog
- **THEN** the service returns the catalogued tools, including `erp.create_order` and `erp.cancel_invoice`

### Requirement: Payload is validated before execution

The mcp-service SHALL validate a tool payload against its contract before invoking the ERP ACL; a payload that does not conform SHALL be rejected without contacting the ACL.

#### Scenario: Reject invalid payload without calling ACL
- **WHEN** a tool payload does not conform to the contract
- **THEN** the mcp-service returns `validation_error`
- **AND** it does not call the erp-acl-service

#### Scenario: Accept valid payload
- **WHEN** a tool payload conforms to the contract
- **THEN** the mcp-service proceeds to execute the tool via the ACL

### Requirement: Execution goes through the ACL gateway only

The mcp-service SHALL execute tools exclusively through the erp-acl-service gateway (gRPC), never bypassing the ACL.

#### Scenario: Create order is routed to OrderService.CreateOrder
- **WHEN** `erp.create_order` is executed with a valid payload
- **THEN** the service calls `OrderService.CreateOrder` on the erp-acl-service
- **AND** returns `order_id` per contract

#### Scenario: Cancel invoice is routed to InvoiceService.CancelInvoice
- **WHEN** `erp.cancel_invoice` is executed with a valid payload
- **THEN** the service calls `InvoiceService.CancelInvoice` on the erp-acl-service
- **AND** returns `success` per contract

### Requirement: Standardized error taxonomy

The mcp-service SHALL return errors using a standardized taxonomy: `tool_not_found`, `validation_error`, `acl_business_error`, `acl_unavailable`.

#### Scenario: Unknown tool returns tool_not_found
- **WHEN** an execution is requested for a tool not in the catalog
- **THEN** the service returns the `tool_not_found` error

#### Scenario: ACL failure maps to classified errors
- **WHEN** the ACL returns a business rejection
- **THEN** the service returns `acl_business_error`
- **AND** when the ACL is unavailable it returns `acl_unavailable`

### Requirement: Executions are auditable

The mcp-service SHALL record an auditable event with correlation for every tool execution.

#### Scenario: Execution leaves an audit event
- **WHEN** a tool execution completes
- **THEN** an audit event tied to the correlation id is recorded

### Requirement: Discovery and execution endpoints

The mcp-service SHALL expose minimal endpoints for discovery and execution: `/mcp/tools`, `/mcp/tools/{toolName}`, `/mcp/tools/{toolName}/execute`, and `/health`.

#### Scenario: Tools are discoverable and executable via HTTP
- **WHEN** a consumer calls the tools or execution endpoints
- **THEN** the service responds with catalog information or execution results accordingly

