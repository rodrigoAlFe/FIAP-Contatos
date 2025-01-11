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


## Pacotes Instalados
- MySql.Data 9.1.0
- MySql.EntityFrameworkCore 8.0.8
- Pomelo.EntityFrameworkCore.MySql 8.0.2
- xUnit 2.9.2
- Moq 4.20.72
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 8.0.8
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 8.0.8
- Microsoft.AspNetCore.OpenApi 8.0.11
- Microsoft.EntityFrameworkCore 8.0.8
- Microsoft.EntityFrameworkCore.Tools 8.0.8
- Microsoft.Extensions.Caching.Abstractions 9.0.0
- Microsoft.Extensions.Caching.Memory 8.0.1