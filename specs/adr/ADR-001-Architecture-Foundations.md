# ADR-001 – Arquitetura Base do agentic-erp-platform-mvp

---

## Status

Aceito

## Contexto

O projeto precisa modernizar uso de ERP legado sem alterar seu núcleo, com automação governável, auditável e evolutiva.

---

## Decisão

Adotar como fundação:

1. DDD;
2. Clean Architecture;
3. Microserviços;
4. C#/.NET como stack principal;
5. Strangler Pattern para evolução do legado;
6. Spec Driven Development (SDD);
7. IA agêntica desacoplada por MCP e RAG;
8. Histórias de usuário com TDD/BDD;
9. Containers por serviço com Docker Compose;
10. Comunicação interna priorizando gRPC e bordas HTTP conforme contexto.

---

## Estrutura de referência

```
agentic-erp-platform-mvp
|
+--agent-service
   |
   +--rag-service
   |
   +--mcp-service
      |
      +--erp-acl-service
         |
         +--erp
```

---

## Racional

- O ERP legado deve permanecer protegido e isolado.
- A inteligência de decisão (agente) deve ser separada da execução.
- Contratos explícitos (MCP/gRPC) reduzem ambiguidade e acoplamento.
- SDD e testes orientados por comportamento aumentam governança e rastreabilidade.

---

## Consequências

### Positivas

- Baixo acoplamento com ERP.
- Evolução incremental com menor risco.
- Maior testabilidade e auditabilidade.

### Negativas / Trade-offs

- Complexidade inicial maior.
- Mais contratos e fronteiras para manter.
- Maior exigência de disciplina arquitetural.

---

## Decisões Relacionadas

- `ADR-002-erp-acl-service`.
- `ADR-003-mcp-service`.
