#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 3000
EXPOSE 3001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY "Torc.sln" "Torc.sln"

COPY ["Services/BookLibrary/BookLibrary.API/BookLibrary.API.csproj", "Services/BookLibrary/BookLibrary.API/BookLibrary.API.csproj"]
COPY ["Services/BookLibrary/BookLibrary.Domain/BookLibrary.Domain.csproj", "Services/BookLibrary/BookLibrary.Domain/BookLibrary.Domain.csproj"]
COPY ["Services/BookLibrary/BookLibrary.Infrastructure/BookLibrary.Infrastructure.csproj", "Services/BookLibrary/BookLibrary.Infrastructure/BookLibrary.Infrastructure.csproj"]
COPY ["Services/BookLibrary/BookLibrary.SignalRHub/BookLibrary.SignalRHub.csproj", "Services/BookLibrary/BookLibrary.SignalRHub/BookLibrary.SignalRHub.csproj"]
COPY ["Services/BookLibrary/tests/BookLibrary.UnitTests/BookLibrary.UnitTests.csproj", "Services/BookLibrary/tests/BookLibrary.UnitTests/BookLibrary.UnitTests.csproj"]
COPY ["BuildingBlocks/EventBus/EventBus/EventBus.csproj", "BuildingBlocks/EventBus/EventBus/EventBus.csproj"]
COPY ["BuildingBlocks/EventBus/EventBus.AzureServiceBus/EventBus.AzureServiceBus.csproj", "BuildingBlocks/EventBus/EventBus.AzureServiceBus/EventBus.AzureServiceBus.csproj"]
COPY ["WebApps/booklibrary-spa/booklibrary-spa.esproj", "WebApps/booklibrary-spa/booklibrary-spa.esproj"]

COPY "docker-compose.dcproj" "docker-compose.dcproj"

RUN dotnet restore "Torc.sln"

COPY . .
WORKDIR /src/Services/BookLibrary/BookLibrary.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build as unittest
WORKDIR /src/Services/BookLibrary/BookLibrary.UnitTests

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookLibrary.API.dll"]