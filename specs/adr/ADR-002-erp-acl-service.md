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

