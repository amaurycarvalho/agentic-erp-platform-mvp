# UC-001 – Solicitar Criação de Pedido via MCP
---

## ID
`UC-AGENT-001`

## História de Usuário
**Como** agente do sistema  
**preciso** interpretar a intenção de criar pedido e acionar o MCP  
**para que** o pedido seja criado no ERP por meio da cadeia MCP -> ACL.

### Critérios de Aceite
1. O agente deve transformar solicitação natural em chamada da ferramenta `erp.create_order`;
2. O payload enviado ao MCP deve obedecer o contrato `MCP-TOOL-001`;
3. O agente deve tratar respostas de sucesso e erro padronizado do MCP;
4. O agente não deve acessar ERP diretamente.

---

## Cenários de Teste

### TS-001: Solicitação válida de criação de pedido
**Dado** que o usuário solicita criação de pedido  
**E** os dados necessários estão presentes  
**Quando** o agente processar a intenção  
**Então** deve chamar `erp.create_order` no MCP  
**E** retornar o `order_id` para o consumidor.

### TS-002: Solicitação com dados insuficientes
**Dado** que o usuário solicita criação de pedido sem dados obrigatórios  
**Quando** o agente processar a intenção  
**Então** deve solicitar complementação ou retornar erro de validação  
**E** não deve forçar execução inválida no MCP.
