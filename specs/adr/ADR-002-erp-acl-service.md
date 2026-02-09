# ADR-002 – Decisões arquiteturais para o erp-acl-service

---

## Status

Aceito

## Contexto

Uma Camada Anticorrupção (do inglês, Anti-Corruption Layer - ACL) em uma IA agêntica é um padrão de projeto arquitetural que funciona como uma ponte intermediária e tradutora entre a inteligência do agente (o "cérebro" da IA) e sistemas externos, APIs de terceiros ou sistemas legados.

O termo "anticorrupção" refere-se à proteção do modelo de dados interno da IA, impedindo que a complexidade, as inconsistências ou formatos de dados arcaicos de sistemas externos "corrompam" a lógica e a eficiência da IA.

---

## Decisão

Adotar as seguintes abordagens arquiteturais para o serviço ACL do ERP:

1. **Endpoints GRPC para uso interno do MCP server**.

---

## Justificativas

### 1. Endpoints GRPC para uso interno do MCP server

Os endpoints da camada ACL do ERP serão de consumo exclusivo do servidor MCP, exigindo máxima eficiencia e gestão minuciosa dos contratos envolvidos.

#### Comunicação entre Serviços

O serviço ERP ACL expõe seus casos de uso por meio de endpoints gRPC.
Essa decisão foi tomada para garantir:

- Contratos fortemente tipados entre os serviços;
- Limites técnicos claros, alinhados ao padrão ACL (Anti-Corruption Layer);
- Comunicação interna de baixa latência;
- Arquivos proto atuando como especificações executáveis dos contratos.

---

## Contratos gRPC

Os contratos gRPC do Erp ACL são centralizados no projeto ErpAcl.Contracts.

Este projeto é:

- Referenciado pelo ErpAcl.Api (provider)
- Referenciado pelos testes ErpAcl.Contract.Tests (consumer)

Testes de contrato validam exclusivamente a estrutura e estabilidade
dos DTOs gRPC, sem dependência de infraestrutura ou execução do host.

---

## Organização dos serviços gRPC no erp-acl-service

O serviço ACL expõe múltiplos serviços gRPC, organizados por contexto:

- OrderService: operações relacionadas a pedidos
- InvoiceService: operações relacionadas a faturas

Essa separação evita contratos inchados e mantém alinhamento com
os bounded contexts do domínio.

### Endpoints Expostos

- OrderService.CreateOrder
  - Cria um novo pedido no sistema ERP por meio da camada ACL;
  - Mapeia diretamente o UC-001 (Create Order).
- InvoiceService.CancelInvoice
  - Cancela uma fatura existente no sistema ERP;
  - Mapeia diretamente o UC-002 (Cancel Invoice).

Os contratos gRPC são definidos na camada API do serviço ERP ACL e são
considerados parte da interface interna pública do serviço para comunicação com outros microserviços da plataforma.

---

## Testes

### Estratégia de Testes Unitários

As regras de negócio expostas pelo ERP ACL Service são validadas por meio de testes unitários na camada de Aplicação.

Os seguintes princípios se aplicam:

- Os casos de uso são testados de forma isolada;
- O acesso ao ERP é simulado (mockado) por meio das interfaces de gateway;
- Adaptadores de infraestrutura são excluídos dos testes unitários;
- Os testes são derivados diretamente das especificações dos casos de uso (Spec-Driven Development).

Essa abordagem garante:

- As regras de negócio permanecem estáveis independentemente da implementação do ERP;
- Refatorações seguras dos adaptadores e camadas de integração;
- Alinhamento claro entre especificações, código e validação automatizada.

### Testes de Contrato (gRPC)

O projeto ErpAcl.Contract.Tests valida exclusivamente os contratos gRPC,
sem inicializar o host da aplicação.

Esses testes verificam:

- Estrutura dos DTOs gerados a partir dos arquivos .proto;
- Presença de campos obrigatórios;
- Estabilidade do contrato para consumidores externos.

Testes de contrato NÃO executam lógica de negócio nem infraestrutura.
Testes de integração são tratados em projetos separados.
