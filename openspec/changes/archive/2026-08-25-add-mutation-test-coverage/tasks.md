## 1. Test Infrastructure

- [x] 1.1 Add `RecordingLogger<T>` helper implementing `ILogger<T>` in `Agent.Application.Tests` that records `LogInformation` calls (template + args) into an in-memory list
- [x] 1.2 Update the two happy-path tests to use `RecordingLogger` instead of `NullLogger` for the `ProcessAgentCommandUseCase` under test

## 2. Validation Guard Tests

- [x] 2.1 Add test(s) asserting `AgentValidationException` with `message is required.` for null, empty, and whitespace `Message` (kills mutants 6, 7)
- [x] 2.2 Add test asserting `total_amount` validation for `TotalAmount = 0m` with valid `CustomerId` (kills 23)
- [x] 2.3 Add test asserting `total_amount` validation for null `TotalAmount` with valid `CustomerId` and keyword-free message (kills 19, 25, 26, 55)
- [x] 2.4 Add test asserting `invoice_id` validation for null `InvoiceId` with valid `Reason` and keyword-free message (kills 39, 40, 61)

## 3. Intent Detection Tests

- [x] 3.1 Add test asserting `UnsupportedIntentException` (with descriptive message) for a keyword-free message with no ids (kills 10, 82)
- [x] 3.2 Add test asserting `UnsupportedIntentException` for a message containing a create keyword but no order keyword (kills 68, 69, 73)
- [x] 3.3 Add test asserting `UnsupportedIntentException` for a message containing an order keyword but no create keyword (kills 71, 72, 73)
- [x] 3.4 Add test asserting `UnsupportedIntentException` for a message containing `cancel` but no invoice keyword (kills 79, 80, 81)
- [x] 3.5 Add test asserting `UnsupportedIntentException` for a message containing an invoice keyword but no cancel keyword (kills 77)
- [x] 3.6 Add test asserting `invoice_id` validation for a message containing `cancel` plus an invoice keyword with no ids (kills 78, 84)

## 4. Logging Tests

- [x] 4.1 Add assertion on the recorded log template/argument for the valid create-order path (kills 27, 28, 29)
- [x] 4.2 Add assertion on the recorded log template/argument for the valid cancel-invoice path (kills 48, 49, 50)

## 5. Verification

- [x] 5.1 Run `dotnet test` on `Agent.Application.Tests` and confirm all tests pass
- [x] 5.2 Run `dotnet-stryker` for the agent-service and confirm the mutation score is at or above the 60% break threshold (expect only mutant 85 to remain, documented as equivalent)

## 6. MCP: InMemoryMcpToolCatalog metadata

- [x] 6.1 Add a metadata assertion test for the `erp.create_order` tool (Name, Description, InternalTransport, InternalRoute, InputSchema/OutputSchema field Name/Type/Required/Description/Constraints) using the real catalog (kills 1, 3, 4, 6, 7, 9, 11, 12, 13, 16, 17)
- [x] 6.2 Add a metadata assertion test for the `erp.cancel_invoice` tool (kills 18, 20, 22, 23, 25, 26, 27, 29, 31, 32, 34, 35)
- [x] 6.3 Add a `GetByName` not-found test returning null using the real catalog (kills 42 FirstOrDefault→First)

## 7. MCP: ExecuteMcpToolUseCase logging

- [x] 7.1 Add `RecordingLogger<T>` helper to `Mcp.Application.Tests`
- [x] 7.2 Update the create-order execution test to assert the recorded `Executing MCP tool {ToolName}` log (kills 45)
- [x] 7.3 Update the cancel-invoice execution test to assert the recorded success log template and `{Success}` argument (kills 57, 58)

## 8. MCP: ValidatePayload type and message coverage

