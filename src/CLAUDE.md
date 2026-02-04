# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 10.0 microservices-based application for managing a music archive (albums, bands, tracks, genres). The solution follows Clean Architecture principles with CQRS pattern and vertical slice architecture.

## Technology Stack

- **.NET 10.0** - Primary framework
- **PostgreSQL 16** - Database (via Npgsql and EF Core 10)
- **Entity Framework Core 10.0.2** - ORM with migrations
- **Redis 7** - Distributed caching (UserContent.API)
- **Mediator 3.0.1** (`martinothamar/Mediator`) - Source-generated CQRS implementation (replaces MediatR)
- **Carter 10.0.0** - Minimal API routing
- **FluentValidation 12.1.1** - Request validation
- **Mapster 7.4.0** - Object mapping (Archive.API uses explicit `MappingConfig`)
- **StackExchange.Redis** - Redis client for caching and cache invalidation
- **Docker & Docker Compose** - Containerization

## Solution Structure

### BuildingBlocks Project
Shared infrastructure library containing:
- **CQRS Abstractions**: `ICommand<TResponse>`, `IQuery<TResponse>`, `ICommandHandler<,>`, `IQueryHandler<,>` (wrapping `Mediator` interfaces)
- **Repository Pattern**: Generic `IRepository<TContext>` with `BaseGenericRepository<T>` implementation
- **Pipeline Behaviors**: `LoggingBehavior<,>`, `ValidationBehavior<,>`, `UnitOfWorkBehavior<,>` (using `Mediator.IPipelineBehavior`)
- **Pagination**: `PagedQuery<T>`, `PagedResult<T>`, and `QueryableExtensions.ToPagedResultAsync()` in `Extentions/`
- **Global Exception Handling**: `GlobalExceptionHandler` in `Exceptions/`

### Archive.API Service
Main service for music archive data following vertical slice architecture:
- **Feature Folders**: Each feature (GetAlbums, CreateAlbum, etc.) contains its endpoint, handler, validators, DTOs in one folder
- **Endpoints**: Carter modules using minimal APIs (pattern: `{Feature}Endpoint.cs`)
- **Handlers**: Mediator handlers (pattern: `{Feature}Handler.cs` or `{Feature}QueryHandler.cs`/`{Feature}CommandHandler.cs`)
- **Data Layer**: EF Core context (`ArchiveContext`), repository implementation (`ArchiveRepository`)
- **DI Extensions**: `ServiceCollectionDIExtensions` with `InjectValidators()` and `InjectServices()` methods
- **Mapping Config**: `Mappings/MappingConfig.cs` — explicit Mapster type mapping registered in `Program.cs`
- **Validation Resources**: Localized validation messages via resource files in `Resources/ResourceFiles/`
- **Domain Models**: Located in `Models/` with join tables in `Models/JoinTables/`
- **Health Checks**: `/health` endpoint with PostgreSQL health check
- **Global Usings**: `GlobalUsing.cs` imports common namespaces project-wide

### UserContent.API Service
Service for user-specific content (favorites, profiles) following the same vertical slice architecture:
- **Feature Folders**: `UserContent/FavoriteAlbums/` (Add, Delete), `UserContent/FavoriteBands/` (Add, Delete), `UserContent/UserProfile/` (GetUserProfile)
- **Data Layer**: EF Core context (`UserContentContext`), repository implementations (`UserContentRepository`, `CachedUserContentRepository`)
- **Caching**: Redis-backed decorator pattern — `CachedUserContentRepository` wraps `UserContentRepository` (see Caching section below)
- **Models**: `FavoriteAlbum`, `FavoriteBand`, `UserProfileInfo` in `Models/`
- **Same patterns as Archive.API**: Carter endpoints, Mediator handlers, Mapster mapping
- **Pipeline**: LoggingBehavior + ValidationBehavior + UnitOfWorkBehavior
- **Database**: Separate PostgreSQL instance (`UserContentDB`), migrations and seed data configured
- **No Health Checks**: Unlike Archive.API, no `/health` endpoint configured

## Common Development Commands

### Database Operations

```bash
# Add a new migration (from the respective service directory)
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

# Run UserContent.API locally
cd Services/UserContent/UserContent.API
dotnet run

# Run with Docker Compose (from src directory)
docker-compose up --build

# Stop Docker Compose
docker-compose down
```

### Docker Environment

Archive.API:
- **HTTP**: localhost:6000 (mapped to container 8080)
- **HTTPS**: localhost:6001 (mapped to container 8081)

UserContent.API:
- **HTTP**: localhost:6010 (mapped to container 8080)
- **HTTPS**: localhost:6011 (mapped to container 8081)

PostgreSQL databases:
- **ArchiveDb**: localhost:5432 (in Docker: archivedb:5432) — credentials: postgres/postgres
- **UserContentDB**: localhost:5433 (in Docker: usercontentdb:5432) — credentials: postgres/postgres

