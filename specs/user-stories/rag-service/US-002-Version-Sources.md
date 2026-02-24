# US-002 - Versionar Fontes e Respostas
---

## ID
`US-RAG-002`

## História de Usuário
**Como** `agent-service` consumidor do `rag-service`  
**preciso** receber fontes com versionamento explícito  
**para que** a resposta seja auditável e reproduzível por documento/política.

### Critérios de Aceite
1. Quando houver múltiplas versões da mesma `policy_code`, apenas a versão mais recente deve ser retornada;
2. Cada item de `sources` deve expor `source_id`, `policy_code`, `version` e `updated_at_utc`;
3. O serviço deve preservar rastreabilidade da versão usada na resposta.

---

## Cenários de Teste

### TS-001: Retornar somente a versão mais recente por política
**Dado** que existem versões `1.0` e `2.0` da mesma política  
**Quando** o consumidor chamar `POST /rag/search`  
**Então** o serviço deve retornar apenas a versão mais recente.

### TS-002: Expor metadados de versão por fonte recuperada
**Dado** que houve recuperação de fontes  
**Quando** o serviço responder a busca  
**Então** cada item deve conter `source_id`, `policy_code`, `version` e `updated_at_utc`.

