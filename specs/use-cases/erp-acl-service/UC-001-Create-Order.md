# UC-001 – Criar Pedido
---

## ID
`UC-ACL-001`

## História de Usuário
**Como** agente do sistema  
**preciso** criar um pedido no ERP legado por meio do serviço de ACL  
**para que** novas vendas possam ser registradas sem acessar diretamente o núcleo do ERP.

### Critérios de Aceite
1. O pedido deve conter um cliente válido;
2. O valor total do pedido deve ser maior que zero;
3. O sistema deve retornar o identificador do pedido criado.

---

## Cenários de Teste

### TS-001: Criar pedido válido
**Dado** que existe um cliente válido  
**E** o valor total do pedido é maior que zero  
**Quando** o agente solicitar a criação do pedido  
**Então** o pedido deve ser criado no ERP  
**E** um identificador de pedido deve ser retornado.

### TS-002: Criar pedido com valor inválido
**Dado** que o valor total do pedido é zero  
**Quando** o agente solicitar a criação do pedido  
**Então** o sistema deve rejeitar a operação  
**E** informar que o valor do pedido é inválido.
