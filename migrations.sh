#!/bin/sh
set -e  # Para o script caso algum comando falhe

echo "Preparando pra roda as migrations.."
dotnet tool install --global dotnet-ef
export PATH="$PATH:/root/.dotnet/tools"

echo "Executando migrations..."
dotnet ef database update --project src/FIAP.Contatos.Infrastructure/FIAP.Contatos.Infrastructure.csproj --startup-project src/FIAP.Contatos/FIAP.Contatos.csproj --context FIAP.Contatos.Infrastructure.Data.ApplicationDbContext
