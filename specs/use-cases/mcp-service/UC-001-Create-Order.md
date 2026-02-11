# UC-001 – Criar Pedido via MCP
---

## ID
`UC-MCP-001`

## História de Usuário
**Como** agent-service consumidor do servidor MCP  
**preciso** executar a ferramenta `erp.create_order` no MCP  
**para que** um pedido seja criado no ERP legado sem acesso direto ao núcleo ERP.

### Critérios de Aceite
1. O MCP deve aceitar execução somente se a ferramenta estiver no catálogo (`MCP-TOOL-001`);
2. O request/response deve seguir exatamente o contrato `MCP-TOOL-001`;
3. A execução deve chamar exclusivamente `OrderService.CreateOrder` no `erp-acl-service`;
4. O MCP deve retornar erro padronizado quando houver falha de validação, negócio ACL ou indisponibilidade;
5. O MCP deve registrar auditoria da execução.

---

## Cenários de Teste

### TS-001: Criar pedido válido via ferramenta MCP
**Dado** que `MCP-TOOL-001` está disponível no catálogo MCP  
**E** o payload atende o contrato  
**Quando** o agent-service solicitar a execução  
**Então** o MCP deve executar via `OrderService.CreateOrder`  
**E** retornar `order_id` conforme contrato.

### TS-002: Rejeitar payload inválido sem chamar ACL
**Dado** que `MCP-TOOL-001` está disponível no catálogo MCP  
**E** o payload não atende o contrato  
**Quando** o agent-service solicitar a execução  
**Então** o MCP deve retornar `validation_error`  
**E** não deve chamar o ERP ACL.
