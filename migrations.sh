#!/bin/sh
set -e  # Para o script caso algum comando falhe

echo "Preparando pra roda as migrations.."
dotnet tool install --global dotnet-ef
export PATH="$PATH:/root/.dotnet/tools"

echo "Executando migrations..."
dotnet ef database update --project src/persistencia-api/persistencia-api.csproj --startup-project src/persistencia-api/persistencia-api.csproj --context AppDbContext