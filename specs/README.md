# Specs – agentic-erp-platform-mvp

Este diretório contém as **especificações formais (SDD – Spec Driven Development)** do projeto.

As specs são a **fonte primária de verdade**: código, prompts, contratos MCP e integrações devem existir apenas para implementar o que está definido aqui.

Se não está especificado aqui, **não faz parte do sistema**.

## Estrutura SDD:

- Princípios e limites do sistema (constitution);
- Linguagem comum de negócio (glossary);
- Requisitos funcionais e não funcionais (requirements e non-functional-requirements);
- Casos de uso orientados a valor organizados por serviço (use-cases/);
- Decisões arquiteturais (adr/)
- Exceções importantes (edge-cases);
- Plano evolutivo e backlog inicial (plan e tasks).

## Estrutura do projeto

- /specs: Fonte de verdade para comportamento, decisões e planejamento;
- /services: Implementações dos serviços descritos nas specs;
- /services/<serviço>/src: Código fonte do serviço;
- /services/<serviço>/tests: Testes unitários do serviço;
- /shared: Código compartilhado entre serviços.
