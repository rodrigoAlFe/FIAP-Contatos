# FIAP-Contatos
Projeto Pós Tech Arquitetura de Sistemas .NET - FIAP

## Organização da solução

1. **FIAP.Api**:
    - Camada de apresentação da API, onde as requisições tratadas passam os dados para a aplicação.

2. **FIAP.Application**:
    - Camada de lógica de aplicação (interage com FIAP.Domain para aplicar as regras de negócio).

3. **FIAP.Domain**:
    - Contém as regras de domínio principal e entidades.

4. **FIAP.Infrastructure**:
    - Implementação concreta de conexões externas, como banco de dados ou APIs externas.

5. **FIAP.CrossCutting**:
    - Gerenciar preocupações transversais, como logs, autenticação e validação.