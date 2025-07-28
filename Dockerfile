# Etapa Build para persistencia-api
# caso use arquitetura amd64, descomente a linha abaixo
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-persistencia
# caso use arquitetura arm64, descomente a linha abaixo
FROM mcr.microsoft.com/dotnet/sdk:8.0.412-bookworm-slim-arm64v8 AS build-persistencia
WORKDIR /src
COPY ./src/persistencia-api ./persistencia-api
WORKDIR /src/persistencia-api
RUN dotnet restore
# Instala o dotnet-ef globalmente (necessário para gerar o bundle)
RUN dotnet tool install --global dotnet-ef
# Garante que o PATH do dotnet tools está disponível
ENV PATH="$PATH:/root/.dotnet/tools"
# compila o projeto
RUN dotnet build -c Release
# Gera o migrations bundle self-contained para linux-x64
RUN dotnet ef migrations bundle -v --force \
    --startup-project ./persistencia-api.csproj \
    --self-contained \
    --target-runtime linux-arm64 \
    --output /out-persistencia/migrations-bundle 
# Publica a aplicação
RUN dotnet publish -c Release -o /out-persistencia

# Etapa Build para cadastro-api
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-cadastro
WORKDIR /src
COPY ./src/cadastro-api ./cadastro-api
WORKDIR /src/cadastro-api
RUN dotnet restore
RUN dotnet publish -c Release -o /out-cadastro

# Etapa Runtime para persistencia-api
# caso use arquitetura amd64, descomente a linha abaixo
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS persistencia-api
# caso use arquitetura arm64, descomente a linha abaixo
FROM mcr.microsoft.com/dotnet/aspnet:8.0.18-bookworm-slim-arm64v8 AS persistencia-api
WORKDIR /app
COPY --from=build-persistencia /out-persistencia .
COPY --from=build-persistencia /out-persistencia/migrations-bundle .
RUN chmod +x /app/migrations-bundle
EXPOSE 5148
#ENTRYPOINT ["dotnet", "persistencia-api.dll"]
ENTRYPOINT ["/bin/sh", "-c", "/app/migrations-bundle --connection $ConnectionStrings__DefaultConnection && dotnet persistencia-api.dll"]

# Etapa Runtime para cadastro-api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS cadastro-api
WORKDIR /app
COPY --from=build-cadastro /out-cadastro .
EXPOSE 5083
ENTRYPOINT ["dotnet", "cadastro-api.dll"]