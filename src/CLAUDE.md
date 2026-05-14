# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

.NET 10.0 microservices application for managing a music archive (albums, bands, tracks, genres). All services use N-Layer Domain-Driven Design (DDD): Domain → Application → Infrastructure → API.

## Technology Stack

- **.NET 10.0** with Central Package Management (`Directory.Packages.props`)
- **PostgreSQL 16** via Npgsql + EF Core 10
- **Redis 7** — distributed caching (UserContent.Infrastructure only)
- **Mediator 3.0.2** (`martinothamar/Mediator`) — source-generated CQRS (not MediatR); used in Library service only
- **Hangfire 1.8.23** + **Hangfire.PostgreSql** — background job scheduling (Library service); dashboard at `/hangfire`; `IAlbumReleaseJob` interface in Library.Application, `AlbumReleaseJob` implementation in Library.Infrastructure; jobs scheduled from `AlbumReleaseSchedulingHandler` domain event handler
- **Carter 10.0.0** — minimal API routing (Library.API only)
- **FluentValidation 12.1.1** — request validation
- **Mapster 7.4.0** — object mapping (`source.Adapt<T>()`)
- **Scrutor** — decorator pattern for DI (CachedUserContentRepository)
- **MassTransit** + **RabbitMQ** — async integration events (`BuildingBlocks.Messaging`); Library publishes, UserContent and Notifications consume
- **Microsoft.FeatureManagement** — feature flags in Library.Application (e.g., `FeatureFlags.AlbumFulfillment`)
- **gRPC** — inter-service communication (Library.API = server, UserContent.Application = client)
- **SSE (Server-Sent Events)** — real-time push from Notifications.API via `SseChannelService` (in `BuildingBlocks.Messaging/SSE/`)
- **MongoDB 7** — document store for Activity service (via MongoDB.Driver)
- **Meilisearch** — full-text search (Library service)
- **Serilog** + **Seq** — structured logging across all services
- **YARP** — API gateway (`ApiGateways/YarpApiGateway`)
- **Docker Compose** — containerization

## Common Development Commands

```bash
# Build the entire solution
dotnet build blackened.death.slnx

# Run services locally
dotnet run --project Services/Library/Library.API
dotnet run --project Services/UserContent/UserContent.API
dotnet run --project Services/Notifications/Notifications.API
dotnet run --project Services/Activity/Activity.API

# Run with Docker Compose (from src directory)
docker-compose up --build

# Run tests (by project — test projects are not in the default build configuration)
dotnet test Services/Library/Library.ApplicationTests/Library.ApplicationTests.csproj
dotnet test Services/Library/Library.InfrastructureTests/Library.InfrastructureTests.csproj
dotnet test Services/Library/Library.APITests/Library.APITests.csproj
dotnet test Services/UserContent/UserContent.ApplicationTests/UserContent.ApplicationTests.csproj
dotnet test Services/UserContent/UserContent.InfrastructureTests/UserContent.InfrastructureTests.csproj
dotnet test Services/UserContent/UserContent.APITests/UserContent.APITests.csproj
dotnet test Services/Activity/Activity.ApplicationTests/Activity.ApplicationTests.csproj

# EF Core migrations for Library service (DbContext is in Library.Infrastructure)
dotnet ef migrations add MigrationName --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API
dotnet ef database update --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API
dotnet ef migrations remove --project Services/Library/Library.Infrastructure --startup-project Services/Library/Library.API

# EF Core migrations for UserContent service (DbContext is in UserContent.Infrastructure)
dotnet ef migrations add MigrationName --project Services/UserContent/UserContent.Infrastructure --startup-project Services/UserContent/UserContent.API

# EF Core migrations for Notifications service (DbContext is in Notifications.Infrastructure)
dotnet ef migrations add MigrationName --project Services/Notifications/Notifications.Infrastructure --startup-project Services/Notifications/Notifications.API
```

> **Note**: Test projects are excluded from the default solution build configuration. Run them individually with the paths above.

## Docker Environment

