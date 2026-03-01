var builder = WebApplication.CreateBuilder(args);

builder.Services
        .AddApplicationServices()
        .AddInfrastructureServices(builder.Configuration)
        .AddApiServices();
// -----------------------------

// ===== MAPPINGS =====
//MappingConfig.RegisterMappings();

// ===== CONFIGURATION =====
//var dbConnection = builder.Configuration.GetConnectionString(ConnectionStrings.LibraryDatabase)
//    ?? throw new InvalidOperationException($"{ConnectionStrings.LibraryDatabase} connection string is missing");

// ===== SERVICES =====
//builder.Services
//    .AddDatabaseServices(dbConnection)
//    .AddRepositoryServices()
//    .AddMediatorServices()
//    .AddGrpcServices()
//    .AddValidationServices()
//    .AddHealthCheckServices(dbConnection)
//    .AddApiDocumentation();

//builder.Services.AddCarter();
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//builder.Services.AddProblemDetails();

// ===== APP =====
var app = builder.Build();

// Database initialization
//await app.InitializeDatabaseAsync();

// Exception handling middleware
//app.UseExceptionHandler();

// Endpoints
//app.MapCarter();
//app.MapGrpcService<LibraryService>();
//app.MapHealthChecks("/health", new HealthCheckOptions
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

app.UseApiServices();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}

app.Run();