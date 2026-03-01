# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Services/UserContent/UserContent.API/UserContent.API.csproj", "Services/UserContent/UserContent.API/"]
COPY ["Services/UserContent/UserContent.Application/UserContent.Application.csproj", "Services/UserContent/UserContent.Application/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["Services/UserContent/UserContent.Domain/UserContent.Domain.csproj", "Services/UserContent/UserContent.Domain/"]
COPY ["Services/UserContent/UserContent.Infrastructure/UserContent.Infrastructure.csproj", "Services/UserContent/UserContent.Infrastructure/"]
RUN dotnet restore "./Services/UserContent/UserContent.API/UserContent.API.csproj"
COPY . .
WORKDIR "/src/Services/UserContent/UserContent.API"
RUN dotnet build "./UserContent.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserContent.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserContent.API.dll"]