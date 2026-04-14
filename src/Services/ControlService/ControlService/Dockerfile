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
COPY ["src/Services/ControlService/ControlService/Control.API.csproj", "src/Services/ControlService/ControlService/"]
COPY ["src/Services/ControlService/ControlService.Application/Control.Application.csproj", "src/Services/ControlService/ControlService.Application/"]
COPY ["src/Services/ControlService/ControlService.Domain/Control.Domain.csproj", "src/Services/ControlService/ControlService.Domain/"]
COPY ["src/Services/ControlService/ControlService.Infrastructure/Control.Infrastructure.csproj", "src/Services/ControlService/ControlService.Infrastructure/"]

RUN dotnet restore "src/Services/ControlService/ControlService/Control.API.csproj"

COPY . .

WORKDIR "/src/src/Services/ControlService/ControlService"
RUN dotnet publish "Control.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build  /app/publish .
ENTRYPOINT ["dotnet", "Control.API.dll"]