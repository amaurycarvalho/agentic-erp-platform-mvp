# UC-001 – Criar Pedido

## Contexto
O Serviço ACL do ERP expõe uma interface controlada para criar pedidos em um sistema ERP legado.

Este caso de uso isola o núcleo do ERP de consumidores externos, como agentes de IA ou camadas de orquestração.

## Ator Principal
Serviço de Agente (ou qualquer orquestrador upstream)

## Objetivo
Criar um novo pedido de venda no sistema ERP por meio de um contrato estável e seguro.

## Pré-condições
- Os dados do pedido devem ser validados antes de chegarem ao ERP;
- O núcleo do ERP não é acessado diretamente.

## Fluxo Principal
1. O cliente envia uma solicitação de criação de pedido;
2. O ACL valida os campos obrigatórios;
3. O pedido é mapeado para o formato específico do ERP;
4. O adaptador do ERP é invocado;
5. O identificador do pedido criado é retornado.

## Pós-condições
- Um pedido é criado no ERP (ou ERP simulado);
- O núcleo do ERP permanece isolado.

## Regras de Negócio
- O total do pedido deve ser maior que zero;
- É necessário pelo menos um item (simplificado no MVP).

## Fora do escopo
- Cálculo de preços;
- Validação de estoque;
- Processamento de pagamentos.