| Service              | Host Port | Container Port | Protocol   |
|----------------------|-----------|----------------|------------|
| Library.API HTTP     | 6080      | 8080           | HTTP/1+2   |
| Library.API HTTPS    | 6081      | 8081           | HTTP/1+2   |
| UserContent HTTP     | 6010      | 8080           | HTTP       |
| UserContent HTTPS    | 6011      | 8081           | HTTPS      |
| Notifications HTTP   | 6040      | 8080           | HTTP       |
| Notifications HTTPS  | 6041      | 8081           | HTTPS      |
| Activity HTTP        | 6060      | 8080           | HTTP       |
| Activity HTTPS       | 6061      | 8081           | HTTPS      |
| Identity HTTP        | 6020      | 8080           | HTTP       |
| Identity HTTPS       | 6021      | 8081           | HTTPS      |
| YARP Gateway HTTP    | 6050      | 8080           | HTTP       |
| YARP Gateway HTTPS   | 6051      | 8081           | HTTPS      |
| Angular UI           | 4200      | 80             | HTTP       |
| librarydb            | 5434      | 5432           | PostgreSQL |
| usercontentdb        | 5435      | 5432           | PostgreSQL |
| identitydb           | 5436      | 5432           | PostgreSQL |
| notificationsdb      | 5437      | 5432           | PostgreSQL |
| activitydb (MongoDB) | 27017     | 27017          | MongoDB    |
| Redis                | 6379      | 6379           | Redis      |
| RabbitMQ AMQP        | 5672      | 5672           | AMQP       |
| RabbitMQ UI          | 15672     | 15672          | HTTP       |
| Seq UI               | 8082      | 80             | HTTP       |
| Seq ingest           | 5341      | 5341           | HTTP       |
| Meilisearch          | 7700      | 7700           | HTTP       |
| Mongo Express        | 8083      | 8081           | HTTP       |

Library.API serves both REST and gRPC on the same ports (HTTP/1+2 via Kestrel `EndpointDefaults`). No separate gRPC port.

Database credentials: `postgres/postgres` for all PostgreSQL databases. MongoDB: `root/root`. Meilisearch master key: `BlackEndDeathMasterKey123!`.

## Solution Structure

```
blackened.death.slnx                   # XML-format solution file (.slnx)
Directory.Packages.props               # Central Package Management — ALL versions here
BuildingBlocks/
  BuildingBlocks/                      # CQRS, Repository, Behaviors, Exceptions
  BuildingBlocks.Messaging/            # Integration events + MassTransit RabbitMQ setup + SseChannelService
ApiGateways/
  YarpApiGateway/                      # YARP reverse proxy — routes /library, /usercontent, /notifications, /identity
  YarpApiGateway.Tests/
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
  Notifications/
    Notifications.Domain/              # Notification, Subscription plain models (no DDD aggregates)
    Notifications.Application/         # INotificationService, NotificationService, AlbumCreatedConsumer
    Notifications.Infrastructure/      # EF Core (PostgreSQL), NotificationsContext, migrations
    Notifications.API/                 # MVC controllers: NotificationsController, SubscriptionsController
  Activity/
    Activity.Domain/                   # UserActivity model, IActivityRepository
    Activity.Application/              # IActivityService, ActivityService, FavoriteAlbumAddedConsumer
    Activity.Infrastructure/           # MongoDB repository, MongoIndexInitializer
    Activity.API/                      # MVC controller: ActivityController
    Activity.ApplicationTests/
  Identity/
    IdentityServer/                    # Duende IdentityServer (OpenID Connect / OAuth2)
```

### BuildingBlocks

**BuildingBlocks** (core shared library):
- **CQRS**: `ICommand<T>`, `IQuery<T>`, `ICommandHandler<,>`, `IQueryHandler<,>` — thin wrappers over `Mediator.IRequest`
- **Repository**: Generic `IRepository<TContext>` with `BaseGenericRepository<T>` implementation
- **Pipeline Behaviors**: `LoggingBehavior` → `ValidationBehavior` (⚠️ `UnitOfWorkBehavior` is currently **commented out** in Library.Application DI — handlers call `SaveChangesAsync` explicitly)
- **Pagination**: `PagedQuery<T>`, `PagedResult<T>`, `QueryableExtensions.ToPagedResultAsync()` in `Extentions/`
- **Exceptions**: `GlobalExceptionHandler`, `NotFoundException`, `BadRequestException`, `InternalServerException`

