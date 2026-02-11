# Plan

## Situação atual (MVP)

- `erp-acl-service` implementado com contratos gRPC para criação de pedido e cancelamento de fatura;
- `mcp-service` implementado com catálogo explícito de tools e execução via ACL;
- Estrutura de serviços (`agent-service`, `rag-service`, `mcp-service`, `erp-acl-service`) com health checks e compose;
- Base de specs e ADRs estabelecida.

## Próxima fase (curto prazo)

- Consolidar testes automatizados por rastreabilidade (`REQ` -> `UC` -> teste);
- Implementar testes de integração MCP -> ERP ACL;
- Completar backlog funcional de `agent-service` e `rag-service`;
- Padronizar observabilidade (logs estruturados, métricas, correlação).

## Fase seguinte (evolução)

- Expandir catálogo de ferramentas MCP;
- Introduzir segurança/autorização por ferramenta;
- Evoluir RAG com políticas versionadas e validação de consistência.
