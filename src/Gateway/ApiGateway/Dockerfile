FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Копируем глобальные файлы настроек
COPY ["*.slnx", "*.sln", "./"]
COPY ["src/Directory.Build.props", "src/"]

# 2. КРИТИЧНО: Копируем контракты, так как гейтвей на них ссылается (для JWT моделей и т.д.)
COPY ["src/Contracts/Contracts/Contracts.csproj", "src/Contracts/Contracts/"]

# 3. Копируем сам проект гейтвея
COPY ["src/Gateway/ApiGateway/ApiGateway.csproj", "src/Gateway/ApiGateway/"]

RUN dotnet restore "src/Gateway/ApiGateway/ApiGateway.csproj"

COPY . .

WORKDIR "src/Gateway/ApiGateway/"
RUN dotnet publish "ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build  /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]