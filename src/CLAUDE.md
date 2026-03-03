# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

.NET 10.0 microservices application for managing a music archive (albums, bands, tracks, genres). Uses Domain-Driven Design (DDD) with CQRS, vertical slice endpoints, and a layered architecture inside the Library service.

## Technology Stack

- **.NET 10.0** with Central Package Management (`Directory.Packages.props`)
- **PostgreSQL 16** via Npgsql + EF Core 10
- **Redis 7** — distributed caching (UserContent.API only)
- **Mediator 3.0.1** (`martinothamar/Mediator`) — source-generated CQRS (not MediatR)
- **Carter 10.0.0** — minimal API routing
- **FluentValidation 12.1.1** — request validation
- **Mapster 7.4.0** — object mapping (`source.Adapt<T>()`)
- **Scrutor** — decorator pattern for DI (used in UserContent.API for cached repository)
- **gRPC** — inter-service communication (Library.API = server, UserContent.API = client)
- **Docker Compose** — containerization

## Common Development Commands

```bash
# Build the entire solution
dotnet build blackened.death.slnx

# Run services locally
dotnet run --project Services/Library/Library.API
dotnet run --project Services/UserContent/UserContent.API

# Run with Docker Compose (from src directory)
docker-compose up --build

# EF Core migrations for Library service (DbContext is in Library.Infrastructure)
dotnet ef migrations add MigrationName --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API
dotnet ef database update --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API
dotnet ef migrations remove --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API

# EF Core migrations for UserContent service
dotnet ef migrations add MigrationName --project Services/UserContent/UserContent.API
```

**No test projects exist yet.** Testing packages (xunit, FluentAssertions, Moq, Testcontainers) are pre-configured in `Directory.Packages.props` but commented out.

## Docker Environment

| Service            | Host Port | Container Port | Protocol  |
|--------------------|-----------|----------------|-----------|
| Library.API HTTP   | 6000      | 8080           | HTTP/1+2  |
| Library.API HTTPS  | 6001      | 8081           | HTTP/1+2  |
| UserContent HTTP   | 6010      | 8080           | HTTP      |
| UserContent HTTPS  | 6011      | 8081           | HTTPS     |
| librarydb          | 5432      | 5432           | PostgreSQL|
| usercontentdb      | 5433      | 5432           | PostgreSQL|
| Redis              | 6379      | 6379           | Redis     |

Library.API serves both REST and gRPC on the same ports (HTTP/1+2 via Kestrel `EndpointDefaults`). No separate gRPC port.

Database credentials: `postgres/postgres` for both databases.

## Solution Structure

```
blackened.death.slnx                   # XML-format solution file (.slnx)
Directory.Packages.props               # Central Package Management — ALL versions here
BuildingBlocks/                        # Shared library (CQRS, Repository, Behaviors, Exceptions)
Services/
  Library/
    Librrary.Domain/                   # ⚠️ Folder typo (triple-r); csproj/namespace: Library.Domain
    Library.Application/               # Mediator registration, DI extension
    Library.Infrastructure/            # EF Core, DbContext, interceptors, migrations, seeding
    Library.API/                       # Carter endpoints, Program.cs, gRPC server
  UserContent/
    UserContent.API/                   # User favorites/profiles + gRPC client
```

### BuildingBlocks (shared library)

- **CQRS**: `ICommand<T>`, `IQuery<T>`, `ICommandHandler<,>`, `IQueryHandler<,>` — thin wrappers over `Mediator.IRequest`
- **Repository**: Generic `IRepository<TContext>` with `BaseGenericRepository<T>` implementation
- **Pipeline Behaviors**: `LoggingBehavior` → `ValidationBehavior` → `UnitOfWorkBehavior` (auto-saves for commands)
- **Pagination**: `PagedQuery<T>`, `PagedResult<T>`, `QueryableExtensions.ToPagedResultAsync()` in `Extentions/`
- **Exceptions**: `GlobalExceptionHandler`, `NotFoundException`, `BadRequestException`, `InternalServerException`

### Librrary.Domain

