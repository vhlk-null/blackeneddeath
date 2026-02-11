# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

.NET 10.0 microservices application for managing a music archive (albums, bands, tracks, genres). Uses Clean Architecture with CQRS pattern and vertical slice architecture.

## Technology Stack

- **.NET 10.0** with Central Package Management (`Directory.Packages.props`)
- **PostgreSQL 16** via Npgsql + EF Core 10
- **Redis 7** — distributed caching (UserContent.API only)
- **Mediator 3.0.1** (`martinothamar/Mediator`) — source-generated CQRS (not MediatR)
- **Carter 10.0.0** — minimal API routing
- **FluentValidation 12.1.1** — request validation
- **Mapster 7.4.0** — object mapping (`source.Adapt<T>()`)
- **Scrutor** — decorator pattern for DI (used in UserContent.API for cached repository)
- **gRPC** — inter-service communication (Archive.API = server, UserContent.API = client)
- **Docker Compose** — containerization

## Common Development Commands

```bash
# Build the entire solution
dotnet build blackened.death.slnx

# Run a specific service locally
dotnet run --project Services/Archive/Archive.API
dotnet run --project Services/UserContent/UserContent.API

# Run with Docker Compose (from src directory)
docker-compose up --build

# EF Core migrations (run from the respective service directory)
dotnet ef migrations add MigrationName
dotnet ef database update
dotnet ef migrations remove
```

**No test projects exist yet.** Testing packages (xunit, FluentAssertions, Moq, Testcontainers) are pre-configured in `Directory.Packages.props` but commented out.

## Docker Environment

| Service          | Host Port | Container Port | Protocol  |
|------------------|-----------|----------------|-----------|
| Archive.API HTTP | 6000      | 8080           | HTTP/1+2  |
| Archive.API HTTPS| 6001      | 8081           | HTTP/1+2  |
| UserContent HTTP | 6010      | 8080           | HTTP      |
| UserContent HTTPS| 6011      | 8081           | HTTPS     |
| ArchiveDb        | 5432      | 5432           | PostgreSQL|
| UserContentDB    | 5433      | 5432           | PostgreSQL|
| Redis            | 6379      | 6379           | Redis     |

Archive.API serves both REST and gRPC on the same ports (HTTP/1+2 via Kestrel `EndpointDefaults`). No separate gRPC port.

Database credentials: `postgres/postgres` for both databases.

## Solution Structure

```
blackened.death.slnx          # XML-format solution file (.slnx)
Directory.Packages.props      # Central Package Management — ALL package versions here
BuildingBlocks/               # Shared library (CQRS, Repository, Behaviors, Exceptions)
Services/
  Archive/Archive.API/        # Music archive CRUD + gRPC server
  UserContent/UserContent.API/ # User favorites/profiles + gRPC client
```

### BuildingBlocks (shared library)

- **CQRS**: `ICommand<T>`, `IQuery<T>`, `ICommandHandler<,>`, `IQueryHandler<,>` — thin wrappers over `Mediator.IRequest`
- **Repository**: Generic `IRepository<TContext>` with `BaseGenericRepository<T>` implementation
- **Pipeline Behaviors**: `LoggingBehavior` → `ValidationBehavior` → `UnitOfWorkBehavior` (auto-saves for commands)
- **Pagination**: `PagedQuery<T>`, `PagedResult<T>`, `QueryableExtensions.ToPagedResultAsync()` in `Extentions/`
- **Exceptions**: `GlobalExceptionHandler`, `NotFoundException`, `BadRequestException`, `InternalServerException`

### Archive.API

Vertical slice feature folders: `Albums/CreateAlbum/`, `Bands/GetBands/`, etc. Each feature contains its endpoint, handler, validator, and DTOs in one folder. Nested features for related queries (e.g., `Albums/GetAlbumsBy/GetAlbumById/`).

