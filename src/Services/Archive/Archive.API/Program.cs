var builder = WebApplication.CreateBuilder(args);

// ===== MAPPINGS =====
MappingConfig.RegisterMappings();

// ===== CONFIGURATION =====
var dbConnection = builder.Configuration.GetConnectionString(ConnectionStrings.ArchiveDatabase)
    ?? throw new InvalidOperationException($"{ConnectionStrings.ArchiveDatabase} connection string is missing");

// ===== SERVICES =====
builder.Services
    .AddDatabaseServices(dbConnection)
    .AddMediatorServices()
    .AddValidationServices()
    .AddHealthCheckServices(dbConnection)
    .AddApiDocumentation();

builder.Services.AddCarter();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ===== APP =====
var app = builder.Build();

// Database initialization
await app.InitializeDatabaseAsync();

// Exception handling middleware
app.UseExceptionHandler();

// Endpoints
app.MapCarter();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();