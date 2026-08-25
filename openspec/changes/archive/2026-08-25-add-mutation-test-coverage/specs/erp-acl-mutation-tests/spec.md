## ADDED Requirements

### Requirement: ERP-ACL tests assert exception message content

The ErpAcl.Application test suite SHALL assert exception message content so that mutations to validation/error message strings are killed.

#### Scenario: Empty cancellation reason message is asserted

- **WHEN** `CancelInvoiceUseCase` is called with an empty or whitespace reason
- **THEN** an `ArgumentException` with a message containing `reason` is thrown

#### Scenario: Missing invoice message is asserted

- **WHEN** `CancelInvoiceUseCase` is called and the invoice is not found
- **THEN** an `InvalidOperationException` with a message containing `not found` is thrown

#### Scenario: Already-cancelled invoice message is asserted

- **WHEN** `CancelInvoiceUseCase` is called and the invoice is already cancelled
- **THEN** an `InvalidOperationException` with a message containing `cancelled` is thrown

#### Scenario: Non-positive order total message is asserted

- **WHEN** `CreateOrderUseCase` is called with a total amount of zero or less
- **THEN** an `ArgumentException` with a message containing `greater than zero` is thrown
