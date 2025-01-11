# FIAP-Contatos
Projeto Pós Tech Arquitetura de Sistemas .NET - FIAP

## Como Rodar o Database MySQL

Use o docker


```shell
docker run --name mysql-fiap -e MYSQL_ROOT_PASSWORD=admin -p 3306:3306 -d mysql:latest
```

Caso queira criar uma migrations rode:
```shell
cd src/
dotnet ef migrations add V1 --project FIAP.Contatos.Infrastructure/FIAP.Contatos.Infrastructure.csproj --startup-project FIAP.Contatos/FIAP.Contatos.csproj --context FIAP.Contatos.Infrastructure.Data.ApplicationDbContext --verbose
```
Para aplicar as alterações no banco de dados, (ainda dentro do diretório src)
```shell
cd src/
dotnet ef database update V1  --project FIAP.Contatos.Infrastructure/FIAP.Contatos.Infrastructure.csproj --startup-project FIAP.Contatos/FIAP.Contatos.csproj --context FIAP.Contatos.Infrastructure.Data.ApplicationDbContext --verbose
```

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