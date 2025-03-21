# Etapa Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
# RUN dotnet publish -c Release -o /out
RUN dotnet build -c Debug -o /out # Apenas build em vez de publish


# Etapa Runtime
FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 8080
CMD ["dotnet", "FIAP.Contatos.dll"]
