# Glossary

## Geral

- ERP Legado: Sistema corporativo existente responsável por cálculos contábeis, fiscais e persistência transacional.
- IA Agêntica: Modelo de IA que interpreta intenção, planeja ações e orquestra fluxos.
- MCP: Model Context Protocol. Contrato explícito de capacidades disponíveis para a IA.
- RAG: Retrieval-Augmented Generation. Fonte controlada de conhecimento contextual.
- ACL: Anti-Corruption Layer. Camada que protege e traduz o ERP.
- Spec: Documento que define comportamento esperado antes de qualquer implementação.

## Termos do domínio

- Order: Representa uma intenção confirmada de compra. NÃO usar: Purchase, Sale, Transaction.

## Termos técnicos de integração

- Correlation ID: identificador único por requisição/fluxo para rastrear execução ponta a ponta entre serviços.
- `validation_error`: erro de validação de payload/entrada antes da execução da ação.
- `acl_business_error`: erro de regra de negócio retornado pelo `erp-acl-service`.
- `acl_unavailable`: erro de indisponibilidade/comunicação com o `erp-acl-service`.
- Tool Catalog: lista versionada de ferramentas MCP autorizadas para execução.
