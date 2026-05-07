FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Services/Activity/Activity.API/Activity.API.csproj", "Services/Activity/Activity.API/"]
COPY ["Services/Activity/Activity.Application/Activity.Application.csproj", "Services/Activity/Activity.Application/"]
COPY ["Services/Activity/Activity.Domain/Activity.Domain.csproj", "Services/Activity/Activity.Domain/"]
COPY ["Services/Activity/Activity.Infrastructure/Activity.Infrastructure.csproj", "Services/Activity/Activity.Infrastructure/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks.MongoDB/BuildingBlocks.MongoDB.csproj", "BuildingBlocks/BuildingBlocks.MongoDB/"]
RUN dotnet restore "./Services/Activity/Activity.API/Activity.API.csproj"
COPY . .
WORKDIR "/src/Services/Activity/Activity.API"
RUN dotnet build "./Activity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Activity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Activity.API.dll"]
