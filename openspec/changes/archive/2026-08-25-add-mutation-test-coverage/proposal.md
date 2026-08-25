## Why

The agent-service mutation run (`make mutation`) reports a real score of 58.9% against the 60% break threshold, so the gate fails. The 30 surviving mutants in `ProcessAgentCommandUseCase` are genuine test gaps (unexercised validation branches, intent-detection paths masked by keyword/ID overlap, and logging emitted into a `NullLogger`), not tooling artifacts. Without tests that kill them, the mutation gate can never pass and those behaviors remain unprotected.

## What Changes

- Add unit tests to `Agent.Application.Tests` covering the surviving mutant groups in `ProcessAgentCommandUseCase`:
  - Empty/whitespace message validation
  - `TotalAmount` boundary and null validation (incl. partial-ID intent detection)
  - Missing `InvoiceId` validation
  - Keyword-based intent detection with partial keyword coverage (order and cancel)
  - Unsupported/unknown intent path
- Introduce a recording `ILogger<T>` test helper so log-statement mutations (message template and tool-name argument) become observable and assertable.
- Treat the `return "unsupported"` string mutation as an equivalent mutant (documented in design) — it is not killable without changing production design.
- Update the `mutation-testing` spec to require that the agent-service mutation score meets the configured break threshold.

## Capabilities

### New Capabilities

- `agent-mutation-tests`: tests for `ProcessAgentCommandUseCase` that kill the surviving mutants, bringing the agent-service mutation score above the break threshold, plus the recording-logger test helper that makes log mutations observable.
- `mcp-mutation-tests`: tests that kill the surviving mutants in the MCP service (catalog metadata, tool-execution logging, payload type validation), raising the mcp-service score above the 60% break threshold.
- `erp-acl-mutation-tests`: tests that assert exception message content in the ERP-ACL use cases, killing the surviving message-string mutants.
- `rag-mutation-tests`: tests that kill the surviving mutants in the RAG service (traceable-response ordering/excerpt, version comparison, search validation, consistency evaluation), raising the rag-service score above the 60% break threshold.

### Modified Capabilities

- `mutation-testing`: add a requirement that the configured break/high/low thresholds are actually achievable and met by each service test suite, with the agent, mcp, erp-acl and rag suites enforced (scenarios for the survivor groups that must be killed).

## Impact

- `services/agent-service/tests/Agent.Application.Tests/ProcessAgentCommand/ProcessAgentCommandUseCaseTests.cs` — new test cases (message validation, financial validation boundaries, missing invoice id, intent keyword coverage, unsupported intent, log assertions).
- `services/agent-service/tests/Agent.Application.Tests/` — new test helper for recording `ILogger<T>` calls.
- `services/mcp-service/tests/Mcp.Application.Tests/` — new catalog metadata tests, `RecordingLogger<T>` helper, tool-execution log assertions, and payload type/message validation tests.
- `services/erp-acl-service/tests/ErpAcl.Application.Tests/` — exception message assertions in the CancelInvoice and CreateOrder use-case tests.
- `services/rag-service/tests/Rag.Application.Tests/` — new tests for `BuildTraceableResponseUseCase` (ordering/excerpt), `ResolveVersionedSourcesUseCase` (version comparison), search-request validation, and `ValidateConsistencyAgainstErpStateUseCase` (branches and detail messages).
- `openspec/specs/mutation-testing/spec.md` — delta spec with the threshold-achievement requirement.
- No production code changes; the mutation gate becomes achievable for the agent, mcp, erp-acl and rag services.
