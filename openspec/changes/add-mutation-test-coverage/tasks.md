## 1. Test Infrastructure

- [ ] 1.1 Add `RecordingLogger<T>` helper implementing `ILogger<T>` in `Agent.Application.Tests` that records `LogInformation` calls (template + args) into an in-memory list
- [ ] 1.2 Update the two happy-path tests to use `RecordingLogger` instead of `NullLogger` for the `ProcessAgentCommandUseCase` under test

## 2. Validation Guard Tests

- [ ] 2.1 Add test(s) asserting `AgentValidationException` with `message is required.` for null, empty, and whitespace `Message` (kills mutants 6, 7)
- [ ] 2.2 Add test asserting `total_amount` validation for `TotalAmount = 0m` with valid `CustomerId` (kills 23)
- [ ] 2.3 Add test asserting `total_amount` validation for null `TotalAmount` with valid `CustomerId` and keyword-free message (kills 19, 25, 26, 55)
- [ ] 2.4 Add test asserting `invoice_id` validation for null `InvoiceId` with valid `Reason` and keyword-free message (kills 39, 40, 61)

## 3. Intent Detection Tests

- [ ] 3.1 Add test asserting `UnsupportedIntentException` (with descriptive message) for a keyword-free message with no ids (kills 10, 82)
- [ ] 3.2 Add test asserting `UnsupportedIntentException` for a message containing a create keyword but no order keyword (kills 68, 69, 73)
- [ ] 3.3 Add test asserting `UnsupportedIntentException` for a message containing an order keyword but no create keyword (kills 71, 72, 73)
- [ ] 3.4 Add test asserting `UnsupportedIntentException` for a message containing `cancel` but no invoice keyword (kills 79, 80, 81)
- [ ] 3.5 Add test asserting `UnsupportedIntentException` for a message containing an invoice keyword but no cancel keyword (kills 77)
- [ ] 3.6 Add test asserting `invoice_id` validation for a message containing `cancel` plus an invoice keyword with no ids (kills 78, 84)

## 4. Logging Tests

- [ ] 4.1 Add assertion on the recorded log template/argument for the valid create-order path (kills 27, 28, 29)
- [ ] 4.2 Add assertion on the recorded log template/argument for the valid cancel-invoice path (kills 48, 49, 50)

## 5. Verification

- [ ] 5.1 Run `dotnet test` on `Agent.Application.Tests` and confirm all tests pass
- [ ] 5.2 Run `dotnet-stryker` for the agent-service and confirm the mutation score is at or above the 60% break threshold (expect only mutant 85 to remain, documented as equivalent)
