FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 3002
EXPOSE 3003

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
WORKDIR /src/Services/BookLibrary/BookLibrary.SignalRHub
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookLibrary.SignalRHub.dll"]

#COPY . .
#WORKDIR "/src/Services/BookLibrary/BookLibrary.SignalRHub"
#RUN dotnet build "./BookLibrary.SignalRHub.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./BookLibrary.SignalRHub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "BookLibrary.SignalRHub.dll"]