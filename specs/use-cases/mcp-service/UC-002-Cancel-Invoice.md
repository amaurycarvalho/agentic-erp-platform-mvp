# UC-002 – Cancelar Fatura via MCP
---

## ID
`UC-MCP-002`

## História de Usuário
**Como** agent-service consumidor do servidor MCP  
**preciso** executar a ferramenta `erp.cancel_invoice` no MCP  
**para que** uma fatura seja cancelada no ERP legado com governança e rastreabilidade.

### Critérios de Aceite
1. O MCP deve aceitar execução somente se a ferramenta estiver no catálogo (`MCP-TOOL-002`);
2. O request/response deve seguir exatamente o contrato `MCP-TOOL-002`;
3. A execução deve chamar exclusivamente `InvoiceService.CancelInvoice` no `erp-acl-service`;
4. O MCP deve retornar erro padronizado quando houver falha de validação, negócio ACL ou indisponibilidade;
5. O MCP deve registrar auditoria da execução.

---

## Cenários de Teste

### TS-001: Cancelar fatura válida via ferramenta MCP
**Dado** que `MCP-TOOL-002` está disponível no catálogo MCP  
**E** o payload atende o contrato  
**Quando** o agent-service solicitar a execução  
**Então** o MCP deve executar via `InvoiceService.CancelInvoice`  
**E** retornar `success` conforme contrato.

### TS-002: Rejeitar payload inválido sem chamar ACL
**Dado** que `MCP-TOOL-002` está disponível no catálogo MCP  
**E** o payload não atende o contrato  
**Quando** o agent-service solicitar a execução  
**Então** o MCP deve retornar `validation_error`  
**E** não deve chamar o ERP ACL.
