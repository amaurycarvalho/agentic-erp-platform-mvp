# Specs – agentic-erp-platform-mvp

Este diretório contém as especificações formais do projeto (SDD).

Se não está especificado aqui, não faz parte do sistema.

## Estrutura

- `constitution.md`: princípios e regras mandatórias;
- `glossary.md`: linguagem comum de domínio;
- `requirements.md` e `non-functional-requirements.md`: requisitos do sistema;
- `user-stories/`: comportamento esperado por serviço;
- `adr/`: decisões arquiteturais;
- `edge-cases.md`: exceções importantes;
- `plan.md` e `tasks.md`: evolução e execução.

## Fonte de verdade por tipo

- Regras globais e transversais: `requirements.md`, `non-functional-requirements.md`, `constitution.md`.
- Comportamento de negócio por fluxo: `user-stories/`.
- Contratos de payload/erro de ferramentas MCP: `specs/user-stories/mcp-service/CONTRACT-tools.md`.
- Decisões estruturais de arquitetura: `adr/`.

## Matriz de rastreabilidade (MVP atual)

| Requisito | UC ID | Use Case | Contrato | Testes esperados |
|---|---|---|---|---|
| `REQ-FUNC-003`, `REQ-FUNC-004`, `REQ-FUNC-005` | `US-MCP-001` | `specs/user-stories/mcp-service/US-001-Create-Order.md` | `MCP-TOOL-001` | Unitários MCP + Contract ACL + Integração MCP->ACL |
| `REQ-FUNC-003`, `REQ-FUNC-004`, `REQ-FUNC-005` | `US-MCP-002` | `specs/user-stories/mcp-service/US-002-Cancel-Invoice.md` | `MCP-TOOL-002` | Unitários MCP + Contract ACL + Integração MCP->ACL |
| `REQ-FUNC-004` | `US-ACL-001` | `specs/user-stories/erp-acl-service/US-001-Create-Order.md` | `erp_acl.proto` (`OrderService.CreateOrder`) | Unitários aplicação ACL + contrato gRPC |
| `REQ-FUNC-004` | `US-ACL-002` | `specs/user-stories/erp-acl-service/US-002-Cancel-Invoice.md` | `erp_acl.proto` (`InvoiceService.CancelInvoice`) | Unitários aplicação ACL + contrato gRPC |
| `REQ-FUNC-001`, `REQ-FUNC-002`, `REQ-FUNC-003` | `US-AGENT-001` | `specs/user-stories/agent-service/US-001-Create-Order.md` | `MCP-TOOL-001` | Testes de orquestração do agent-service |
| `REQ-FUNC-001`, `REQ-FUNC-002`, `REQ-FUNC-003` | `US-AGENT-002` | `specs/user-stories/agent-service/US-002-Cancel-Invoice.md` | `MCP-TOOL-002` | Testes de orquestração do agent-service |

## Matriz Edge Cases (MVP atual)

| Edge Case | UC IDs prioritários |
|---|---|
| `EC-001` Solicitação ambígua/incompleta | `US-AGENT-001`, `US-AGENT-002` |
| `EC-002` Ação não exposta no MCP | `US-MCP-001`, `US-MCP-002` |
| `EC-003` Divergência RAG x ERP | `US-AGENT-001`, `US-AGENT-002`, `US-MCP-001`, `US-MCP-002` |
| `EC-004` Falha parcial de plano | `US-AGENT-001`, `US-AGENT-002` |
| `EC-005` Reexecução idempotente | `US-MCP-001`, `US-MCP-002`, `US-ACL-001`, `US-ACL-002` |
