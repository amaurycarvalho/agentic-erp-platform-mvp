# ADR-002 – Decisões arquiteturais para o erp-acl-service

---

## Status

Aceito

## Contexto

Este ADR define decisões específicas do `erp-acl-service` dentro da base arquitetural do `ADR-001`.

O serviço ACL precisa expor capacidades de negócio do ERP sem contaminar o domínio da plataforma e com contrato interno estável para consumo do MCP.

---

## Decisão

1. Expor casos de uso do ACL via gRPC para consumo interno do `mcp-service`.
2. Centralizar contratos em `ErpAcl.Contracts` com `.proto` versionado.
3. Organizar serviços gRPC por contexto (`OrderService`, `InvoiceService`).
4. Validar regras de negócio por testes unitários de aplicação e estabilidade de DTO por testes de contrato.

---

## Justificativas

- gRPC oferece contratos fortemente tipados e baixa latência para comunicação interna.
- Centralização dos contratos reduz divergência entre provider e consumer.
- Separação por contexto evita contratos inchados e melhora evolução incremental.
- Estratégia de testes separa validação de regra de negócio de validação de contrato.

---

## Consequências

### Positivas

- Integração interna mais previsível entre MCP e ACL.
- Fronteira anticorrupção explícita e estável.
- Refatoração de infraestrutura com menor risco de quebra de contrato.

### Negativas / Trade-offs

- Exige disciplina de versionamento dos `.proto`.
- Amplia custo de coordenação em mudanças de contrato.

---

## Decisões Relacionadas

- `ADR-001-Architecture-Foundations`.
- `ADR-003-mcp-service`.
