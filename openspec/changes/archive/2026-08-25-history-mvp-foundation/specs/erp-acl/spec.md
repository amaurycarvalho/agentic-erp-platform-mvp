## ADDED Requirements

### Requirement: ACL exposes ERP capabilities via gRPC

The erp-acl-service SHALL expose ERP business capabilities to the mcp-service through versioned gRPC contracts, grouped by business context (`OrderService`, `InvoiceService`), with a stable `csharp_namespace`.

#### Scenario: Contracts are versioned and grouped by context
- **WHEN** the mcp-service consumes the ACL contract
- **THEN** it uses gRPC services `OrderService` and `InvoiceService`
- **AND** the generated clients come from the `.proto` contract

### Requirement: Create order via OrderService.CreateOrder

The ACL SHALL create an order in the ERP when the customer is valid and the total amount is greater than zero, returning the created order identifier.

#### Scenario: Create a valid order
- **WHEN** the agent requests an order for a valid customer with total amount greater than zero
- **THEN** the order is created in the ERP
- **AND** an order identifier is returned

#### Scenario: Reject a zero-value order
- **WHEN** the order total amount is zero
- **THEN** the ACL rejects the operation
- **AND** reports that the order value is invalid

### Requirement: Cancel invoice via InvoiceService.CancelInvoice

The ACL SHALL cancel an invoice in the ERP when the invoice exists, is not already canceled, and a cancellation reason is provided, marking the invoice as canceled.

#### Scenario: Cancel an existing invoice
- **WHEN** a valid invoice that is not canceled is canceled with a reason
- **THEN** the invoice is marked as canceled in the ERP

#### Scenario: Reject an already-canceled invoice
- **WHEN** a request attempts to cancel an invoice that is already canceled
- **THEN** the ACL rejects the operation
- **AND** reports that the invoice was already canceled

#### Scenario: Reject a cancellation without reason
- **WHEN** the cancellation is requested without informing a reason
- **THEN** the ACL rejects the operation
- **AND** reports that the cancellation reason is required
