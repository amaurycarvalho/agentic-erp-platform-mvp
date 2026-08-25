# Contrato de Busca do RAG

## Objetivo

Definir o contrato do endpoint de recuperação de contexto do `rag-service` para consumo pelo `agent-service`.

## Contrato de Busca (MVP)

### `RAG-SEARCH-001` - `POST /rag/search`

- Descrição: recupera políticas por contexto operacional com metadados de rastreabilidade.
- Input schema:
  - `operation_context` (string, obrigatório, não vazio)
  - `correlation_id` (string, opcional)
  - `erp_snapshot_version` (string, opcional)
  - `max_source_age_days` (number, opcional, > 0, default `30`)
- Output schema:
  - `operation_context` (string, obrigatório)
  - `correlation_id` (string, obrigatório)
  - `request_id` (string, obrigatório)
  - `retrieved_at_utc` (string datetime, obrigatório)
  - `consistency` (objeto, obrigatório)
  - `consistency.status` (string, obrigatório: `fresh`, `stale`, `unknown`)
  - `consistency.detail` (string, obrigatório)
  - `consistency.erp_snapshot_version` (string, opcional)
  - `sources` (array, obrigatório)
  - `sources[].source_id` (string, obrigatório)
  - `sources[].policy_code` (string, obrigatório)
  - `sources[].version` (string, obrigatório)
  - `sources[].updated_at_utc` (string datetime, obrigatório)
  - `sources[].excerpt` (string, obrigatório)
- Erros esperados:
  - `validation_error`

## Regras de Governança

1. O `rag-service` não executa ações de negócio no ERP.
2. O contrato de busca deve manter compatibilidade para consumo do `agent-service`.
3. Toda resposta deve conter metadados de rastreabilidade.
4. Mudanças incompatíveis exigem nova versão de contrato.

