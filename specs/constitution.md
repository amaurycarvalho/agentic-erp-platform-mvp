# Constitution

Este documento define os princípios imutáveis do sistema.

---

## Princípios gerais

1. O ERP legado é um sistema estável e intocável;
2. Nenhuma IA acessa diretamente o ERP;
3. Toda automação deve ser reversível e auditável;
4. Decisão é separada de execução;
5. Conhecimento (RAG) não executa ações;
6. Capacidades expostas à IA devem ser explícitas (MCP);
7. Especificações precedem código.

Estes princípios não são opcionais.

---

## Backlog

### Padrão de Especificação de Casos de Uso

Todos os casos de uso deste projeto devem ser escritos seguindo uma abordagem em camadas:

1. Uma História de Usuário em alto nível, no formato:
   "Como [ator], preciso [funcionalidade] para que [benefício/valor]"

2. Critérios de Aceite explícitos, descrevendo as regras de negócio e restrições envolvidas.

3. Cenários comportamentais escritos no formato BDD (Dado / Quando / Então),
   garantindo que as especificações possam ser validadas e automatizadas.

Este padrão se aplica a todos os serviços do projeto e é obrigatório para novas funcionalidades.

### Expectativas mínimas de testes

- Cada caso de uso deverá ter pelo menos 1 cenário de teste mapeado em formato BDD;
- Todos os cenários de testes, exceto os marcados como exploratórios, deverão ter testes automatizados implementados.

---

## Tecnologia

### Princípios arquiteturais obrigatórios

- Consultar a pasta 'adr' para mais informações.

### Codificação

#### Limites explícitos de abstração

#### Regras de naming (código, módulos, APIs)

##### Convenções gRPC em C#

- Todos os serviços gRPC utilizam `option csharp_namespace`;
- Os clients devem ser instanciados diretamente a partir da classe do serviço gerada;
- Testes de contrato devem validar apenas campos definidos no .proto

#### Padrões proibidos

#### Restrições tecnológicas

- Não utilizar frameworks, SDKs, bibliotecas, componentes ou serviços de terceiros que sejam pouco conhecidos no mercado, que não tenham sido suficientemente validados em uso profissional ou que não estejam sendo ativamente mantidos por uma base sólida de desenvolvedores ou empresas;
- Alertar se qualquer dessas restrições forem encontradas no código.

#### Segurança

- Alertar se qualquer parte do código conter vulnerabilidades já conhecidas (OWASP, CISA) e propor a solução;
- Propor a remoção de pacotes que não estiverem sendo ativamente usados no projeto.

### Testes

#### Estratégia de teste

- Uso de TDD;
- Todo endpoint deverá ter pelo menos um teste unitário associado;
- Cobertura de 100% de testes unitários em código de média e alta criticidade;
- Cobertura de 90% de testes unitários em código de baixa criticidade.
