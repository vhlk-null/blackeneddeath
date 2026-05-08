FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update && apt-get install -y --no-install-recommends libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Services/Notifications/Notifications.API/Notifications.API.csproj", "Services/Notifications/Notifications.API/"]
COPY ["Services/Notifications/Notifications.Application/Notifications.Application.csproj", "Services/Notifications/Notifications.Application/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["Services/Notifications/Notifications.Domain/Notifications.Domain.csproj", "Services/Notifications/Notifications.Domain/"]
COPY ["Services/Notifications/Notifications.Infrastructure/Notifications.Infrastructure.csproj", "Services/Notifications/Notifications.Infrastructure/"]
RUN dotnet restore "./Services/Notifications/Notifications.API/Notifications.API.csproj"
COPY . .
WORKDIR "/src/Services/Notifications/Notifications.API"
RUN dotnet build "./Notifications.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Notifications.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.API.dll"]
