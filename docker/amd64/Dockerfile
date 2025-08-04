# filepath: /docker-multi-arch-project/docker-multi-arch-project/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-persistencia
WORKDIR /src
COPY ./src/persistencia-api ./persistencia-api
WORKDIR /src/persistencia-api
RUN dotnet restore
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet build -c Release
RUN dotnet ef migrations bundle -v --force \
    --startup-project ./persistencia-api.csproj \
    --self-contained \
    --target-runtime linux-x64 \
    --output /out-persistencia/migrations-bundle 
RUN dotnet publish -c Release -o /out-persistencia

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS persistencia-api
WORKDIR /app
COPY --from=build-persistencia /out-persistencia .
COPY --from=persistencia /out-persistencia/migrations-bundle .
RUN chmod +x /app/migrations-bundle
EXPOSE 5148
ENTRYPOINT ["/bin/sh", "-c", "/app/migrations-bundle && dotnet persistencia-api.dll"]

# Etapa Build para cadastro-api
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-cadastro
WORKDIR /src
COPY ./src/cadastro-api ./cadastro-api
WORKDIR /src/cadastro-api
RUN dotnet restore
RUN dotnet publish -c Release -o /out-cadastro

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS cadastro-api
WORKDIR /app
COPY --from=build-cadastro /out-cadastro .
EXPOSE 5083
ENTRYPOINT ["dotnet", "cadastro-api.dll"]