**BuildingBlocks.Messaging** (integration events + messaging):
- `IntegrationEvent` base class (Id, OccuredOn, EventType)
- Integration event types under `Events/Albums/` and `Events/Bands/` (Created/Updated/Removed variants)
- `AddMessageBroker(IConfiguration, string clientName, params Assembly[] consumerAssemblies)` — registers MassTransit with RabbitMQ; pass consumer assemblies to auto-register consumers. Config keys: `MessageBroker:Host`, `MessageBroker:Username`, `MessageBroker:Password`
- **`SSE/SseChannelService`** — Singleton; manages per-user `Channel<string>` lists for SSE push. `Subscribe(userId)` returns a `ChannelReader<string>`; `PublishAsync(userId, json)` broadcasts to all open connections; `Unsubscribe()` completes and removes the channel.

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
- **`Services/{Feature}/EventHandlers/Domain/`** — domain event handlers that publish integration events via `IPublishEndpoint`; guarded by feature flags
- **`Constants/FeatureFlags.cs`** — feature flag name constants (e.g., `FeatureFlags.AlbumFulfillment`); configured in `appsettings.json` under `FeatureManagement`
- **`Resources/ResourceFiles/`** — `.resx` validation message files
- **Handler namespace pattern**: `Library.Application.Services.Albums.Commands.CreateAlbum` (note `.Services.` segment)

### Library.Infrastructure

- **`LibraryContext`** — EF Core DbContext, implements `ILibraryDbContext`; registers interceptors via `AddInterceptors()`
- **`DependencyInjection.cs`** — registers interceptors, DbContext, and both `ILibraryDbContext` and `IRepository<LibraryContext>` (both registered and pointing to the same `LibraryContext`)
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
- `Abstractions/ILibraryService` — gRPC client abstraction (⚠️ `IUserContentRepository` was removed; service uses `IRepository<UserContentContext>` from BuildingBlocks directly)
- `Services/UserContentService` — injects `IRepository<UserContentContext>` and `ILibraryService`
- `Services/gRPC/LibraryGrpcService` — implements `ILibraryService` via gRPC (moved here from Infrastructure)
- `Consumers/` — MassTransit consumers: `AlbumRemovedConsumer`, `AlbumUpdatedConsumer` (sync local Album data from Library events)
- `Dtos/`, `Exceptions/`, `Mappings/MappingConfig.cs`
- `DependencyInjection.cs` takes `IConfiguration` (needs gRPC URL + message broker settings); calls `AddMessageBroker()` with its own assembly to register consumers

**UserContent.Infrastructure**: EF Core, Redis caching:
- `Repositories/UserContentRepository` and `CachedUserContentRepository` (Scrutor decorator of `IRepository<UserContentContext>`, 30-min Redis cache)
- Caches `GetWithIncludesAsync<UserProfileInfo>` by userId; invalidates on `AddAsync`/`Delete` of entities with `UserId`
- gRPC client registration and `LibraryGrpcService` have moved to **UserContent.Application**

**UserContent.API**: MVC controllers (not Carter), feature folders under `Endpoints/`:
- `FavoriteAlbumsController`, `FavoriteBandsController`, `UserProfileController`
- Controllers inject `IUserContentService` directly

**Data model**: Many-to-many between `UserProfileInfo` ↔ `Album` (via `FavoriteAlbum`) and `UserProfileInfo` ↔ `Band` (via `FavoriteBand`). Join tables use composite PKs `(UserId, AlbumId/BandId)`.

### Notifications (N-Layer, no CQRS)

Service-pattern (not CQRS). Responsible for user subscriptions to bands/albums and real-time push via SSE.

