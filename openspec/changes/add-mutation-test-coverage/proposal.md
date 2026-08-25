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

### Modified Capabilities

- `mutation-testing`: add a requirement that the configured break/high/low thresholds are actually achievable and met by each service test suite, with the agent-service as the first enforced suite (scenarios for the survivor groups that must be killed).

## Impact

- `services/agent-service/tests/Agent.Application.Tests/ProcessAgentCommand/ProcessAgentCommandUseCaseTests.cs` — new test cases (message validation, financial validation boundaries, missing invoice id, intent keyword coverage, unsupported intent, log assertions).
- `services/agent-service/tests/Agent.Application.Tests/` — new test helper for recording `ILogger<T>` calls.
- `openspec/specs/mutation-testing/spec.md` — delta spec with the threshold-achievement requirement.
- No production code changes; the mutation gate becomes achievable for the agent-service (other services are tracked as follow-up).