Redis:
- **Redis**: localhost:6379 — used by UserContent.API for distributed caching

## Architecture Patterns

### CQRS Flow
1. **Carter Endpoint** receives HTTP request and maps to Command/Query
2. **Mediator** (source-generated) dispatches to appropriate handler through pipeline:
   - LoggingBehavior (logs requests with timing)
   - ValidationBehavior (FluentValidation)
   - UnitOfWorkBehavior (SaveChanges for commands only)
3. **Handler** executes business logic using Repository
4. **Response** mapped back to DTO and returned

### Repository Pattern
- Generic repository (`IRepository<TContext>`) injected into handlers
- Archive.API uses `ArchiveContext`, UserContent.API uses `UserContentContext`
- **Queries**: `GetByAsync<T>()`, `GetByWithIncludeAsync<T>()`, `GetWithIncludesAsync<T>()`, `Filter<T>()`, `FilterAsync<T>()`, `All<T>()`, `AllAsync<T>()`, `AllWithIncludeAsync<T>()`
- **Mutations**: `AddAsync<T>()`, `AddRangeAsync<T>()`, `Update<T>()`, `UpdateRange<T>()`, `Delete<T>()`, `DeleteRange<T>()`, `DeleteAsync<T>()`
- **Aggregates**: `CountAsync<T>()`
- UnitOfWorkBehavior automatically calls `SaveChangesAsync()` for commands

### Caching (UserContent.API)
- **Decorator pattern**: `CachedUserContentRepository` wraps `UserContentRepository`
- Registered manually in `Program.cs`: inner `UserContentRepository` + outer `CachedUserContentRepository` as `IRepository<UserContentContext>`
- Read operations (`GetByAsync`, `FilterAsync`, `GetWithIncludesAsync`) are cached for 30 minutes via `IDistributedCache` (Redis)
- Tracked queries (`asTracked = true`) bypass cache to preserve EF change tracking
- **Cache invalidation**: Mutations (`AddAsync`, `Update`, `Delete`) invalidate all cache keys for that entity type using Redis `KEYS` pattern scan (`UserContent:{TypeName}:*`)
- Cache keys: `UserContent:{TypeName}:{Operation}:{Hash}` where hash is derived from filter/include expressions

### Entity Relationships
- **Many-to-Many**: Explicit join table entities (e.g., `AlbumBand`, `AlbumGenre`, `AlbumTrack`)
- **One-to-Many**: StreamingLink has FK to Album
- Model configuration via extension methods in `ArchiveModelBuilderExtensions.cs`
- Seeding configured per entity in `Data/Seed/` folder

### Validation
- FluentValidation validators co-located with commands in feature folders
- Validators automatically registered via `AddValidatorsFromAssembly()`
- Validation messages use resource files from Archive.API: `ValidationMessages.{Property}`
- ValidationBehavior in Mediator pipeline throws validation exceptions automatically

### Database Initialization
- Both services use `DatabaseInitializerExtensions.InitializeDatabaseAsync()` extension method on `WebApplication`
- Only runs in Development environment: applies EF migrations then calls `DatabaseSeeder.SeedDatabaseAsync()`
- `DatabaseSeeder` (in `Extenstions/`) checks if data exists before seeding, uses transactions for atomicity

## Key Conventions

### File Organization
- **Vertical Slices**: Group by feature, not layer (e.g., `Albums/CreateAlbum/`)
- **Nested Features**: Related features can nest (e.g., `Albums/GetAlbumsBy/GetAlbumById/`)
- **Naming**: `{Feature}Endpoint.cs`, `{Feature}Handler.cs` or `{Feature}CommandHandler.cs`/`{Feature}QueryHandler.cs`
- **DTOs**: Request/Response records defined in endpoint files
- **Command/Query + Result**: Defined in handler files along with validators
- **Validators**: `{Feature}CommandValidator` class in handler file

### Code Patterns
- Use **records** for DTOs, Commands, Queries, Results
- Use **primary constructors** for dependency injection (e.g., `class Handler(IRepository<ArchiveContext> repo)`)
- Commands implement `ICommand<TResult>`, Queries implement `IQuery<TResult>`
- Handlers use `ICommandHandler<,>` or `IQueryHandler<,>`
- Mapster for object mapping: `source.Adapt<DestinationType>()`

### Connection Strings
- Configured via `appsettings.json` or environment variables
- Archive: `ConnectionStrings__ArchiveDb`
- UserContent: `ConnectionStrings__UserContentDB`
- Redis: `ConnectionStrings__Redis`

## Important Notes

- **No Test Projects**: The solution currently has no test projects
- **Global Exception Handling**: `GlobalExceptionHandler` (in BuildingBlocks) provides consistent error responses; services have their own exception types (e.g., `FavoriteAlbumNotFoundException`)
- **Primary Constructors**: C# 12 syntax used throughout for DI
- **Minimal APIs**: Carter framework registers all `ICarterModule` implementations automatically