- [x] 8.1 Add message assertions to the existing missing-field, empty-string and non-positive tests (kills 68, 99, 108)
- [x] 8.2 Add wrong-type tests (`customer_id` as number, `total_amount` as string) asserting `must be of type` (kills 70, 73, 75, 83, 84)
- [x] 8.3 Add a custom-contract test with a valid boolean input field that passes validation (kills 78)
- [x] 8.4 Add a custom-contract test with an unknown field type that passes validation (kills 80)

## 9. ERP-ACL: exception message assertions

- [x] 9.1 Assert `reason`, `not found` and `cancelled` message fragments in the `CancelInvoiceUseCase` tests (kills 6, 7, 10)
- [x] 9.2 Assert `greater than zero` message fragment in the `CreateOrderUseCase` zero/negative test (kills 17)

## 10. Verification (MCP + ERP-ACL)

- [x] 10.1 Run `dotnet test` on `Mcp.Application.Tests` and `ErpAcl.Application.Tests` and confirm all tests pass
- [x] 10.2 Run `dotnet-stryker` for the mcp-service (expect score at or above 60%) and for the erp-acl-service (expect the CancelInvoice/CreateOrder message survivors killed)

## 11. RAG: BuildTraceableResponse excerpt and ordering

- [x] 11.1 Assert sources are ordered ascending by `PolicyCode` in the existing `Should_Return_Only_Relevant_And_Latest_Policy_Versions` test (kills 1)
- [x] 11.2 Add `BuildTraceableResponseUseCaseTests` with excerpt truncation for content > 220 chars asserting it ends with `...` (kills 3, 8)
- [x] 11.3 Add excerpt boundary test: content of exactly 220 chars is not truncated (kills 6)

## 12. RAG: ResolveVersionedSources version comparison

- [x] 12.1 Add `ResolveVersionedSourcesUseCaseTests` with a highest-version-wins test where the higher version is older by date (kills 17, 23, 28, 33, 34, 40, 41)
- [x] 12.2 Add numeric multi-segment comparison test (`1.10` beats `1.9`) (kills 30, 40, 41)
- [x] 12.3 Add different-token-length tests with the shorter and the longer version listed first (kills 27, 32, 35, 36, 39)
- [x] 12.4 Add null-version handling test (kills 18)
- [x] 12.5 Add tie-break-by-newest-update test (kills 11)
- [x] 12.6 Document equivalent mutants 10, 14, 15, 16, 19, 24, 26, 29 (First/IsNullOrWhiteSpace/Split-options/null-coalescing/loop-boundary variants)

## 13. RAG: SearchPoliciesByContext validation

- [x] 13.1 Add a test asserting `RagValidationException` for `MaxSourceAgeDays` of 0 and -1 with message `max_source_age_days` (kills 60, 62, 63)
- [x] 13.2 Assert `operation_context` message fragment in the empty-context validation test (kills 57)
- [x] 13.3 Add a test with an explicit `MaxSourceAgeDays` window (5) that excludes a 10-day-old source, asserting `stale` (kills 49)

## 14. RAG: ValidateConsistencyAgainstErpState

- [x] 14.1 Add `ValidateConsistencyAgainstErpStateUseCaseTests` asserting the Unknown detail message (kills 68)
- [x] 14.2 Assert the Fresh detail message (kills 91)
- [x] 14.3 Assert the Stale-old detail message (kills 87)
- [x] 14.4 Add a version-mismatch test asserting the `does not match` detail (kills 74, 75, 76, 77, 90)
- [x] 14.5 Add a mixed match/mismatch sources test (kills 78)
- [x] 14.6 Add a differing-age sources test (kills 69)
- [x] 14.7 Add an empty-source-version test (kills 82)
- [x] 14.8 Document equivalent boundary mutant 72 (`<` vs `<=` freshness window)

## 15. RAG verification

- [x] 15.1 Run `dotnet test` on `Rag.Application.Tests` and confirm all tests pass
- [x] 15.2 Run `dotnet-stryker` for the rag-service and confirm the mutation score is at or above the 60% break threshold
