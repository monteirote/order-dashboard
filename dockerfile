# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copia o arquivo de solução (.sln) e os .csproj de suas pastas
COPY *.sln .
COPY OrderDashboard/*.csproj ./OrderDashboard/
# (Se você tiver MAIS projetos, ex: OrderDashboard.Data, adicione uma linha para ele)
# COPY OrderDashboard.Data/*.csproj ./OrderDashboard.Data/

# 2. Restaura as dependências usando o .sln
RUN dotnet restore

# 3. Copia TODO o resto do código-fonte
COPY . .

# 4. Publica o projeto específico, referenciando seu .csproj
RUN dotnet publish "OrderDashboard/OrderDashboard.csproj" -c Release -o /app/publish

# Estágio 2: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# 5. Aponta para a DLL correta (deve ser o nome do seu projeto)
ENTRYPOINT ["dotnet", "OrderDashboard.dll"]