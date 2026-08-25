# US-004 - Estratégia de Consistência RAG x ERP
---

## ID
`US-RAG-004`

## História de Usuário
**Como** agente que depende de contexto de negócio  
**preciso** saber o status de consistência entre fontes RAG e snapshot ERP  
**para que** decisões usem conhecimento atualizado e com risco controlado.

### Critérios de Aceite
1. O serviço deve classificar consistência como `fresh`, `stale` ou `unknown`;
2. `fresh`: fontes dentro da janela `max_source_age_days` (default `30`);
3. `stale`: fontes fora da janela configurada ou com divergência de versão com `erp_snapshot_version`;
4. `unknown`: ausência de fontes para avaliar consistência;
5. A resposta deve refletir `erp_snapshot_version` no bloco `consistency`.

---

## Cenários de Teste

### TS-001: Classificar como fresh com fontes atualizadas
**Dado** que as fontes estão dentro da janela configurada  
**Quando** o consumidor chamar `POST /rag/search`  
**Então** o status de consistência deve ser `fresh`.

### TS-002: Classificar como stale quando fontes estiverem desatualizadas
**Dado** que a fonte mais recente está fora da janela `max_source_age_days`  
**Quando** o consumidor chamar `POST /rag/search`  
**Então** o status de consistência deve ser `stale`.

### TS-003: Classificar como unknown quando não houver fontes
**Dado** que não há fontes para o contexto informado  
**Quando** o consumidor chamar `POST /rag/search`  
**Então** o status de consistência deve ser `unknown`.

