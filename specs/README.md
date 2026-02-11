# Specs – agentic-erp-platform-mvp

Este diretório contém as especificações formais do projeto (SDD).

Se não está especificado aqui, não faz parte do sistema.

## Estrutura

- `constitution.md`: princípios e regras mandatórias;
- `glossary.md`: linguagem comum de domínio;
- `requirements.md` e `non-functional-requirements.md`: requisitos do sistema;
- `use-cases/`: comportamento esperado por serviço;
- `adr/`: decisões arquiteturais;
- `edge-cases.md`: exceções importantes;
- `plan.md` e `tasks.md`: evolução e execução.

## Fonte de verdade por tipo

- Regras globais e transversais: `requirements.md`, `non-functional-requirements.md`, `constitution.md`.
- Comportamento de negócio por fluxo: `use-cases/`.
- Contratos de payload/erro de ferramentas MCP: `specs/use-cases/mcp-service/CONTRACT-tools.md`.
- Decisões estruturais de arquitetura: `adr/`.

## Matriz de rastreabilidade (MVP atual)

| Requisito | UC ID | Use Case | Contrato | Testes esperados |
|---|---|---|---|---|
| `REQ-FUNC-003`, `REQ-FUNC-004`, `REQ-FUNC-005` | `UC-MCP-001` | `specs/use-cases/mcp-service/UC-001-Create-Order.md` | `MCP-TOOL-001` | Unitários MCP + Contract ACL + Integração MCP->ACL |
| `REQ-FUNC-003`, `REQ-FUNC-004`, `REQ-FUNC-005` | `UC-MCP-002` | `specs/use-cases/mcp-service/UC-002-Cancel-Invoice.md` | `MCP-TOOL-002` | Unitários MCP + Contract ACL + Integração MCP->ACL |
| `REQ-FUNC-004` | `UC-ACL-001` | `specs/use-cases/erp-acl-service/UC-001-Create-Order.md` | `erp_acl.proto` (`OrderService.CreateOrder`) | Unitários aplicação ACL + contrato gRPC |
| `REQ-FUNC-004` | `UC-ACL-002` | `specs/use-cases/erp-acl-service/UC-002-Cancel-Invoice.md` | `erp_acl.proto` (`InvoiceService.CancelInvoice`) | Unitários aplicação ACL + contrato gRPC |
| `REQ-FUNC-001`, `REQ-FUNC-002`, `REQ-FUNC-003` | `UC-AGENT-001` | `specs/use-cases/agent-service/UC-001-Create-Order.md` | `MCP-TOOL-001` | Testes de orquestração do agent-service |
| `REQ-FUNC-001`, `REQ-FUNC-002`, `REQ-FUNC-003` | `UC-AGENT-002` | `specs/use-cases/agent-service/UC-002-Cancel-Invoice.md` | `MCP-TOOL-002` | Testes de orquestração do agent-service |

## Matriz Edge Cases (MVP atual)

| Edge Case | UC IDs prioritários |
|---|---|
| `EC-001` Solicitação ambígua/incompleta | `UC-AGENT-001`, `UC-AGENT-002` |
| `EC-002` Ação não exposta no MCP | `UC-MCP-001`, `UC-MCP-002` |
| `EC-003` Divergência RAG x ERP | `UC-AGENT-001`, `UC-AGENT-002`, `UC-MCP-001`, `UC-MCP-002` |
| `EC-004` Falha parcial de plano | `UC-AGENT-001`, `UC-AGENT-002` |
| `EC-005` Reexecução idempotente | `UC-MCP-001`, `UC-MCP-002`, `UC-ACL-001`, `UC-ACL-002` |
