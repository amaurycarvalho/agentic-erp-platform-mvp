# ADR-003 – Decisões arquiteturais para o mcp-service

---

## Status

Aceito

## Contexto

Este ADR detalha decisões específicas do `mcp-service` sobre a base arquitetural definida no `ADR-001`.

O problema central do MCP no MVP é expor capacidades executáveis de forma explícita, validada e auditável, sem bypass da ACL.

---

## Decisão

1. Catálogo explícito de ferramentas com schema de entrada/saída e rota interna.
2. Execução por use cases de aplicação (`ListTools`, `GetTool`, `ValidatePayload`, `ExecuteTool`).
3. Integração exclusiva com ERP via gateway gRPC para `erp-acl-service`.
4. API mínima para descoberta e execução (`/mcp/tools`, `/mcp/tools/{toolName}`, `/mcp/tools/{toolName}/execute`, `/health`).
5. Taxonomia padronizada de erros (`tool_not_found`, `validation_error`, `acl_business_error`, `acl_unavailable`).

---

## Justificativas

- Mantém limites explícitos do que a IA pode executar.
- Garante validação antes de qualquer chamada ao ERP ACL.
- Preserva separação entre regra de aplicação e detalhes de infraestrutura.
- Oferece contrato estável para consumo pelo `agent-service`.
- Facilita observabilidade e troubleshooting por classe de erro.

---

## Consequências

### Positivas

- Capacidades rastreáveis e governáveis no MCP.
- Menor risco de execução fora de escopo.
- Melhor testabilidade e evolução incremental de tools.

### Negativas / Trade-offs

- Maior esforço de manutenção entre catálogo, validação e execução.
- Necessidade de coordenação de versionamento com contratos ACL.

---

## Decisões Relacionadas

- `ADR-001-Architecture-Foundations`.
- `ADR-002-erp-acl-service`.
