# agentic-erp-platform-mvp

MVP de referência que demonstra como modernizar um ERP legado usando IA agêntica, sem tocar no núcleo do sistema, aplicando padrões arquiteturais modernos e práticas enterprise.

[![Spec-Driven Development](https://img.shields.io/badge/SDD-OpenSpec-yellow)](openspec/specs/architecture-foundation/spec.md)

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

---

## 🧑‍💻 Para Usuários

### Como Instalar

A forma mais simples é usar as imagens de container publicadas nas
[Releases](https://github.com/amaurycarvalho/agentic-erp-platform-mvp/releases):

1. Baixe os tarballs dos serviços desejados (`*-service.tar.gz`);
2. Carregue cada um com `docker load`;
3. Suba a stack com `docker-compose up -d` (referenciando as imagens carregadas).

Também é possível construir tudo do código-fonte:

```bash
git clone https://github.com/amaurycarvalho/agentic-erp-platform-mvp.git
cd agentic-erp-platform-mvp
make install
make build-images
docker-compose up -d --build
```

### Como Usar

Após subir a stack, a API de cada serviço fica disponível:

- `agent-service`: http://localhost:8080
- `mcp-service`: http://localhost:8082
- `rag-service`: http://localhost:8083
- `erp-acl-service`: http://localhost:8084

Exemplo de chamada de busca de contexto no RAG:

```bash
curl -X POST http://localhost:8083/rag/search \
  -H "Content-Type: application/json" \
  -d '{"operation_context":"order.create","correlation_id":"USR-001"}'
```

Para encerrar os serviços:

```bash
docker-compose down
```

---

## 👨‍🔧 Para Desenvolvedores

### Como Instalar

#### Baixando o codigo fonte

```bash
git clone https://github.com/amaurycarvalho/agentic-erp-platform-mvp.git
```

#### Como Compilar

```bash
make install
make build
```

Requisitos:

- .NET SDK 8.0
- Docker (para `make build-images` e `docker-compose`)

#### Testes

```bash
make test
```

Executa os testes unitários de todos os serviços (excluindo a integração MCP que
exige stack ativa). Para validar a cadeia `agent -> mcp -> acl` ponta a ponta, suba
a stack e rode a integração:

```bash
docker-compose up -d --build
MCP_BASE_URL=http://localhost:8082 make test-integration
```

#### Quality Gate

Por enquanto o _quality gate_ é apenas de testes (`install` + `test`). Os demais
controles (lint, segurança, complexidade, cobertura, mutação) serão incorporados
em uma fase futura.

```bash
make quality-gate
```

### Como Usar

#### Docker Compose

Suba os serviços (constrói as imagens a partir dos `Dockerfile`):

```bash
docker-compose up -d --build
```

Acesse via:

- `agent-service`: http://localhost:8080
- `mcp-service`: http://localhost:8082
- `rag-service`: http://localhost:8083
- `erp-acl-service`: http://localhost:8084 (gRPC em 8081)

Derrube os serviços:

```bash
docker-compose down
```

#### Usando Imagens Pré-compiladas

Para usar uma imagem publicada em uma Release, baixe o tarball do serviço, carregue
no Docker e suba com o `docker-compose` (ou outro orquestrador):

```bash
# Download: pegue agent-service.tar.gz da Release
gunzip -c agent-service.tar.gz | docker load
docker tag agent-service:<versão> agent-service:latest
# Volte a referenciar a imagem no docker-compose.yml (ex.: image: agent-service:<versão>)
docker-compose up -d
```

> O mesmo procedimento se aplica a `mcp-service.tar.gz`, `erp-acl-service.tar.gz`
> e `rag-service.tar.gz`.

---

## Saiba Mais

- [Repositório do projeto](https://github.com/amaurycarvalho/agentic-erp-platform-mvp)
- [Releases com binários pré-compilados](https://github.com/amaurycarvalho/agentic-erp-platform-mvp/releases)