**Notifications.Domain**: plain models (no aggregates, no domain events):
- `Notification` — `(Id, UserId, Title, Message, Type, ResourceId, IsRead, CreatedAt)`; `MarkAsRead()` method; factory `Create(...)`
- `Subscription` — `(Id, UserId, ResourceType, ResourceId, ResourceName, ResourceSlug, CreatedAt)`; factory `Create(...)`

**Notifications.Application**:
- `INotificationService` / `NotificationService` — CRUD over `Notification` and `Subscription` via `IRepository<NotificationsContext>`
- `AlbumCreatedConsumer` — consumes `AlbumCreatedIntegrationEvent`; finds subscriptions for the album's bands; creates `Notification` rows; pushes JSON-serialized `NotificationDto` via `SseChannelService.PublishAsync()`
- `DependencyInjection.cs` — calls `AddMessageBroker(configuration, "notifications", Assembly.GetExecutingAssembly())`; registers `SseChannelService` as Singleton

**Notifications.Infrastructure**: EF Core (PostgreSQL), `NotificationsContext`, `NotificationsRepository`, migrations.

**Notifications.API**: MVC controllers (not Carter), auth via custom `GatewayHeaderAuthenticationHandler` (reads user id injected by YARP gateway):
- `NotificationsController` (`GET /`, `PATCH /{id}/read`, `PATCH /read-all`, `GET /stream`) — SSE endpoint streams from `SseChannelService`
- `SubscriptionsController` (`GET /subscriptions`, `GET /subscriptions/{type}/{id}`, `POST /subscriptions/{type}/{id}`, `DELETE /subscriptions/{type}/{id}`)
- `ExtractUserIdFilter` — action filter that reads `UserId` from `HttpContext.Items` (populated by auth handler) into a typed property

**SSE endpoint** (`GET /stream`): sets `Content-Type: text/event-stream`, subscribes via `SseChannelService.Subscribe(userId)`, streams `data: {json}\n\n` lines, unsubscribes on cancellation. YARP `notifications-stream-route` has no rate-limit policy and a 30-minute `ActivityTimeout`.

**EF migrations**: `dotnet ef migrations add Name --project Services/Notifications/Notifications.Infrastructure --startup-project Services/Notifications/Notifications.API`

### Activity (N-Layer, no CQRS)

Tracks user activity events. Uses **MongoDB** instead of PostgreSQL.

**Activity.Domain**: `UserActivity` document model; `IActivityRepository` interface.

**Activity.Application**:
- `IActivityService` / `ActivityService` — delegates to `IActivityRepository`
- `FavoriteAlbumAddedConsumer` — consumes integration events and records activity documents
- `DependencyInjection.cs` — registers service + message broker

**Activity.Infrastructure**: `ActivityRepository` (MongoDB.Driver); `MongoIndexInitializer` sets up indexes on startup.

**Activity.API**: MVC controller `ActivityController`; `ExtractUserIdFilter` (same pattern as Notifications).

Config keys: `ConnectionStrings:MongoDB`, `MongoDB:DatabaseName`.

### ApiGateways / YarpApiGateway

YARP reverse proxy — single entry point at `api.blackened-death.com`. Routes:
- `/library/{**}` → `library.api:8080` (rate-limited)
- `/usercontent/{**}` → `usercontent.api:8080` (rate-limited)
- `/notifications/stream` → `notifications.api:8080/stream` (no rate limit, 30-min timeout)
- `/notifications/{**}` → `notifications.api:8080` (rate-limited)
- `/identity/{**}`, `/.well-known/{**}`, `/connect/{**}` → `identity.server:8080`

Auth: validates JWT against IdentityServer (`IdentityServer:Authority`). CORS: `Cors:AllowedOrigins`. Rate limiter: fixed window, `RateLimiter:Window` / `RateLimiter:PermitLimit`.

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

### Domain Events → Integration Events

Events implement `IDomainEvent` (which extends `Mediator.INotification`). Aggregates accumulate events during method calls; `DispatchDomainEventsInterceptor` publishes all events via Mediator just before the transaction commits, then clears them.