Pure domain layer — no framework dependencies.

- **Abstractions**: `IEntity` (audit fields), `IAggregate` / `IAggregate<T>` (domain events list + clear), `Entity<T>` (base class), `IDomainEvent`
- **Models**: `Album`, `Band`, `Track`, `Genre`, `Country`, `StreamingLink` as aggregate roots; join tables under `Models/JoinTables/`
- **ValueObjects**: `AlbumRelease`, `BandActivity`, `LabelInfo`; strongly-typed IDs under `ValueObjects/Ids/` (`AlbumId`, `BandId`, etc.) extending `EntityId<TValue>`
- **Events**: Domain events under `Events/Album/` and `Events/Band/` (e.g., `AlbumCreatedEvent`, `BandUpdatedEvent`)
- **Exceptions**: `DomainException` (thrown from factory methods and invariant checks)

### Library.Infrastructure

- **`LibraryContext`** — EF Core DbContext; registers interceptors via `AddInterceptors()`
- **`DependencyInjection.cs`** — registers interceptors and DbContext:
  - `AuditableEntityInterceptor` and `DispatchDomainEventsInterceptor` as **Scoped**
  - `SlowQueryInterceptor` as **Singleton**
- **`Data/Configurations/`** — entity type configurations (Fluent API, snake_case column names)
- **`Data/Interceptors/`** — three EF Core interceptors (see section below)
- **`Data/Migrations/`** — EF Core migrations
- **`Data/Extensions/`** — `DatabaseInitializerExtensions`, `InitialData` seed class

### Library.API

Vertical slice feature folders: `Albums/CreateAlbum/`, `Bands/GetBands/`, etc. DI is split across three extension files:
- `Library.Application/DependencyInjection.cs` — `AddApplicationServices()` (registers Mediator)
- `Library.Infrastructure/DependencyInjection.cs` — `AddInfrastructureServices()` (DbContext + interceptors)
- `Library.API/DependencyInjection.cs` — `AddApiServices()` (Carter, exception handler, etc.)

Also serves as **gRPC server** — proto at `gRPC/Protos/archive.proto`, service implementation at `gRPC/Services/ArchiveService.cs`. Exposes `GetBandById` and `GetAlbumById` RPCs.

Uses `Mappings/MappingConfig.cs` for explicit Mapster type mappings (registered in `Program.cs`).

### UserContent.API

Same vertical slice pattern. Feature folders under `UserContent/` (FavoriteAlbums, FavoriteBands, UserProfile).

Acts as **gRPC client** — references Library.API's proto file to validate albums/bands exist before adding to favorites. The `.csproj` links the proto with `GrpcServices="Client"`.

**Data model**: Many-to-many between `UserProfileInfo` ↔ `Album` (via `FavoriteAlbum` join table) and `UserProfileInfo` ↔ `Band` (via `FavoriteBand` join table). Join tables use composite PKs `(UserId, AlbumId/BandId)` with payload columns (AddedDate, UserRating, etc.). Bidirectional navigation properties on all entities.

**Caching**: Decorator pattern via Scrutor — `CachedUserContentRepository` wraps `UserContentRepository`. Reads cached 30 min via Redis. Mutations invalidate all cache keys for the entity type using Redis `KEYS` pattern scan.

## DDD Patterns

### Aggregates

Aggregates extend `Aggregate<TId>` (which extends `Entity<TId>`). Key conventions:
- Private collection backing fields (`List<AlbumBand> _albumBands = []`), exposed as `IReadOnlyList<>`
- **Factory methods** (`Create(...)`) instead of public constructors — validate arguments and raise domain events
- **Behavioral methods** (`AddBand()`, `Update()`, etc.) that enforce invariants and raise domain events
- `AddDomainEvent(IDomainEvent)` called inside aggregate methods; events are dispatched by `DispatchDomainEventsInterceptor` at `SaveChanges`

### Value Objects & Strongly-Typed IDs

