FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["*.slnx", "*.sln", "./"]
COPY ["src/Directory.Build.props", "src/"]

COPY ["src/BuildingBlocks/BuildingBlocks.Domain/BuildingBlocks.Domain.csproj", "src/BuildingBlocks/BuildingBlocks.Domain/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Application/BuildingBlocks.Application.csproj", "src/BuildingBlocks/BuildingBlocks.Application/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj", "src/BuildingBlocks/BuildingBlocks.Infrastructure/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Presentation/BuildingBlocks.Presentation.csproj", "src/BuildingBlocks/BuildingBlocks.Presentation/"]
COPY ["src/BuildingBlocks/BuildingBlocks.IntegrationEvents/BuildingBlocks.IntegrationEvents.csproj", "src/BuildingBlocks/BuildingBlocks.IntegrationEvents/"]
COPY ["src/BuildingBlocks/BuildingBlocks.GrpcContracts/BuildingBlocks.GrpcContracts.csproj", "src/BuildingBlocks/BuildingBlocks.GrpcContracts/"]

COPY ["src/Gateway/ApiGateway/ApiGateway.csproj", "src/Gateway/ApiGateway/"]

RUN dotnet restore "src/Gateway/ApiGateway/ApiGateway.csproj"

COPY . .

WORKDIR "/src/src/Gateway/ApiGateway/"
RUN dotnet publish "ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build  /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
