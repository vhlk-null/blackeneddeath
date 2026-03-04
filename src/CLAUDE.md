# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

.NET 10.0 microservices application for managing a music archive (albums, bands, tracks, genres). Both services use full N-Layer Domain-Driven Design (DDD): Domain → Application → Infrastructure → API.

## Technology Stack

- **.NET 10.0** with Central Package Management (`Directory.Packages.props`)
- **PostgreSQL 16** via Npgsql + EF Core 10
- **Redis 7** — distributed caching (UserContent.Infrastructure only)
- **Mediator 3.0.1** (`martinothamar/Mediator`) — source-generated CQRS (not MediatR); used in Library service only
- **Carter 10.0.0** — minimal API routing (Library.API only)
- **FluentValidation 12.1.1** — request validation
- **Mapster 7.4.0** — object mapping (`source.Adapt<T>()`)
- **Scrutor** — decorator pattern for DI (CachedUserContentRepository)
- **gRPC** — inter-service communication (Library.API = server, UserContent.Infrastructure = client)
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

# Run tests (by project — test projects are not in the default build configuration)
dotnet test Services/Library/Library.ApplicationTests/Library.ApplicationTests.csproj
dotnet test Services/Library/Library.InfrastructureTests/Library.InfrastructureTests.csproj
dotnet test Services/Library/Library.APITests/Library.APITests.csproj
dotnet test Services/UserContent/UserContent.ApplicationTests/UserContent.ApplicationTests.csproj
dotnet test Services/UserContent/UserContent.InfrastructureTests/UserContent.InfrastructureTests.csproj
dotnet test Services/UserContent/UserContent.APITests/UserContent.APITests.csproj

# EF Core migrations for Library service (DbContext is in Library.Infrastructure)
dotnet ef migrations add MigrationName --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API
dotnet ef database update --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API
dotnet ef migrations remove --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API

