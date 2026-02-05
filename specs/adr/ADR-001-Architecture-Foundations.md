# ADR-001 – Arquitetura Base do agentic-erp-platform-mvp

## Status
Aceito

## Contexto

Este projeto tem como objetivo demonstrar, em formato MVP, como modernizar a utilização de um ERP legado por meio de IA agêntica, reduzindo riscos, preservando o núcleo do sistema e permitindo evolução incremental.

O cenário considerado envolve:
- ERP crítico e intocável;
- Regras de negócio distribuídas e pouco documentadas;
- Forte dependência de decisões humanas;
- Necessidade de automação com governança, auditabilidade e baixo acoplamento.

Diante desse contexto, foi necessário definir uma base arquitetural sólida, compreensível por líderes técnicos e de negócio, e sustentável a longo prazo.

---

## Decisão

Adotar as seguintes abordagens arquiteturais como **fundação do projeto**:

1. **DDD (Domain-Driven Design)**;
2. **Clean Architecture**;
3. **Arquitetura de Microserviços**;
4. **C# / .NET como plataforma principal**;
5. **Strangler Pattern para evolução do legado**;
6. **Spec Driven Development (SDD) como prática central**;
7. **IA Agêntica com MCP e RAG desacoplados**;
8. **TDD e BDD**;
9. **Containers docker por serviço gerenciados por docker-compose**.

---

## Justificativas

### 1. Domain-Driven Design (DDD)

O ERP legado impõe uma linguagem técnica e estrutural que não representa corretamente o negócio.

DDD é adotado para:
- Criar uma linguagem ubíqua entre negócio e tecnologia;
- Evitar que modelos do ERP contaminem o domínio;
- Permitir evolução independente do legado;
- Modelar decisões, intenções e políticas explicitamente.

O domínio passa a ser a principal referência, não o banco ou o ERP.

---

### 2. Clean Architecture

Clean Architecture é utilizada para garantir que:
- Regras de negócio não dependam de frameworks, LLMs ou infraestrutura;
- O ERP, APIs externas e modelos de IA sejam detalhes substituíveis;
- O sistema seja testável e evolutivo.

Isso é essencial em um cenário onde tecnologias de IA e ERPs podem mudar ao longo do tempo.

---

### 3. Microserviços

A solução é organizada em microserviços para:
- Isolar responsabilidades (agente, MCP, RAG, ACL);
- Reduzir blast radius de falhas;
- Permitir escalabilidade e evolução independente;
- Facilitar governança e segurança.

Cada microserviço representa uma capacidade clara de negócio ou plataforma.

---

### 4. C# / .NET

C# e .NET foram escolhidos por:
- Forte maturidade em ambientes corporativos;
- Excelente suporte a Clean Architecture e DDD;
- Ecossistema robusto para APIs, mensageria e observabilidade;
- Integração nativa com Azure OpenAI e serviços cloud;
- Alta produtividade para times enterprise.

A escolha privilegia clareza, segurança e sustentabilidade.

---

### 5. Strangler Pattern

O ERP legado não é substituído ou reescrito.

O Strangler Pattern permite:
- Evolução incremental;
- Redução de risco operacional;
- Transferência gradual de decisões e automações;
- Convivência controlada entre legado e novas capacidades.

A IA agêntica atua como camada externa que “envolve” o ERP.

---

### 6. Spec Driven Development (SDD)

SDD é adotado para:
- Alinhar negócio e tecnologia desde o início;
- Reduzir ambiguidades em sistemas baseados em IA;
- Tornar decisões auditáveis e rastreáveis;
- Guiar código, prompts, testes e contratos.

Nenhuma funcionalidade existe sem uma spec explícita.

---

### 7. IA Agêntica com MCP e RAG

A IA é tratada como:
- Um agente de decisão e orquestração;
- Nunca como executora direta de regras críticas;

MCP define claramente o que a IA pode fazer.
RAG fornece contexto confiável para decisões.
O ERP permanece protegido por uma ACL dedicada.

---

## 8. TDD e BDD

TDD (Test-Driven Development) e BDD (Behavior-Driven Development) aumentam a qualidade do software, reduzem bugs e facilitam a manutenção através de testes automatizados. O TDD foca na qualidade do código unitário e refatoração segura (dentro para fora), enquanto o BDD alinha regras de negócio a cenários de comportamento compreensíveis por todos, garantindo que a funcionalidade correta seja desenvolvida (fora para dentro). 

## 9. Containers docker por serviço gerenciados por docker-compose

Containers Docker oferecem alta portabilidade, padronização entre ambientes (eliminando o "funciona na minha máquina"), rapidez na implantação e escalabilidade eficiente. Leves e isolados, consomem menos recursos (CPU/RAM) que máquinas virtuais, facilitando o desenvolvimento, CI/CD e a execução de microsserviços com segurança e agilidade. 

---

## Consequências

### Positivas
- Baixo acoplamento com o ERP;
- Alta governança e auditabilidade;
- Evolução incremental e segura;
- Arquitetura compreensível para líderes técnicos e de negócio;
- Facilidade de adaptação a novos ERPs ou modelos de IA.

### Negativas / Trade-offs
- Maior complexidade inicial;
- Mais serviços e contratos para gerenciar;
- Necessidade de maturidade arquitetural do time.

Esses trade-offs são considerados aceitáveis para o contexto enterprise proposto.

---

## Decisões Relacionadas (futuras ADRs)

- Estratégia de mensageria e eventos;
- Observabilidade e auditoria;
- Segurança e controle de acesso;
- Governança de prompts e modelos;
- Estratégia de testes baseados em specs.
