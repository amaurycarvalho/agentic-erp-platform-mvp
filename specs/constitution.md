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

## Pontos específicos

### Princípios arquiteturais obrigatórios

- Consultar a pasta 'adr' para mais informações.

### Limites explícitos de abstração

### Regras de naming (código, módulos, APIs)

### Padrões proibidos

### Expectativas mínimas de testes

- Cada caso de uso deverá ter pelo menos 1 caso de teste mapeado em formato BDD;
- Todos os casos de testes, exceto os marcados como exploratórios, deverão ter testes automatizados implementados;
- Todo endpoint deverá ter pelo menos um teste unitário associado;
- Cobertura de 100% de testes unitários em código de alta criticidade;
- Cobertura de 90% de testes unitários em código de média criticidade;
- Cobertura de 50% de testes unitários em código de baixa criticidade.

### Restrições tecnológicas

