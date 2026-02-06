# Constitution

Este documento define os princípios imutáveis do sistema.

## Princípios gerais

1. O ERP legado é um sistema estável e intocável;
2. Nenhuma IA acessa diretamente o ERP;
3. Toda automação deve ser reversível e auditável;
4. Decisão é separada de execução;
5. Conhecimento (RAG) não executa ações;
6. Capacidades expostas à IA devem ser explícitas (MCP);
7. Especificações precedem código.

Estes princípios não são opcionais.

## Padrão de Especificação de Casos de Uso

Todos os casos de uso deste projeto devem ser escritos seguindo uma abordagem em camadas:

1. Uma História de Usuário em alto nível, no formato:
   "Como [ator], preciso [funcionalidade] para que [benefício/valor]"

2. Critérios de Aceite explícitos, descrevendo as regras de negócio e restrições envolvidas.

3. Cenários comportamentais escritos no formato BDD (Dado / Quando / Então),
   garantindo que as especificações possam ser validadas e automatizadas.

Este padrão se aplica a todos os serviços do projeto e é obrigatório para novas funcionalidades.

## Expectativas mínimas de testes

- Cada caso de uso deverá ter pelo menos 1 cenário de teste mapeado em formato BDD;
- Todos os cenários de testes, exceto os marcados como exploratórios, deverão ter testes automatizados implementados.

- TDD:
    - Todo endpoint deverá ter pelo menos um teste unitário associado;
    - Cobertura de 100% de testes unitários em código de alta criticidade;
    - Cobertura de 90% de testes unitários em código de média criticidade;
    - Cobertura de 50% de testes unitários em código de baixa criticidade.

## Tecnologia

### Princípios arquiteturais obrigatórios

- Consultar a pasta 'adr' para mais informações.

### Limites explícitos de abstração

### Regras de naming (código, módulos, APIs)

### Padrões proibidos

### Restrições tecnológicas

