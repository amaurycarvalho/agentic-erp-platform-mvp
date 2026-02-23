# Tasks

## Concluído

- [x] Definir specs iniciais e princípios (constitution, requirements, ADRs base);
- [x] Criar ERP ACL dummy com gRPC e testes de contrato/aplicação;
- [x] Criar MCP server com catálogo de tools e execução via ACL;
- [x] Estruturar compose com health checks por serviço.
- [x] Automatizar testes com rastreabilidade explícita a partir das specs.
- [x] Implementar testes de integração `mcp-service` -> `erp-acl-service`.
- [x] Expandir cobertura de integração para cenários de resiliência (timeout/retry).

## Próximos passos

- [x] Implementar casos de uso faltantes no `agent-service`;
- [ ] Implementar backlog inicial do `rag-service`;
- [ ] Implementar observabilidade (correlation id, métricas e tracing);
- [ ] Observabilidade `mcp-service`: consolidar estratégia de retry com telemetria por tentativa (logs e métricas);
- [ ] Observabilidade `erp-acl-service`: padronizar logs estruturados, métricas de health e tracing de chamadas gRPC;
- [ ] Observabilidade `agent-service`: instrumentar execução de casos de uso com correlation id, métricas e tracing;
- [ ] Observabilidade `rag-service`: definir baseline de métricas, tracing e logs para pipeline de recuperação e geração;
- [ ] Definir política de versionamento de contratos MCP tools.