# EF Core migrations for UserContent service (DbContext is in UserContent.Infrastructure)
dotnet ef migrations add MigrationName --project Services/UserContent/UserContent.Infrastructure --startup-project Services/UserContent/UserContent.API
```

> **Note**: Test projects are excluded from the default solution build configuration. Run them individually with the paths above.

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
    Library.Application/               # CQRS handlers, validators, DTOs, exceptions, resource files
    Library.Infrastructure/            # EF Core, DbContext, interceptors, migrations, seeding
    Library.API/                       # Thin Carter endpoints, Program.cs, gRPC server
    Library.ApplicationTests/          # Unit tests (xunit + Moq + FluentAssertions)
    Library.InfrastructureTests/       # Integration tests for interceptors
    Library.APITests/                  # Integration tests (WebApplicationFactory)
  UserContent/
    UserContent.Domain/                # Domain models (UserProfileInfo, FavoriteAlbum, FavoriteBand)
    UserContent.Application/           # IUserContentService + UserContentService, DTOs, abstractions
    UserContent.Infrastructure/        # EF Core, Redis caching, gRPC client, migrations
    UserContent.API/                   # MVC controllers, Program.cs
    UserContent.ApplicationTests/
    UserContent.InfrastructureTests/
    UserContent.APITests/
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
- **Events**: Domain events under `Events/Album/`, `Events/Band/`, `Events/Genre/`
- **Exceptions**: `DomainException` (thrown from factory methods and invariant checks)

### Library.Application

CQRS handlers, validators, and application logic. Folder structure: `Services/{Feature}/Commands|Queries/{Name}/`.

- **`Data/ILibraryDbContext`** — DbContext abstraction used by handlers (not `LibraryContext` directly); enables unit testing with mocks
- **`Dtos/`** — shared DTOs (AlbumDto, BandDto, etc.) used by handlers and endpoints
- **`Exceptions/`** — application exceptions (AlbumNotFoundException, BandNotFoundException, etc.)
- **`Extensions/`** — domain-to-DTO extension methods (AlbumExtensions, BandExtensions, etc.)
- **`Services/{Feature}/Commands/{Name}/`** — command + handler pair (e.g., `CreateAlbumCommand.cs` + `CreateAlbumHandler.cs`)
- **`Services/{Feature}/Queries/{Name}/`** — query + handler pair
- **`Services/{Feature}/EventHandlers/`** — domain event handlers
- **`Resources/ResourceFiles/`** — `.resx` validation message files
- **Handler namespace pattern**: `Library.Application.Services.Albums.Commands.CreateAlbum` (note `.Services.` segment)

### Library.Infrastructure

- **`LibraryContext`** — EF Core DbContext, implements `ILibraryDbContext`; registers interceptors via `AddInterceptors()`
- **`DependencyInjection.cs`** — registers interceptors and DbContext:
  - `AuditableEntityInterceptor` and `DispatchDomainEventsInterceptor` as **Scoped**
  - `SlowQueryInterceptor` as **Singleton**
- **`Data/Configurations/`** — entity type configurations (Fluent API, snake_case column names)
- **`Data/Interceptors/`** — three EF Core interceptors (see section below)
- **`Data/Extensions/`** — `DatabaseInitializerExtensions`, `InitialData` seed class

### Library.API

Thin Carter minimal API endpoints only. DI is split across three extension files:
- `Library.Application/DependencyInjection.cs` — `AddApplicationServices()` (registers Mediator)
- `Library.Infrastructure/DependencyInjection.cs` — `AddInfrastructureServices()` (DbContext + interceptors)
- `Library.API/DependencyInjection.cs` — `AddApiServices()` (Carter, exception handler, etc.)

**Endpoint files** (`Endpoints/{Feature}/{FeatureAction}.cs`): contain only `Request`/`Response` records and the `ICarterModule` class. No handlers or validators — those live in Library.Application.

Also serves as **gRPC server** — proto at `gRPC/Protos/archive.proto`, service implementation at `gRPC/Services/ArchiveService.cs`. Exposes `GetBandById` and `GetAlbumById` RPCs.

Uses `Mappings/MappingConfig.cs` for explicit Mapster type mappings (registered in `Program.cs`).

### UserContent (N-Layer)

**UserContent.Domain**: plain domain models — `UserProfileInfo`, `FavoriteAlbum`, `FavoriteBand`.

**UserContent.Application**: service-pattern (not CQRS — no Mediator handlers):
- `Abstractions/IUserContentService` — primary interface used by controllers
- `Abstractions/IUserContentRepository` — repository interface
- `Abstractions/ILibraryService` — gRPC client abstraction
- `Services/UserContentService` — implementation
- `Dtos/`, `Exceptions/`, `Mappings/MappingConfig.cs`

**UserContent.Infrastructure**: EF Core, Redis caching, gRPC client:
- `Repositories/UserContentRepository` and `CachedUserContentRepository` (Scrutor decorator, 30-min Redis cache)
- `gRPC/LibraryGrpcService` — implements `ILibraryService` via gRPC client

**UserContent.API**: MVC controllers (not Carter), feature folders under `Endpoints/`:
- `FavoriteAlbumsController`, `FavoriteBandsController`, `UserProfileController`
- Controllers inject `IUserContentService` directly

**Data model**: Many-to-many between `UserProfileInfo` ↔ `Album` (via `FavoriteAlbum`) and `UserProfileInfo` ↔ `Band` (via `FavoriteBand`). Join tables use composite PKs `(UserId, AlbumId/BandId)`.

## DDD Patterns

### Aggregates

Aggregates extend `Aggregate<TId>` (which extends `Entity<TId>`). Key conventions:
- Private collection backing fields (`List<AlbumBand> _albumBands = []`), exposed as `IReadOnlyList<>`
- **Factory methods** (`Create(...)`) instead of public constructors — validate arguments and raise domain events
- **Behavioral methods** (`AddBand()`, `Update()`, etc.) that enforce invariants and raise domain events
- `AddDomainEvent(IDomainEvent)` called inside aggregate methods; events dispatched by `DispatchDomainEventsInterceptor` at `SaveChanges`

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

Events implement `IDomainEvent` (which extends `Mediator.INotification`). Aggregates accumulate events during method calls; `DispatchDomainEventsInterceptor` publishes all events via Mediator just before the transaction commits, then clears them. Event handlers live in `Library.Application/Services/{Feature}/EventHandlers/`.

## EF Core Interceptors

Three interceptors run in pipeline order during `SaveChanges`:

1. **`AuditableEntityInterceptor`** (Scoped, `SaveChangesInterceptor`) — sets `CreatedAt`, `CreatedBy`, `LastModifiedAt`, `LastModifiedBy` on all `IEntity` entries; also handles owned entity changes via `HasChangedOwnedEntities()`

2. **`DispatchDomainEventsInterceptor`** (Scoped, `SaveChangesInterceptor`) — extracts `IDomainEvent`s from all tracked `IAggregate` entries, clears them, then publishes each via `IMediator.Publish()`

3. **`SlowQueryInterceptor`** (Singleton, `DbCommandInterceptor`) — logs any DB command exceeding 500 ms, including the SQL and parameters

## Architecture Patterns

### CQRS Flow (Library service)
1. **Carter Endpoint** (`Library.API/Endpoints/`) → maps HTTP request to Command/Query via `Adapt<T>()`, sends via `ISender`
2. **Mediator pipeline**: `LoggingBehavior` → `ValidationBehavior` → `UnitOfWorkBehavior`
3. **Handler** in `Library.Application/Services/{Feature}/` executes logic via `ILibraryDbContext`
4. **DispatchDomainEventsInterceptor** fires on SaveChanges, publishing domain events to Application event handlers
5. **UnitOfWorkBehavior** auto-calls `SaveChangesAsync()` for commands only

### Adding a New Package
All NuGet versions are centrally managed:
1. Add `<PackageVersion>` to `Directory.Packages.props` (with version)
2. Add `<PackageReference>` to the service `.csproj` (without version)

### Database Initialization
Both services use `DatabaseInitializerExtensions.InitializeDatabaseAsync()` — applies EF migrations then seeds data. Only runs in Development.

### Inter-Service Communication
UserContent.Infrastructure → Library.API via gRPC. `LibraryGrpcService` implements `ILibraryService` and is injected into `UserContentService` to verify albums/bands exist before adding to favorites.

## Testing

- **Library.ApplicationTests**: xunit unit tests; mock `ILibraryDbContext` with Moq + `MockDbSetFactory`; test handlers directly
- **Library.APITests**: integration tests via `WebApplicationFactory<Program>` (`LibraryWebAppFactory`); mock `ISender` to test Carter endpoint routing and HTTP responses in isolation
- **Library.InfrastructureTests**: test EF Core interceptors
- **UserContent.*Tests**: equivalent test projects (some are placeholder stubs)

## Key Conventions

- **Records** for DTOs, Commands, Queries, Results, Request/Response
- **Primary constructors** for DI: `class Handler(ILibraryDbContext context)`
- **Naming in Library**: `{Name}Handler.cs` or `{Name}CommandHandler.cs` / `{Name}QueryHandler.cs` (inconsistent — check existing files)
- **Endpoint files**: contain only `Request`/`Response` records + `ICarterModule` class (no DTOs, handlers, or validators)
- **Application DTOs**: in `Library.Application/Dtos/` (shared across handlers and endpoints)
- **GlobalUsing.cs** in each project imports common namespaces
- **Service DI** organized as extension methods in `Extenstions/ServiceCollection*.cs` (API projects) or `DependencyInjection.cs`
- **Folder name typo**: `Extenstions/` (not `Extensions/`) in service API projects — follow as-is
- **BuildingBlocks folder typo**: `Extentions/` (different typo) — also follow as-is
- **Domain folder typo**: `Librrary.Domain/` (triple-r in folder name) — the csproj and namespace use correct spelling `Library.Domain`
- **Validation messages**: Library.Application uses `.resx` resource files (`Resources/ResourceFiles/ValidationMessages`); UserContent uses inline strings
- **Exception types**: Each service defines its own (e.g., `AlbumNotFoundException`) in the Application layer, inheriting from BuildingBlocks base exceptions
- **EF column naming**: All columns use explicit snake_case `HasColumnName()` — never rely on EF conventions
- **Join tables**: Composite PKs (no surrogate `Id`), bidirectional nav props, configured via `HasOne().WithMany().HasForeignKey()`
- **Mediator source generator**: Must be referenced in BOTH Application AND API csproj. CS0436 warnings about duplicate `Mediator`/`AssemblyReference` types are expected (not errors). Never mix `global using BuildingBlocks.CQRS;` with `global using Mediator;` — causes ambiguous type conflicts
