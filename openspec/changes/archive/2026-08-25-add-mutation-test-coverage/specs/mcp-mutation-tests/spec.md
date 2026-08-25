## ADDED Requirements

### Requirement: MCP tests assert full catalog metadata

The Mcp.Application test suite SHALL assert the exact metadata of every tool exposed by `InMemoryMcpToolCatalog` so that mutations to catalog string/boolean literals are killed.

#### Scenario: Create-order tool metadata is asserted

- **WHEN** the tool catalog is queried for `erp.create_order`
- **THEN** its `Name`, `Description`, `InternalTransport`, `InternalRoute` and all input/output schema field attributes (`Name`, `Type`, `Required`, `Description`, `Constraints`) match the expected catalog values

#### Scenario: Cancel-invoice tool metadata is asserted

- **WHEN** the tool catalog is queried for `erp.cancel_invoice`
- **THEN** its `Name`, `Description`, `InternalTransport`, `InternalRoute` and all input/output schema field attributes match the expected catalog values

#### Scenario: Unknown tool name returns null

- **WHEN** `GetByName` is called on the real catalog with a non-existent tool name
- **THEN** `null` is returned

### Requirement: MCP tests observe tool-execution logging

The Mcp.Application test suite SHALL capture `ILogger` output for tool execution so that mutations to log statements are killed.

#### Scenario: Create-order execution is logged

- **WHEN** `ExecuteMcpToolUseCase` handles a valid create-order execution with a recording logger
- **THEN** a log entry with template `Executing MCP tool {ToolName}` is recorded

#### Scenario: Cancel-invoice execution success is logged

- **WHEN** `ExecuteMcpToolUseCase` handles a valid cancel-invoice execution with a recording logger
- **THEN** a log entry whose template contains `executed successfully with success` and whose argument equals `true` is recorded

### Requirement: MCP tests cover payload type validation

The Mcp.Application test suite SHALL assert validation error messages and exercise every field type branch in `ValidatePayloadUseCase` so that mutations to type validation and messages are killed.

#### Scenario: Missing required field message is asserted

- **WHEN** a required field is missing from the payload
- **THEN** a `ToolValidationException` with a message containing `is required.` is thrown

#### Scenario: Wrong type is rejected with a message

- **WHEN** a string field receives a number or a number field receives a string
- **THEN** a `ToolValidationException` with a message containing `must be of type` is thrown

#### Scenario: Empty constraint message is asserted

- **WHEN** a string field with `must_not_be_empty` is empty
- **THEN** a `ToolValidationException` with a message containing `must not be empty` is thrown

#### Scenario: Non-positive number message is asserted

- **WHEN** a number field with `greater_than_zero` is zero or negative
- **THEN** a `ToolValidationException` with a message containing `greater than zero` is thrown

#### Scenario: Valid boolean field passes

- **WHEN** a required boolean input field receives a boolean value
- **THEN** validation passes without throwing

#### Scenario: Unknown field type passes

- **WHEN** a field has a type outside `string`/`number`/`boolean`
- **THEN** validation passes without throwing
