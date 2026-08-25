# US-003 - Expor Busca com Metadados de Rastreabilidade
---

## ID
`US-RAG-003`

## História de Usuário
**Como** responsável por operação e auditoria  
**preciso** que cada busca no RAG retorne metadados de rastreabilidade  
**para que** seja possível auditar origem, tempo e correlação das respostas.

### Critérios de Aceite
1. O endpoint `POST /rag/search` deve aceitar `correlation_id` (opcional) no payload;
2. A resposta deve conter `correlation_id`, `request_id` e `retrieved_at_utc`;
3. A resposta deve conter a lista de fontes recuperadas em `sources`;
4. O endpoint deve manter contrato estável para consumo do `agent-service`.

---

## Cenários de Teste

### TS-001: Retornar metadados de rastreabilidade no resultado
**Dado** uma busca válida no endpoint do RAG  
**Quando** o serviço processar a requisição  
**Então** a resposta deve incluir `correlation_id`, `request_id` e `retrieved_at_utc`.

### TS-002: Gerar rastreabilidade mesmo sem correlation_id informado
**Dado** que o consumidor não enviou `correlation_id`  
**Quando** o serviço processar a requisição  
**Então** o serviço deve preencher correlação utilizando o `request_id`.

