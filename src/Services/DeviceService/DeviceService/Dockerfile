FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
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
COPY ["src/Services/DeviceService/DeviceService/Device.API.csproj", "src/Services/DeviceService/DeviceService/"]
COPY ["src/Services/DeviceService/Device.Application/Device.Application.csproj", "src/Services/DeviceService/Device.Application/"]
COPY ["src/Services/DeviceService/Device.Domain/Device.Domain.csproj", "src/Services/DeviceService/Device.Domain/"]
COPY ["src/Services/DeviceService/Device.Infrastrucrute/Device.Infrastructure.csproj", "src/Services/DeviceService/Device.Infrastrucrute/"]

RUN dotnet restore "src/Services/DeviceService/DeviceService/Device.API.csproj"

COPY . .

WORKDIR "src/Services/DeviceService/DeviceService"
RUN dotnet publish "Device.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build  /app/publish .
ENTRYPOINT ["dotnet", "Device.API.dll"]