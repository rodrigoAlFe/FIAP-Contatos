# Etapa Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o código de ambas as APIs
COPY ./src/persistencia-api ./persistencia-api
COPY ./src/cadastro-api ./cadastro-api

# Restaura e publica persistencia-api
WORKDIR /src/persistencia-api
RUN dotnet restore
RUN dotnet publish -c Release -o /out-persistencia

# Restaura e publica cadastro-api
WORKDIR /src/cadastro-api
RUN dotnet restore
RUN dotnet publish -c Release -o /out-cadastro

# Etapa Runtime para persistencia-api
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS persistencia-api
WORKDIR /app
COPY --from=build /out-persistencia .
EXPOSE 5148
ENTRYPOINT ["dotnet", "persistencia-api.dll"]

# Etapa Runtime para cadastro-api
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS cadastro-api
WORKDIR /app
COPY --from=build /out-cadastro .
EXPOSE 5083
ENTRYPOINT ["dotnet", "cadastro-api.dll"]