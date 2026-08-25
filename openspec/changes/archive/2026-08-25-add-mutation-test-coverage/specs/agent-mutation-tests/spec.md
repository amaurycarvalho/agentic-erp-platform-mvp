## ADDED Requirements

### Requirement: Agent tests kill message validation mutants

The Agent.Application test suite SHALL exercise the message guard so that mutations to the empty/whitespace message validation are killed by Stryker.

#### Scenario: Empty message is rejected

- **WHEN** `ExecuteAsync` is called with a null, empty or whitespace `Message`
- **THEN** an `AgentValidationException` with a message containing `message is required.` is thrown

### Requirement: Agent tests kill financial validation mutants

The Agent.Application test suite SHALL exercise the `TotalAmount` guard at its boundaries so that mutations to the null/positive checks are killed.

#### Scenario: Zero total amount is rejected

- **WHEN** `ExecuteAsync` is called with a valid `CustomerId` and `TotalAmount` equal to zero
- **THEN** an `AgentValidationException` with a message containing `total_amount` is thrown

#### Scenario: Null total amount is rejected

- **WHEN** `ExecuteAsync` is called with a valid `CustomerId`, a null `TotalAmount`, and a message containing no intent keywords
- **THEN** an `AgentValidationException` with a message containing `total_amount` is thrown

### Requirement: Agent tests kill invoice-id validation mutants

The Agent.Application test suite SHALL exercise the `InvoiceId` guard so that mutations to the missing-invoice validation are killed.

#### Scenario: Missing invoice id is rejected

- **WHEN** `ExecuteAsync` is called with a valid `Reason`, a null `InvoiceId`, and a message containing no intent keywords
- **THEN** an `AgentValidationException` with a message containing `invoice_id` is thrown

### Requirement: Agent tests kill intent keyword-detection mutants

The Agent.Application test suite SHALL cover keyword-based intent detection with partial keyword coverage and keyword-free messages so that mutations to `Contains` checks and logical operators in `DetectIntent` are killed.

#### Scenario: Order keyword with partial coverage is unsupported

- **WHEN** `ExecuteAsync` is called with a message containing a create keyword (e.g. `criar`) but no order keyword and no ids
- **THEN** an `UnsupportedIntentException` is thrown

#### Scenario: Create keyword with partial coverage is unsupported

- **WHEN** `ExecuteAsync` is called with a message containing an order keyword (e.g. `pedido`) but no create keyword and no ids
- **THEN** an `UnsupportedIntentException` is thrown

#### Scenario: Cancel keyword with partial coverage is unsupported

- **WHEN** `ExecuteAsync` is called with a message containing `cancel` but no invoice keyword and no ids
- **THEN** an `UnsupportedIntentException` is thrown

#### Scenario: Invoice keyword alone is unsupported

- **WHEN** `ExecuteAsync` is called with a message containing an invoice keyword (e.g. `fatura`) but no cancel keyword and no ids
- **THEN** an `UnsupportedIntentException` is thrown

#### Scenario: Cancel and invoice keywords resolve to cancel

- **WHEN** `ExecuteAsync` is called with a message containing both `cancel` and an invoice keyword and no ids
- **THEN** an `AgentValidationException` with a message containing `invoice_id` is thrown

### Requirement: Agent tests kill unsupported-intent mutants

The Agent.Application test suite SHALL assert on the unsupported-intent outcome so that mutations to the default intent handling are killed.

#### Scenario: Unknown intent is rejected

- **WHEN** `ExecuteAsync` is called with a message containing no recognized keywords and no ids
- **THEN** an `UnsupportedIntentException` with a descriptive message is thrown

### Requirement: Agent tests observe tool-call logging

The Agent.Application test suite SHALL capture `ILogger` output so that mutations to log statements (template or tool-name argument) are killed.

#### Scenario: Create-order tool call is logged

- **WHEN** `ExecuteAsync` handles a valid create-order request with a recording logger
- **THEN** a log entry is recorded whose template contains `Calling MCP tool` and whose argument equals `erp.create_order`

#### Scenario: Cancel-invoice tool call is logged

- **WHEN** `ExecuteAsync` handles a valid cancel-invoice request with a recording logger
- **THEN** a log entry is recorded whose template contains `Calling MCP tool` and whose argument equals `erp.cancel_invoice`

### Requirement: Agent suite meets the mutation gate

The agent-service mutation run SHALL yield a mutation score at or above the configured break threshold.

#### Scenario: Agent mutation score passes the gate

- **WHEN** Stryker.NET runs against the Agent.Application.Tests suite
- **THEN** the reported mutation score is at or above the configured break threshold (60%)
