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
COPY ["src/Services/NotificationService/NotificationService/Notification.API.csproj", "src/Services/NotificationService/NotificationService/"]
COPY ["src/Services/NotificationService/Notification.Application/Notification.Application.csproj", "src/Services/NotificationService/Notification.Application/"]
COPY ["src/Services/NotificationService/Notification.Domain/Notification.Domain.csproj", "src/Services/NotificationService/Notification.Domain/"]
COPY ["src/Services/NotificationService/Notification.Infrastructure/Notification.Infrastructure.csproj", "src/Services/NotificationService/Notification.Infrastructure/"]

RUN dotnet restore "src/Services/NotificationService/NotificationService/Notification.API.csproj"

COPY . .

WORKDIR "/src/src/Services/NotificationService/NotificationService/"
RUN dotnet publish "Notification.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build  /app/publish .
ENTRYPOINT ["dotnet", "Notification.API.dll"]