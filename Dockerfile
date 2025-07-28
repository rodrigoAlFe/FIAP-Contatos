# Etapa Build para persistencia-api
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-persistencia
WORKDIR /src
COPY ./src/persistencia-api ./persistencia-api
WORKDIR /src/persistencia-api
RUN dotnet restore
RUN dotnet publish -c Release -o /out-persistencia

# Etapa Build para cadastro-api
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-cadastro
WORKDIR /src
COPY ./src/cadastro-api ./cadastro-api
WORKDIR /src/cadastro-api
RUN dotnet restore
RUN dotnet publish -c Release -o /out-cadastro

# Etapa Runtime para persistencia-api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS persistencia-api
WORKDIR /app
COPY --from=build-persistencia /out-persistencia .
EXPOSE 5148
ENTRYPOINT ["dotnet", "persistencia-api.dll"]

# Etapa Runtime para cadastro-api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS cadastro-api
WORKDIR /app
COPY --from=build-cadastro /out-cadastro .
EXPOSE 5083
ENTRYPOINT ["dotnet", "cadastro-api.dll"]