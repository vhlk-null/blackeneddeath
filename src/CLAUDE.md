# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 10.0 microservices-based application for managing a music archive (albums, bands, tracks, genres). The solution follows Clean Architecture principles with CQRS pattern and vertical slice architecture.

## Technology Stack

- **.NET 10.0** - Primary framework
- **PostgreSQL 16** - Database (via Npgsql and EF Core 10)
- **Entity Framework Core 10.0.2** - ORM with migrations
- **MediatR 14.0.0** - CQRS implementation
- **Carter 10.0.0** - Minimal API routing
- **FluentValidation 12.1.1** - Request validation
- **Mapster 7.4.0** - Object mapping
- **Docker & Docker Compose** - Containerization

## Solution Structure

### BuildingBlocks Project
Shared infrastructure library containing:
- **CQRS Abstractions**: `ICommand<TResponse>`, `IQuery<TResponse>`, `ICommandHandler<,>`, `IQueryHandler<,>`
- **Repository Pattern**: Generic `IRepository<TContext>` with `BaseGenericRepository<T>` implementation
- **MediatR Behaviors**: `LoggingBehavior<,>`, `ValidationBehavior<,>` (from BuildingBlocks)
- **Unit of Work**: `UnitOfWorkBehavior<,>` - automatically saves changes for commands (skips queries)
- **Validation Resources**: Localized validation messages via resource files

### Archive.API Service
Main service following vertical slice architecture:
- **Feature Folders**: Each feature (GetAlbums, CreateAlbum, etc.) contains its endpoint, handler, validators, DTOs in one folder
- **Endpoints**: Carter modules using minimal APIs (pattern: `{Feature}Endpoint.cs`)
- **Handlers**: MediatR handlers (pattern: `{Feature}Handler.cs` or `{Feature}QueryHandler.cs`/`{Feature}CommandHandler.cs`)
- **Data Layer**: EF Core context (`ArchiveContext`), repository implementation (`ArchiveRepository`)
- **Domain Models**: Located in `Models/` with join tables in `Models/JoinTables/`
- **Global Usings**: `GlobalUsing.cs` imports common namespaces project-wide

## Common Development Commands

### Database Operations

```bash
# Add a new migration (from Archive.API directory)
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove
```

### Build and Run

```bash
# Build the solution
dotnet build blackened.death.slnx

# Run Archive.API locally
cd Services/Archive/Archive.API
dotnet run

# Run with Docker Compose (from src directory)
docker-compose up --build

# Stop Docker Compose
docker-compose down
```

### Docker Environment

The Archive.API runs on:
- **HTTP**: localhost:6000 (mapped to container 8080)
- **HTTPS**: localhost:6001 (mapped to container 8081)

PostgreSQL database:
- **Host**: localhost:5432 (in Docker: archivedb:5432)
- **Database**: ArchiveDb
- **Credentials**: postgres/postgres

### Testing Endpoints

```bash
# Health check
curl http://localhost:6000/health

# Get albums with pagination
curl http://localhost:6000/albums?PageNumber=1&PageSize=10
```

## Architecture Patterns

### CQRS Flow
1. **Carter Endpoint** receives HTTP request and maps to Command/Query
2. **MediatR** dispatches to appropriate handler through pipeline:
   - LoggingBehavior (logs requests)
   - ValidationBehavior (FluentValidation)
   - UnitOfWorkBehavior (SaveChanges for commands only)
3. **Handler** executes business logic using Repository
4. **Response** mapped back to DTO and returned

### Repository Pattern
- Generic repository (`IRepository<TContext>`) injected into handlers
- Context is `ArchiveContext` for all operations
- Use `Filter<T>()` or `FilterAsync<T>()` for queries with predicates
- Use `AddAsync<T>()`, `Update<T>()`, `Delete<T>()` for mutations
- UnitOfWorkBehavior automatically calls `SaveChangesAsync()` for commands

### Entity Relationships
- **Many-to-Many**: Explicit join table entities (e.g., `AlbumBand`, `AlbumGenre`, `AlbumTrack`)
- **One-to-Many**: StreamingLink has FK to Album
- Model configuration via extension methods in `ArchiveModelBuilderExtensions.cs`
- Seeding configured per entity in `Data/Seed/` folder

### Validation
- FluentValidation validators co-located with commands in feature folders
- Validators automatically registered via `AddValidatorsFromAssembly()`
- Validation messages use resource files from BuildingBlocks: `ValidationMessages.{Property}`
- ValidationBehavior in MediatR pipeline throws validation exceptions automatically

## Key Conventions

### File Organization
- **Vertical Slices**: Group by feature, not layer (e.g., `Albums/CreateAlbum/`)
- **Naming**: `{Feature}Endpoint.cs`, `{Feature}Handler.cs`, `{Feature}Command/Query.cs`
- **DTOs**: Defined as records in the same file as endpoints
- **Validators**: Defined in handler files as `{Feature}CommandValidator`

### Code Patterns
- Use **records** for DTOs, Commands, Queries, Results
- Use **primary constructors** for dependency injection (e.g., `class Handler(IRepository<ArchiveContext> repo)`)
- Commands implement `ICommand<TResult>`, Queries implement `IQuery<TResult>`
- Handlers use `ICommandHandler<,>` or `IQueryHandler<,>`
- Mapster for object mapping: `source.Adapt<DestinationType>()`

### Database Migrations
- Migrations automatically applied on startup in Development environment (see `Program.cs:48`)
- Seeding happens after migrations via `DatabaseSeeder.SeedDatabaseAsync()`
- Connection string configured via `appsettings.json` or environment variable `ConnectionStrings__ArchiveDb`

## Important Notes

- **Automatic Seeding**: Development environment auto-applies migrations and seeds data on startup
- **Health Checks**: `/health` endpoint configured with PostgreSQL health check
- **Global Exception Handling**: `GlobalExceptionHandler` provides consistent error responses
- **Primary Constructors**: Modern C# 12 syntax used throughout for DI
- **Minimal APIs**: Carter framework registers all `ICarterModule` implementations automatically
