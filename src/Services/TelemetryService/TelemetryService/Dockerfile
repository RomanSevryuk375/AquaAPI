FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["*.slnx", "*.sln", "./"]
COPY ["src/*.props", "src/*.targets", "src/"]

COPY ["src/Contracts/Contracts/Contracts.csproj", "src/Contracts/Contracts/"]
COPY ["src/Services/TelemetryService/TelemetryService/Telemetry.API.csproj", "src/Services/TelemetryService/TelemetryService/"]
COPY ["src/Services/TelemetryService/Telemetry.Application/Telemetry.Application.csproj", "src/Services/TelemetryService/Telemetry.Application/"]
COPY ["src/Services/TelemetryService/Telemetry.Domain/Telemetry.Domain.csproj", "src/Services/TelemetryService/Telemetry.Domain/"]
COPY ["src/Services/TelemetryService/Telemetry.Infrastructure/Telemetry.Infrastructure.csproj", "src/Services/TelemetryService/Telemetry.Infrastructure/"]

RUN dotnet restore "src/Services/TelemetryService/TelemetryService/Telemetry.API.csproj"

COPY . .

WORKDIR "src/Services/TelemetryService/TelemetryService"
RUN dotnet publish "Telemetry.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build  /app/publish .
ENTRYPOINT ["dotnet", "Telemetry.API.dll"]