# FIAP-Contatos
Projeto Pós Tech Arquitetura de Sistemas .NET - FIAP

## Passo a Passo: Como Rodar o Projeto
1. Certifique-se de que tem o **Docker** e o **Docker Compose** instalados na sua máquina. Isso será necessário para subir todas as dependências do projeto.
2. Abra um terminal na pasta raiz do projeto, onde se encontra o arquivo `docker-compose.yml`.
3. Execute o seguinte comando para iniciar a aplicação e todas as dependências:
``` bash
   docker-compose up
```
Esse comando irá construir todas as imagens e iniciar os containers necessários para o projeto, como o banco de dados MySQL e a aplicação API.

> **Dica:** Para rodar os containers em segundo plano (background), utilize o comando `docker-compose up -d`.
>

## Executar Migrations do Banco de Dados
Se for a primeira execução será necessário executar as migrations para preparar o banco de dados com as tabelas e configurações iniciais, siga os passos abaixo:
1. Abra um terminal dentro do container da aplicação `fiap.contatos_api`. Para entrar no container, você pode usar um comando similar a este:
``` bash
   docker exec -it fiap.contatos_api bash
```
Isso abrirá um shell bash dentro do container.
1. Navegue até a pasta `src` da aplicação:
``` bash
   cd /src
```
1. Execute o script responsável pelas migrations:
``` bash
   ./migrations.sh
```
Esse script irá aplicar as migrations usando o Entity Framework Core e preparar o banco de dados.
## Observações
- Após rodar o comando `docker-compose up`, o projeto estará disponível na porta definida no arquivo `docker-compose.yml`. Geralmente, o endereço será algo como:
``` 
  http://localhost:8080/swagger
```
- O script `./migrations.sh` garante que o banco de dados seja configurado corretamente com base no código atual.
- Sempre que houver mudanças em entidades ou na estrutura de banco de dados, você poderá criar novas migrations e aplicar pelo mesmo processo.


### Estrutura de Projetos na Solução
A solução contém os seguintes projetos estruturados em pastas principais (`src` e `test`).

Estes refletem uma arquitetura dividida em camadas e componentes principais:
#### **1. Projetos Principais (`src`)**
Os projetos localizados na pasta `src` formam o núcleo funcional da aplicação. Eles incluem:
- **FIAP.Contatos**
  Representa o projeto principal da solução, representa a camada de apresentação, uma API Rest construída com ASP.NET Core.
- **FIAP.Contatos.Domain**
  Contém as entidades de domínio e os contratos (interfaces). Essa camada é responsável por representar as regras e a lógica de negócio essencial da aplicação.
- **FIAP.Contatos.Infrastructure**
  Inclui a infraestrutura necessária, como acesso ao banco de dados (através do Entity Framework Core), implementações de repositórios e integrações com outros serviços.
- **FIAP.Contatos.Service**
  O projeto que implementa os serviços de aplicação. Ele contém a lógica intermediária entre o domínio e a apresentação. Pode incluir serviços para gerenciar operações mais complexas.

#### **2. Projetos de Testes (`test`)**
Os projetos na pasta `test` são usados para garantir a qualidade do código por meio de testes automatizados:
- **Domain.Test**
  Contém testes unitários que verificam as regras de negócio na camada de domínio.
- **Infrastructure.Test**
  Contém testes relacionados à infraestrutura, como validação de repositórios ou interações com o banco de dados.

#### **3. Pastas Virtuais**
Há duas pastas virtuais que organizam os projetos dentro da solução:
- **src**
  Agrupa todos os projetos de implementação.
- **test**
  Agrupa todos os projetos de teste.
- **Estrutura Aninhada**
  Os projetos dentro da pasta `src` e `test` estão estruturados hierarquicamente no arquivo de solução para facilitar o gerenciamento.

### Observações sobre Ferramentas
Esta solução utiliza ferramentas modernas como:
- **EF Core para acesso a dados**.
- **MySQL com suporte a migrations**.
- **xUnit e Moq para testes unitários**.
- **Dependências de ASP.NET Core**, como ASP.NET Identity e OpenAPI, oferecem suporte para autenticação, documentação de API (Swagger) e outros serviços web.