# UC-002 – Solicitar Cancelamento de Fatura via MCP
---

## ID
`UC-AGENT-002`

## História de Usuário
**Como** agente do sistema  
**preciso** interpretar a intenção de cancelar fatura e acionar o MCP  
**para que** a fatura seja cancelada no ERP por meio da cadeia MCP -> ACL.

### Critérios de Aceite
1. O agente deve transformar solicitação natural em chamada da ferramenta `erp.cancel_invoice`;
2. O payload enviado ao MCP deve obedecer o contrato `MCP-TOOL-002`;
3. O agente deve tratar respostas de sucesso e erro padronizado do MCP;
4. O agente não deve acessar ERP diretamente.

---

## Cenários de Teste

### TS-001: Solicitação válida de cancelamento de fatura
**Dado** que o usuário solicita cancelamento de fatura com motivo  
**Quando** o agente processar a intenção  
**Então** deve chamar `erp.cancel_invoice` no MCP  
**E** retornar o resultado de sucesso para o consumidor.

### TS-002: Solicitação sem motivo de cancelamento
**Dado** que o usuário solicita cancelamento sem motivo  
**Quando** o agente processar a intenção  
**Então** deve solicitar complementação ou retornar erro de validação  
**E** não deve forçar execução inválida no MCP.
