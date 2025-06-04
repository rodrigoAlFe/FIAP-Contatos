# FIAP-Contatos

Projeto desenvolvido para a Pós Tech FIAP - Arquitetura de Sistemas .NET, com foco em arquitetura moderna, integração de microsserviços, automação de infraestrutura e testes.

---

## Visão Geral

O sistema é composto por dois principais microsserviços:

- **cadastro-api**: API REST para gerenciamento de contatos, responsável pela orquestração e validação de dados.
- **persistencia-api**: API responsável pela persistência dos dados em banco relacional (MySQL).

A solução utiliza Docker para provisionamento dos ambientes, EF Core para acesso a dados, Prometheus e Grafana para observabilidade, e xUnit/Moq para testes.

---

## Como Executar o Projeto

### Pré-requisitos

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### Passos

1. Clone o repositório e acesse a pasta raiz.
2. Execute no terminal:
   docker-compose upEsse comando irá construir todas as imagens e iniciar os containers necessários para o projeto, como o banco de dados MySQL e a aplicação API.

> **Dica:** Para rodar os containers em segundo plano (background), utilize o comando `docker-compose up -d`.
>

## Executar Migrations do Banco de Dados
Se for a primeira execução será necessário executar as migrations para preparar o banco de dados com as tabelas e configurações iniciais, siga os passos abaixo:
1. Abra um terminal dentro do container da aplicação `fiap.contatos_api`. Para entrar no container, você pode usar um comando similar a este:   docker exec -it fiap.contatos_api bashIsso abrirá um shell bash dentro do container.
1. Navegue até a pasta `src` da aplicação:   cd /src1. Execute o script responsável pelas migrations:   ./migrations.shEsse script irá aplicar as migrations usando o Entity Framework Core e preparar o banco de dados.
## Observações
- Após rodar o comando `docker-compose up`, o projeto estará disponível na porta definida no arquivo `docker-compose.yml`. Geralmente, o endereço será algo como:  http://localhost:8080/swagger  # Documentação da API
  http://localhost:9090/metrics  # Métricas do Prometheus
  http://localhost:3000/         # Grafana- O script `./migrations.sh` garante que o banco de dados seja configurado corretamente com base no código atual.
- Sempre que houver mudanças em entidades ou na estrutura de banco de dados, você poderá criar novas migrations e aplicar pelo mesmo processo.

---

## Endpoints e Serviços

- **API de Contatos (cadastro-api):**
- `GET /api/contatos` — Lista todos os contatos
- `GET /api/contatos/{id}` — Busca contato por ID
- `POST /api/contatos` — Cria novo contato
- `PUT /api/contatos/{id}` — Atualiza contato existente
- `DELETE /api/contatos/{id}` — Remove contato

- **API de Persistência (persistencia-api):**
- Responsável por CRUD no banco de dados MySQL.

---

## Acesso às Interfaces

- **Swagger (Documentação da API):**  
[http://localhost:5083/swagger](http://localhost:5083/swagger)
- **Swagger (Documentação da Persistência da API):**  
[http://localhost:5148/swagger](http://localhost:5148/swagger)
- **Prometheus (Métricas):**  
[http://localhost:9090/](http://localhost:9090/)
- **Grafana (Dashboards):**  
[http://localhost:3000/](http://localhost:3000/)

---