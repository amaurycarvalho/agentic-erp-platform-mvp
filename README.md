# agentic-erp-platform-mvp

## Sinopse
MVP de referência que demonstra como modernizar um ERP legado usando IA agêntica, sem tocar no núcleo do sistema, aplicando padrões arquiteturais modernos e práticas enterprise.

---

## Visão geral

Este repositório apresenta um **MVP (Minimum Viable Product)** que demonstra, de forma prática e pragmática, como empresas podem **modernizar a forma como utilizam um ERP legado**, sem reescrevê-lo ou substituí-lo.

A proposta central é simples:

> **Manter o ERP estável, protegido e intocado, enquanto uma camada externa de IA agêntica assume decisões, automações e orquestração de processos.**

O projeto foi pensado como material de referência para **líderes técnicos e de negócio**, arquitetos, tech leads e gestores que enfrentam os desafios clássicos de sistemas legados críticos.

---

## O problema que este MVP endereça

Na maioria das organizações, o ERP legado:

- É crítico demais para ser alterado;
- Carrega regras complexas e pouco documentadas;
- Depende de processos manuais, planilhas e decisões humanas;
- Bloqueia iniciativas de inovação e automação.

Este MVP demonstra como **desacoplar inteligência, decisão e automação do núcleo do ERP**, reduzindo risco e criando espaço para evolução contínua.

---

## A proposta arquitetural

O projeto adota uma arquitetura baseada em **microserviços**, onde cada responsabilidade é claramente isolada:

- **IA Agêntica**: interpreta intenção, planeja ações e orquestra fluxos;
- **RAG (Retrieval-Augmented Generation)**: fornece contexto e conhecimento confiável para decisões;
- **MCP (Model Context Protocol)**: define e controla as capacidades expostas ao modelo de IA;
- **ACL (Anti-Corruption Layer)**: protege o ERP e traduz conceitos de negócio;
- **ERP Dummy**: simula um sistema legado real, sem dependências externas.

Tudo isso é construído sobre **Clean Architecture**, garantindo baixo acoplamento e alta testabilidade.

---

## Técnicas e conceitos utilizados (e por que)

### Agentic AI (IA Agêntica)
Modelo no qual a IA não apenas responde perguntas, mas **decide, planeja e executa ações** com base em objetivos e contexto.
No cenário de ERP, isso permite automatizar decisões operacionais que antes dependiam de intervenção humana.

### MCP (Model Context Protocol)
Define explicitamente **o que a IA pode ou não fazer**.
Funciona como uma fronteira de segurança entre o modelo de linguagem e os sistemas corporativos, reduzindo riscos operacionais.

### RAG (Retrieval-Augmented Generation)
Permite que a IA tome decisões com base em **documentação real, políticas e regras do negócio**, evitando respostas inventadas ou inconsistentes.

### Strangler Pattern
O ERP não é substituído de uma vez.
Funcionalidades e decisões são gradualmente deslocadas para a camada agêntica, reduzindo risco e permitindo evolução incremental.

### Clean Architecture
Separa domínio, aplicação e infraestrutura.
Garante que regras de negócio não dependam de frameworks, LLMs ou detalhes técnicos.

### DDD (Domain-Driven Design)
O domínio é tratado como cidadão de primeira classe.
Conceitos do negócio são modelados explicitamente, evitando que o ERP dite a linguagem do sistema.

### Microservices
Cada capacidade (agente, MCP, RAG, ACL) é isolada.
Isso permite escalar, evoluir ou substituir partes do sistema sem impacto sistêmico.

### SDD (Spec Driven Development)
O comportamento do sistema nasce de **specs** (especificações legíveis por humanos).
As specs guiam código, prompts, testes e contratos, alinhando times técnicos e de negócio.

---

## O que este MVP não é

- Não é um produto pronto;
- Não é um framework fechado;
- Não é uma tentativa de “colocar IA dentro do ERP”.

Este repositório é uma **demonstração arquitetural**, pensada para aprendizado, discussão e adaptação ao contexto real de cada empresa.

---

## Para quem este projeto é indicado

- Líderes técnicos avaliando modernização de legados;
- Gestores de negócio buscando automação com baixo risco;
- Arquitetos definindo estratégias de IA corporativa;
- Times explorando Agentic AI além de chatbots.

---

## Como usar este repositório

- Como referência arquitetural;
- Como base para POCs e MVPs internos;
- Como material de discussão entre TI e negócio;
- Como exemplo de integração responsável entre IA e sistemas críticos.

---

## Próximos passos sugeridos

- Adaptar o ERP Dummy para seu ERP real;
- Evoluir specs com regras específicas do negócio;
- Integrar mensageria e observabilidade;
- Adicionar governança e controles de segurança.

---

## Licença

Uso livre para fins educacionais e experimentais.

> Adapte, evolua e questione.


