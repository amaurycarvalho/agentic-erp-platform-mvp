# US-001 - Recuperar Políticas por Contexto
---

## ID
`US-RAG-001`

## História de Usuário
**Como** `agent-service` consumidor do `rag-service`  
**preciso** recuperar políticas relevantes por contexto de operação  
**para que** decisões sejam tomadas com base em conhecimento aplicável ao fluxo atual.

### Critérios de Aceite
1. O `rag-service` deve aceitar busca por `operation_context` via `POST /rag/search`;
2. Para `operation_context` válido, o serviço deve retornar apenas fontes relevantes ao contexto;
3. Quando nenhuma fonte for encontrada, o serviço deve retornar `sources` vazio sem erro técnico;
4. `operation_context` vazio deve retornar `400` com erro `validation_error`.

---

## Cenários de Teste

### TS-001: Retornar políticas relevantes ao contexto informado
**Dado** que existem políticas para o contexto `order.create`  
**Quando** o consumidor chamar `POST /rag/search` com `operation_context` válido  
**Então** o serviço deve retornar apenas fontes aplicáveis ao contexto.

### TS-002: Retornar lista vazia quando não houver políticas
**Dado** que não existem políticas para o contexto informado  
**Quando** o consumidor chamar `POST /rag/search`  
**Então** o serviço deve retornar `sources` vazio sem erro técnico.

