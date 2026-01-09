# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todos os projetos para /src
COPY FCG_Games ./FCG_Games
COPY FCG_Games.Application ./FCG_Games.Application
COPY FCG_Games.Domain ./FCG_Games.Domain
COPY FCG_Games.Infrastructure ./FCG_Games.Infrastructure

# Restaurar e publicar a WebAPI
WORKDIR /src/FCG_Games
RUN dotnet restore ./FCG_Games.WebAPI.csproj
RUN dotnet publish ./FCG_Games.WebAPI.csproj -c Release -o /app

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "FCG_Games.WebAPI.dll"]