Also serves as **gRPC server** — proto at `gRPC/Protos/archive.proto`, service implementation at `gRPC/Services/ArchiveService.cs`. Exposes `GetBandById` and `GetAlbumById` RPCs.

Uses `Mappings/MappingConfig.cs` for explicit Mapster type mappings (registered in `Program.cs`).

### UserContent.API

Same vertical slice pattern. Feature folders under `UserContent/` (FavoriteAlbums, FavoriteBands, UserProfile).

Acts as **gRPC client** — references Archive.API's proto file to validate albums/bands exist before adding to favorites. The `.csproj` links the proto with `GrpcServices="Client"`.

**Data model**: Many-to-many between `UserProfileInfo` ↔ `Album` (via `FavoriteAlbum` join table) and `UserProfileInfo` ↔ `Band` (via `FavoriteBand` join table). `Album` and `Band` are local entities mirroring Archive.API data. Join tables use composite PKs `(UserId, AlbumId/BandId)` with payload columns (AddedDate, UserRating, etc.). Bidirectional navigation properties on all entities.

**Caching**: Decorator pattern via Scrutor — `CachedUserContentRepository` wraps `UserContentRepository`. Reads cached 30 min via Redis. Mutations invalidate all cache keys for the entity type using Redis `KEYS` pattern scan.

## Architecture Patterns

### CQRS Flow
1. **Carter Endpoint** → maps HTTP request to Command/Query via `Adapt<T>()`
2. **Mediator pipeline**: LoggingBehavior → ValidationBehavior → UnitOfWorkBehavior
3. **Handler** executes logic via `IRepository<TContext>`
4. **UnitOfWorkBehavior** auto-calls `SaveChangesAsync()` for commands only

### Adding a New Package
All NuGet versions are centrally managed. To add a package:
1. Add `<PackageVersion>` to `Directory.Packages.props` (with version)
2. Add `<PackageReference>` to the service `.csproj` (without version)

### Database Initialization
Both services use `DatabaseInitializerExtensions.InitializeDatabaseAsync()` — applies EF migrations then seeds data via `DatabaseSeeder`. Only runs in Development environment. Seeding checks for existing data and uses transactions.

### Inter-Service Communication
UserContent.API → Archive.API via gRPC on the same port as REST (HTTP/2 content-type negotiation). When adding a favorite album/band, UserContent.API calls Archive.API's gRPC service to verify the entity exists, then maps the response to a local model.

## Key Conventions

- **Vertical slices**: Group by feature, not layer. Each feature folder = endpoint + handler + validator + DTOs
- **Records** for DTOs, Commands, Queries, Results
- **Primary constructors** for DI: `class Handler(IRepository<ArchiveContext> repo)`
- **Naming**: `{Feature}Endpoint.cs`, `{Feature}Handler.cs` (or `{Feature}CommandHandler.cs`/`{Feature}QueryHandler.cs`)
- **DTOs** (Request/Response records) defined in endpoint files; Command/Query + Result + Validator defined in handler files
- **GlobalUsing.cs** in each service project imports common namespaces
- **Service DI** organized as extension methods in `Extenstions/ServiceCollection*.cs`
- **Folder name typo**: `Extenstions/` (not `Extensions/`) is used in both service projects — follow this existing convention, do not rename
- **BuildingBlocks folder typo**: `Extentions/` (different typo) — also follow as-is
- **Validation messages**: Archive.API uses `.resx` resource files (`Resources/ResourceFiles/ValidationMessages`); UserContent.API uses inline strings
- **Exception types**: Each service defines its own (e.g., `AlbumNotFoundException`, `FavoriteAlbumNotFoundException`), inheriting from BuildingBlocks base exceptions
- **Join tables**: Use composite PKs (no surrogate `Id`), bidirectional nav props, configured via `HasOne().WithMany().HasForeignKey()` in `UserContentModelBuilderExtensions`
- **EF column naming**: All columns use explicit snake_case `HasColumnName()` — never rely on EF conventions