All entity IDs are strongly typed, extending `EntityId<TValue>`:
```csharp
public record AlbumId : EntityId<Guid>
{
    public static AlbumId Of(Guid value) =>
        value == Guid.Empty ? throw new DomainException(...) : Of<AlbumId>(value);
}
```
Use `AlbumId.Of(guid)` — never pass raw `Guid` where an `AlbumId` is expected.

### Domain Events

Events implement `IDomainEvent` (which extends `Mediator.INotification`). Aggregates accumulate events during method calls; `DispatchDomainEventsInterceptor` publishes all events via Mediator just before the transaction commits, then clears them.

## EF Core Interceptors

Three interceptors run in pipeline order during `SaveChanges`:

1. **`AuditableEntityInterceptor`** (Scoped, `SaveChangesInterceptor`) — sets `CreatedAt`, `CreatedBy`, `LastModifiedAt`, `LastModifiedBy` on all `IEntity` entries; also handles owned entity changes via `HasChangedOwnedEntities()`

2. **`DispatchDomainEventsInterceptor`** (Scoped, `SaveChangesInterceptor`) — extracts `IDomainEvent`s from all tracked `IAggregate` entries, clears them, then publishes each via `IMediator.Publish()`

3. **`SlowQueryInterceptor`** (Singleton, `DbCommandInterceptor`) — logs any DB command exceeding 500 ms, including the SQL and parameters

## Architecture Patterns

### CQRS Flow (Library.API)
1. **Carter Endpoint** → maps HTTP request to Command/Query via `Adapt<T>()`
2. **Mediator pipeline**: LoggingBehavior → ValidationBehavior → UnitOfWorkBehavior
3. **Handler** executes logic, calling aggregate factory/behavioral methods
4. **DispatchDomainEventsInterceptor** fires on SaveChanges, publishing domain events
5. **UnitOfWorkBehavior** auto-calls `SaveChangesAsync()` for commands only

### Adding a New Package
All NuGet versions are centrally managed:
1. Add `<PackageVersion>` to `Directory.Packages.props` (with version)
2. Add `<PackageReference>` to the service `.csproj` (without version)

### Database Initialization
Both services use `DatabaseInitializerExtensions.InitializeDatabaseAsync()` — applies EF migrations then seeds data. Only runs in Development. Seeding checks for existing data and uses transactions.

### Inter-Service Communication
UserContent.API → Library.API via gRPC on the same port as REST (HTTP/2 content-type negotiation). When adding a favorite album/band, UserContent.API calls Library.API's gRPC service to verify the entity exists, then maps the response to a local model.

## Key Conventions

- **Vertical slices in API layer**: Each feature folder = endpoint + handler + validator + DTOs
- **Records** for DTOs, Commands, Queries, Results
- **Primary constructors** for DI: `class Handler(IRepository<LibraryContext> repo)`
- **Naming**: `{Feature}Endpoint.cs`, `{Feature}Handler.cs` (or `{Feature}CommandHandler.cs`/`{Feature}QueryHandler.cs`)
- **DTOs** (Request/Response records) defined in endpoint files; Command/Query + Result + Validator defined in handler files
- **GlobalUsing.cs** in each project imports common namespaces
- **Service DI** organized as extension methods in `Extenstions/ServiceCollection*.cs`
- **Folder name typo**: `Extenstions/` (not `Extensions/`) in both service API projects — follow as-is
- **BuildingBlocks folder typo**: `Extentions/` (different typo) — also follow as-is
- **Domain folder typo**: `Librrary.Domain/` (triple-r in folder name) — the csproj and namespace use correct spelling `Library.Domain`
- **Validation messages**: Library.API uses `.resx` resource files (`Resources/ResourceFiles/ValidationMessages`); UserContent.API uses inline strings
- **Exception types**: Each service defines its own (e.g., `AlbumNotFoundException`), inheriting from BuildingBlocks base exceptions
- **EF column naming**: All columns use explicit snake_case `HasColumnName()` — never rely on EF conventions
- **Join tables**: Composite PKs (no surrogate `Id`), bidirectional nav props, configured via `HasOne().WithMany().HasForeignKey()`
