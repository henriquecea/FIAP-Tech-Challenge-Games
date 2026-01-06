# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiando todos os projetos necess�rios e o arquivo de solu��o
COPY FCG_Games.WebAPI/ FCG_Games.WebAPI/
COPY FCG_Games.Application/ FCG_Games.Application/
COPY FCG_Games.Domain/ FCG_Games.Domain/
COPY FCG_Games.Infrastructure/ FCG_Games.Infrastructure/
COPY FCG_Games.WebAPI/FCG_Games.Application.sln FCG_Games.Application.sln

# Restaurar depend�ncias e compilar
WORKDIR /src/FCG.WebAPI
RUN dotnet restore FCG_Games.WebAPI.csproj
RUN dotnet publish FCG_Games.WebAPI.csproj -c Release -o /app

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "FCG_Games.WebAPI.dll"]
