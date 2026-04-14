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

COPY ["src/Services/IdentityService/IdentityService/IdentityService.API.csproj", "src/Services/IdentityService/IdentityService/"]
COPY ["src/Services/IdentityService/IdentityService.Application/IdentityService.Application.csproj", "src/Services/IdentityService/IdentityService.Application/"]
COPY ["src/Services/IdentityService/IdentityService.Domain/IdentityService.Domain.csproj", "src/Services/IdentityService/IdentityService.Domain/"]
COPY ["src/Services/IdentityService/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj", "src/Services/IdentityService/IdentityService.Infrastructure/"]

RUN dotnet restore "src/Services/IdentityService/IdentityService/IdentityService.API.csproj"

COPY . .

WORKDIR "/src/src/Services/IdentityService/IdentityService/"
RUN dotnet build "IdentityService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]