# Traceability – Mcp.Application.Tests

## Mapping

- `REQ-FUNC-003` -> `ListToolsUseCaseTests`, `GetToolUseCaseTests`, `ValidatePayloadUseCaseTests`, `ExecuteMcpToolUseCaseTests`
- `REQ-FUNC-004` -> `ExecuteMcpToolUseCaseTests`
- `REQ-FUNC-005` -> `ExecuteMcpToolUseCaseTests`
- `UC-MCP-001` -> `ValidatePayloadUseCaseTests`, `ExecuteMcpToolUseCaseTests`
- `UC-MCP-002` -> `ValidatePayloadUseCaseTests`, `ExecuteMcpToolUseCaseTests`

## Convention

Os testes utilizam `Trait("REQ", "...")` e `Trait("UC", "...")` para permitir filtros e relatórios por spec.
