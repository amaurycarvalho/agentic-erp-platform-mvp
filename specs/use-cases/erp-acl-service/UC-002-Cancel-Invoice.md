# UC-002 – Cancelar Fatura
---

## História de Usuário
**Como** agente do sistema  
**preciso** cancelar uma nota fiscal existente no ERP  
**para que** erros operacionais possam ser corrigidos de forma controlada.

### Critérios de Aceite
1. A nota fiscal deve existir;
2. A nota fiscal não pode estar previamente cancelada;
3. Um motivo de cancelamento deve ser informado;
4. Após o cancelamento, a nota deve ser marcada como cancelada no ERP.

---

## Cenários de Teste

### TS-001: Cancelar nota fiscal existente
**Dado** que existe uma nota fiscal válida  
**E** a nota fiscal não está cancelada  
**E** foi informado um motivo de cancelamento  
**Quando** o agente solicitar o cancelamento  
**Então** a nota fiscal deve ser marcada como cancelada no ERP.

### TS-002: Cancelar nota fiscal já cancelada
**Dado** que a nota fiscal já está cancelada  
**Quando** o agente solicitar o cancelamento  
**Então** o sistema deve rejeitar a operação  
**E** informar que a nota fiscal já foi cancelada.

### TS-003: Cancelar nota fiscal sem motivo
**Dado** que existe uma nota fiscal válida  
**Quando** o agente solicitar o cancelamento sem informar o motivo  
**Então** o sistema deve rejeitar a operação  
**E** informar que o motivo de cancelamento é obrigatório.
