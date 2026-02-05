# UC-001 – Cancelamento de Pedido

## Contexto
Cliente solicita cancelamento de pedido existente.

## Fluxo
1. Agente interpreta solicitação.
2. Consulta políticas via RAG.
3. Seleciona ferramenta MCP apropriada.
4. Executa ação via ACL.
5. Registra auditoria.

## Resultado esperado
Pedido cancelado conforme regras do negócio.
