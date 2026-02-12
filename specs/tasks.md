# Tasks

## Concluído

- [x] Definir specs iniciais e princípios (constitution, requirements, ADRs base);
- [x] Criar ERP ACL dummy com gRPC e testes de contrato/aplicação;
- [x] Criar MCP server com catálogo de tools e execução via ACL;
- [x] Estruturar compose com health checks por serviço.
- [x] Automatizar testes com rastreabilidade explícita a partir das specs.
- [x] Implementar testes de integração `mcp-service` -> `erp-acl-service`.

## Em andamento

- [ ] Expandir cobertura de integração para cenários de resiliência (timeout/retry).

## Próximos passos

- [ ] Implementar casos de uso faltantes no `agent-service`;
- [ ] Implementar backlog inicial do `rag-service`;
- [ ] Padronizar observabilidade (correlation id, métricas e tracing);
- [ ] Definir política de versionamento de contratos MCP tools.
