## Context

The agent-service mutation run is real now (K 43 / S 30, score 58.9% vs break 60%). The 30 survivors in `ProcessAgentCommandUseCase` split into four root causes: unexercised validation branches, intent detection masked by ID/keyword overlap, logging discarded by `NullLogger`, and one equivalent mutant. The existing test file has 4 tests covering only the happy path plus two validation branches.

## Goals / Non-Goals

**Goals:**
- Kill 29 of the 30 surviving mutants via new xUnit tests, raising the agent-service score above the 60% break threshold.
- Make log mutations observable through a recording logger helper.
- Document the single non-killable (equivalent) mutant so the gate result is understood.

**Non-Goals:**
- No production code changes (e.g., no refactor of `DetectIntent` to enums, no `ignore-mutations` tuning).
- No mutation runs for rag/erp-acl/mcp services in this change (they have no valid reports yet; same methodology applies later).
- No CI changes.

## Decisions

### D1. Kill mutants by expanding behavioral tests, not by tuning Stryker

Each survivor group maps to a test with an input matrix that makes mutant behavior differ from original behavior. The core principle: **change one input dimension at a time** so keyword fallback cannot mask ID-based detection and vice versa.

- Validation guards: use **neutral messages** (no keywords, e.g. `"ola"`) so the intent path never falls back to keyword detection.
- Keyword detection: use messages with **partial keyword coverage** (e.g. `"criar estoque"` has `criar` but not `pedido`/`order`) because the mutated `Contains(x)` → `Contains("")` is always true and `||`→`&&` differs only on partial coverage.
- Boundary values: `TotalAmount = 0m` distinguishes `<= 0` from `< 0`; `TotalAmount = null` with a valid `CustomerId` distinguishes the `||`→`&&` mutation on the TotalAmount guard and the partial-ID intent branch.

Test-to-mutant matrix (12 cases → 29 mutants):

| Case | Inputs | Expectation | Mutants killed |
|------|--------|-------------|----------------|
| Empty message | `Message` = null / `""` / `"   "` | `AgentValidationException` c/ `"message is required."` | 6, 7 |
| TotalAmount = 0 | `CustomerId` set, `TotalAmount = 0m`, neutral msg | `AgentValidationException` c/ `"total_amount"` | 23 |
| TotalAmount = null | `CustomerId` set, `TotalAmount = null`, neutral msg | `AgentValidationException` c/ `"total_amount"` | 19, 25, 26, 55 |
| InvoiceId missing | `Reason` set, `InvoiceId = null`, neutral msg | `AgentValidationException` c/ `"invoice_id"` | 39, 40, 61 |
| Create log captured | valid create + recording logger | log call w/ template containing `Calling MCP tool` and arg `"erp.create_order"` | 27, 28, 29 |
| Cancel log captured | valid cancel + recording logger | log call w/ template containing `Calling MCP tool` and arg `"erp.cancel_invoice"` | 48, 49, 50 |
| Unknown intent | msg `"olá mundo"`, no IDs | `UnsupportedIntentException` w/ message | 10, 82 |
| keyword: `criar` only | msg `"criar estoque"`, no IDs | `UnsupportedIntentException` | 68, 69, 73 |
| keyword: `pedido` only | msg `"pedido de compra"`, no IDs | `UnsupportedIntentException` | 71, 72, 73 |
| keyword: `cancel` only | msg `"cancelar algo"`, no IDs | `UnsupportedIntentException` | 79, 80, 81 |
| keyword: `fatura` only | msg `"fatura atrasada"`, no IDs | `UnsupportedIntentException` | 77 |
| keyword: `cancel` + `fatura` | msg `"cancelar fatura"`, no IDs | `AgentValidationException` c/ `"invoice_id"` | 78, 84 |

Rationale over alternatives: behavioral black-box tests keep the production API intact and mirror real usage; no test-only seams (e.g., `internal` intent accessor) are needed except for logging observability.

### D2. Recording logger helper for log mutations

Introduce a small `RecordingLogger<T> : ILogger<T>` in `Agent.Application.Tests` that captures `LogInformation` calls (message template + args) into an in-memory list. It replaces `NullLogger` in the two happy-path tests so log template/argument mutations become assertable. Alternative (mocking `ILogger<T>` with Moq) was rejected: the test project doesn't reference Moq and a hand-rolled helper is dependency-free and explicit.

### D3. Mutant 85 (`return "unsupported"` → `""`) is equivalent

The `ExecuteAsync` switch routes any non-`create_order`/`cancel_invoice` string to the default arm, so the returned string is unobservable behavior. Killed: no behavioral test can distinguish it. Decision: leave it as a documented equivalent survivor; optionally exclude via `ignore-mutations`/file-span later if noise is a concern.

### D4. Verification loop

After adding tests: run `dotnet-stryker` for the agent-service only (`cd services/agent-service/tests/Agent.Application.Tests && dotnet-stryker`) and confirm K ≈ 72, S = 1 (mutant 85), score ≥ 60%. Keep `coverage-analysis: "off"` (xunit.v3 is incompatible with Stryker's coverage capture).

## Risks / Trade-offs

- [Keyword tests couple to message wording] → `DetectIntent` owns the keywords; changing wording requires updating these tests. Acceptable: they encode current behavior explicitly.
- [Logging tests are slow/brittle] → the recording logger is in-memory and fast; only the two happy-path tests switch to it.
- [Score lands at ~99%, not 100%] → the remaining survivor (85) is documented as equivalent; gate passes at ≥60%.
- [Other services still gate-fail] → out of scope; tracked as follow-up after they produce valid reports.
