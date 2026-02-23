# Contrato de Ferramentas do MCP

## Objetivo

Definir as capacidades executáveis do `mcp-service` para consumo do `agent-service`.

## Catálogo de Ferramentas (MVP)

### `MCP-TOOL-001` – `erp.create_order`

- Descrição: cria pedido no ERP via ACL.
- Transporte interno: gRPC -> `OrderService.CreateOrder`.
- Input schema:
  - `customer_id` (string, obrigatório, não vazio)
  - `total_amount` (number, obrigatório, > 0)
- Output schema:
  - `order_id` (string, obrigatório, não vazio)
- Erros esperados:
  - `validation_error`
  - `acl_business_error`
  - `acl_unavailable`

### `MCP-TOOL-002` – `erp.cancel_invoice`

- Descrição: cancela fatura no ERP via ACL.
- Transporte interno: gRPC -> `InvoiceService.CancelInvoice`.
- Input schema:
  - `invoice_id` (string, obrigatório, não vazio)
  - `reason` (string, obrigatório, não vazio)
- Output schema:
  - `success` (boolean, obrigatório)
- Erros esperados:
  - `validation_error`
  - `acl_business_error`
  - `acl_unavailable`

## Regras de Governança

1. Apenas ferramentas presentes neste catálogo podem ser executadas.
2. O MCP valida payload antes de chamar o ERP ACL.
3. Toda execução deve gerar evento auditável com correlação.
4. Mudanças incompatíveis exigem nova versão de contrato.
