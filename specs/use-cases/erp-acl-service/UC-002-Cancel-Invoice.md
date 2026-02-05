# UC-002 – Cancelar Fatura

## Contexto
Faturas no sistema ERP legado podem precisar ser canceladas como ação corretiva.

Este caso de uso garante que os cancelamentos sigam regras de negócio controladas.

## Ator Principal
Serviço de Agente

## Objetivo
Cancelar uma fatura existente no sistema ERP.

## Pré-condições
- A fatura deve existir;
- A fatura não deve ter sido cancelada anteriormente.

## Fluxo Principal
1. O cliente envia uma solicitação de cancelamento de fatura;
2. O ACL verifica o status da fatura;
3. O adaptador ERP é invocado para cancelar a fatura;
4. Uma confirmação é retornada.

## Pós-condições
- A fatura é marcada como cancelada no ERP.

## Regras de Negócio
- Faturas canceladas não podem ser canceladas novamente;
- O motivo do cancelamento é obrigatório.

## Fora do escopo
- Estorno contábil;
- Geração de nota de crédito.

