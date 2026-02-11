# Constitution

Este documento define os princípios imutáveis do sistema.

---

## Princípios gerais

1. O ERP legado é um sistema estável e intocável.
2. Nenhuma IA acessa diretamente o ERP.
3. Toda automação deve ser reversível e auditável.
4. Decisão é separada de execução.
5. Conhecimento (RAG) não executa ações.
6. Capacidades expostas à IA devem ser explícitas (MCP).
7. Especificações precedem código.

Estes princípios não são opcionais.

---

## Padrão obrigatório de use case

Todos os casos de uso devem conter:

1. História de Usuário no formato: "Como [ator], preciso [funcionalidade] para que [benefício/valor]".
2. Critérios de Aceite explícitos.
3. Cenários comportamentais em BDD (`Dado / Quando / Então`).

---

## Diretrizes técnicas obrigatórias

- Consultar a pasta `specs/adr` para decisões arquiteturais vigentes.

### Convenções gRPC em C#

- Todo contrato gRPC deve usar `option csharp_namespace`.
- Clients devem ser instanciados a partir das classes geradas pelo `.proto`.
- Testes de contrato validam apenas campos definidos no `.proto`.

### Restrições tecnológicas

- Não utilizar frameworks, SDKs, bibliotecas, componentes ou serviços de terceiros pouco conhecidos, não validados profissionalmente ou sem manutenção ativa.

### Segurança e qualidade

- Alertar se houver vulnerabilidades conhecidas (OWASP, CISA) e propor mitigação.
- Propor remoção de pacotes não utilizados.

---

## Estratégia de testes

- Uso de TDD.
- Todo endpoint deve ter pelo menos um teste unitário associado.
- Cobertura mínima de 100% para código de média/alta criticidade.
- Cobertura mínima de 90% para código de baixa criticidade.
