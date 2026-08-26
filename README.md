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
2. Baixe também o `docker-compose.release.yml` da mesma Release;
3. Carregue cada tarball com `docker load` e retague a imagem para `latest`;
4. Suba a stack com `docker-compose -f docker-compose.release.yml up -d`.

```bash
# Download: pegue os tarballs (*-service.tar.gz) e o docker-compose.release.yml da Release
for img in agent-service mcp-service erp-acl-service rag-service; do
  gunzip -c "$img.tar.gz" | docker load
  docker tag "$img:<versão>" "$img:latest"
done
docker-compose -f docker-compose.release.yml up -d
```

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

#### Testes unitários

```bash
make test
```

Executa os testes unitários e coleta cobertura por serviço (exclui a integração MCP
que exige stack ativa). Para validar a cadeia `agent -> mcp -> acl` ponta a ponta, suba
a stack e rode a integração:

```bash
docker-compose up -d --build
MCP_BASE_URL=http://localhost:8082 make test-integration
```

#### Quality Gate

O _quality gate_ executa lint + testes (com cobertura) + verificação de cobertura +
métricas + segurança:

```bash
make quality-gate
```

Verificações individuais:

```bash
make lint               # formato/análise (dotnet format --verify-no-changes)
make test               # testes + cobertura
make coverage-check     # cobertura contra COVERAGE_THRESHOLD (default 80)
make metrics            # linhas de código (LOC) por serviço
make security           # pacotes vulnerables/deprecated/outdated + Semgrep SAST
```

Análise estática, complexidade, code smells, dívida técnica e rating de
manutenibilidade são coordenados pelo **SonarCloud** no CI, com um projeto por
serviço (análise per-service), _Leak Period_ sobre código novo e decoração de
Pull Requests. A cobertura é encaminhada via `TestResults/**/coverage.cobertura.xml`.

> **Jobs no CI:** o job `sonarcloud` (SonarCloud) e o job `integration-test`
> (integração MCP ponta a ponta) rodam **apenas em pull requests**. O job
> `quality-gate` (lint + test + coverage + metrics + security) roda em push para
> `main` e em pull requests.

#### Análise SonarCloud no pipeline CI

O SonarCloud requer a configuração dos secrets abaixo no Github:

```
SONAR_PROJECT_KEY_PREFIX
SONAR_ORG
SONAR_TOKEN
```

A chave do projeto no SonarCloud deverá seguir o padrão:

```
SONAR_PROJECT_KEY_PREFIX-service_name
```

Exemplo:

```
agentic-erp-agent-service
agentic-erp-mcp-service
agentic-erp-acl-service
agentic-erp-rag-service
```

#### Análise SonarQube local (self-hosted)

Para analisar os serviços localmente contra um servidor **SonarQube
self-hosted** em execução (ex.: `http://localhost:9000`), instale o scanner e
rode a análise per-service:

```bash
make sonar-install
SONAR_TOKEN=<seu-token> make sonar-check
```

O `sonar-check` executa `begin → build + test (com cobertura) → end` para cada
um dos quatro serviços, um projeto SonarQube por serviço (chave =
`SONAR_PROJECT_KEY_PREFIX` + nome do serviço, ex.: `agentic-erp-agent-service`).

##### Subindo um servidor SonarQube local (Docker Compose)

O repositório inclui uma stack local reproduzível (SonarQube Community +
PostgreSQL, com volumes persistentes) em `sonarqube/docker-compose.yml`,
baseada na referência oficial da SonarSource e com o mesmo hardening
(`read_only`, `tmpfs`, volumes nomeados). Fluxo completo:

```bash
make sonar-up        # sobe a stack e aguarda o SonarQube ficar pronto
# 1) Acesse http://localhost:9000 e faça login com admin / admin
# 2) Troque a senha no primeiro login (obrigatório)
# 3) My Account -> Security -> Tokens -> Generate (token de um usuário admin)
SONAR_TOKEN=<seu-token> make sonar-check   # analisa os 4 serviços
make sonar-down      # para a stack preservando os volumes
```

Com um token de um usuário **admin**, os quatro projetos per-service (as chaves
exibidas pelo `make sonar-up`, uma por serviço) são **criados automaticamente**
na primeira análise.

**Requisitos de host:**

- **Linux:** o Elasticsearch embutido exige `vm.max_map_count` maior; aplique
  `sudo sysctl -w vm.max_map_count=262144` (persista em `/etc/sysctl.conf`).
- **Docker Desktop (Windows/Mac):** reserve pelo menos 2–4 GB de memória para o
  engine (o compose define `SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true` para evitar
  a falha do `max_map_count`, que não é diretamente configurável nesses hosts).
- **Reset completo** (apaga dados da stack): `docker compose -f sonarqube/docker-compose.yml down -v`.
- As credenciais `admin`/`admin` são padrão de desenvolvimento local — não use
  em produção.

Variáveis de ambiente:

- `SONAR_HOST_URL` — URL do servidor SonarQube (default `http://localhost:9000`);
- `SONAR_TOKEN` — token de autenticação (obrigatório);
- `SONAR_PROJECT_KEY_PREFIX` — prefixo das chaves de projeto (default `agentic-erp-`).

> O estado local do scanner (`/.sonarqube`) é ignorado pelo git. A análise local
> usa os mesmos relatórios de cobertura cobertura (`TestResults/**/coverage.cobertura.xml`)
> do `make test`, com exclusão de fontes de teste.

#### Mutation testing (opcional)

O teste de mutação com **Stryker.NET** é manual e não entra no gate do CI:

```bash
make install-quality-tools
make mutation
```

Os reportes de mutação são gerados com threshold `high/low/break` (`80/70/60`).

Leve os reports disponíveis em `services/**/tests/**/StrykerOutput/**/reports/mutation-report.json` e `services/**/tests/**/StrykerOutput/**/reports/mutation-report.html` para análise do seu agente de codificação e solicite a criação de testes para matar os mutantes sobreviventes. Depois, rode o mutation testing novamente.

#### Testes de integração

Ative os serviços com `Docker Compose`, configure a variável de ambiente com a url base e depois rode o teste.

```bash
sudo docker-compose up -d --build --timeout 120
MCP_BASE_URL=http://localhost:8082 make test-integration
sudo docker-compose down
```

### Como Usar

#### Docker Compose

Suba os serviços (constrói as imagens a partir dos `Dockerfile`):

```bash
sudo docker-compose up -d --build
```

Acesse via:

- `agent-service`: http://localhost:8080
- `mcp-service`: http://localhost:8082
- `rag-service`: http://localhost:8083
- `erp-acl-service`: http://localhost:8084 (gRPC em 8081)

Derrube os serviços:

```bash
sudo docker-compose down
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
