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

COPY ["src/Services/ControlService/ControlService/Control.API.csproj", "src/Services/ControlService/ControlService/"]
COPY ["src/Services/ControlService/ControlService.Application/Control.Application.csproj", "src/Services/ControlService/ControlService.Application/"]
COPY ["src/Services/ControlService/ControlService.Domain/Control.Domain.csproj", "src/Services/ControlService/ControlService.Domain/"]
COPY ["src/Services/ControlService/ControlService.Infrastructure/Control.Infrastructure.csproj", "src/Services/ControlService/ControlService.Infrastructure/"]

RUN dotnet restore "src/Services/ControlService/ControlService/Control.API.csproj"

COPY . .

WORKDIR "/src/src/Services/ControlService/ControlService"
RUN dotnet build "Control.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Control.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Control.API.dll"]