Domain event handlers in `Library.Application/Services/{Feature}/EventHandlers/Domain/` translate domain events into integration events and publish them via MassTransit `IPublishEndpoint`, **gated by a feature flag** (e.g. `FeatureFlags.AlbumFulfillment`). This keeps the event publishing opt-in during development.

Album domain events also implement `IAlbumDomainEvent` (marker interface in `Library.Domain/Events/Album/`) to enable type-safe handler constraints.

## EF Core Interceptors

Three interceptors run in pipeline order during `SaveChanges`:

1. **`AuditableEntityInterceptor`** (Scoped, `SaveChangesInterceptor`) — sets `CreatedAt`, `CreatedBy`, `LastModifiedAt`, `LastModifiedBy` on all `IEntity` entries; also handles owned entity changes via `HasChangedOwnedEntities()`

2. **`DispatchDomainEventsInterceptor`** (Scoped, `SaveChangesInterceptor`) — extracts `IDomainEvent`s from all tracked `IAggregate` entries, clears them, then publishes each via `IMediator.Publish()`

3. **`SlowQueryInterceptor`** (Singleton, `DbCommandInterceptor`) — logs any DB command exceeding 500 ms, including the SQL and parameters

## Architecture Patterns

### CQRS Flow (Library service)
1. **Carter Endpoint** (`Library.API/Endpoints/`) → maps HTTP request to Command/Query via `Adapt<T>()`, sends via `ISender`
2. **Mediator pipeline**: `LoggingBehavior` → `ValidationBehavior` (UnitOfWorkBehavior currently disabled)
3. **Handler** in `Library.Application/Services/{Feature}/` executes logic via `ILibraryDbContext`, calls `SaveChangesAsync()` explicitly
4. **DispatchDomainEventsInterceptor** fires on SaveChanges, publishing domain events to domain event handlers
5. **Domain event handlers** publish integration events to RabbitMQ via `IPublishEndpoint` (feature-flag-gated)
6. **UserContent consumers** receive integration events and sync their local read model

### Messaging Flow
- Library domain events → domain event handlers → MassTransit `IPublishEndpoint` → RabbitMQ
- UserContent consumers (`UserContent.Application/Consumers/`) → update local `Album`/`Band` data
- Notifications consumer (`Notifications.Application/Consumers/AlbumCreatedConsumer`) → create `Notification` rows + push via SSE
- Activity consumers (`Activity.Application/Consumers/`) → record `UserActivity` documents in MongoDB
- Queue names use kebab-case formatter (MassTransit default)

### Adding a New Package
All NuGet versions are centrally managed:
1. Add `<PackageVersion>` to `Directory.Packages.props` (with version)
2. Add `<PackageReference>` to the service `.csproj` (without version)

### Database Initialization
Both services use `DatabaseInitializerExtensions.InitializeDatabaseAsync()` — applies EF migrations then seeds data. Only runs in Development.

### Inter-Service Communication
Two channels:
- **gRPC (sync)**: UserContent.Application → Library.API. `LibraryGrpcService` (in `UserContent.Application/Services/gRPC/`) implements `ILibraryService`, registered in `UserContent.Application/DependencyInjection.cs`. Used to fetch album/band data on-demand when not cached locally.
- **RabbitMQ (async)**: Library.API publishes integration events; UserContent.Application consumers keep local `Album`/`Band` copies up to date.

## Testing

- **Library.ApplicationTests**: xunit unit tests; mock `ILibraryDbContext` with Moq + `MockDbSetFactory`; test handlers directly
- **Library.APITests**: integration tests via `WebApplicationFactory<Program>` (`LibraryWebAppFactory`); mock `ISender` to test Carter endpoint routing and HTTP responses in isolation; also includes gRPC service tests (`gRPC/LibraryServiceTests.cs`) and Mapster mapping tests (`Mappings/GrpcMappingTests.cs`)
- **Library.InfrastructureTests**: test EF Core interceptors
- **UserContent.InfrastructureTests**: unit tests for `CachedUserContentRepository` (mock `IRepository`, `IDistributedCache`, `IConnectionMultiplexer`) and `UserContentService`
- **UserContent.ApplicationTests / UserContent.APITests**: placeholder stubs

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
