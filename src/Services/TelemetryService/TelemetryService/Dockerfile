FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["*.sln", "./"]
COPY ["src/*.props", "src/"]
COPY ["src/*.targets", "src/"]

COPY ["src/Contracts/Contracts/Contracts.csproj", "src/Contracts/Contracts/"]
COPY ["src/Services/TelemetryService/TelemetryService/Telemetry.API.csproj", "src/Services/TelemetryService/TelemetryService/"]
COPY ["src/Services/TelemetryService/Telemetry.Application/Telemetry.Application.csproj", "src/Services/TelemetryService/Telemetry.Application/"]
COPY ["src/Services/TelemetryService/Telemetry.Domain/Telemetry.Domain.csproj", "src/Services/TelemetryService/Telemetry.Domain/"]
COPY ["src/Services/TelemetryService/Telemetry.Infrastructure/Telemetry.Infrastructure.csproj", "src/Services/TelemetryService/Telemetry.Infrastructure/"]

RUN dotnet restore "src/Services/TelemetryService/TelemetryService/Telemetry.API.csproj"

COPY . .

WORKDIR "src/Services/TelemetryService/TelemetryService"
RUN dotnet build "Telemetry.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Telemetry.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Telemetry.API.dll"]