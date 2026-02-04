var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURATION =====
var dbConnection = builder.Configuration.GetConnectionString(ConnectionStrings.UserContentDatabase)
    ?? throw new InvalidOperationException($"{ConnectionStrings.UserContentDatabase} is missing");

var redisConnection = builder.Configuration.GetConnectionString(ConnectionStrings.Redis) 
    ?? throw new InvalidOperationException($"{ConnectionStrings.Redis} is missing");

// ===== SERVICES =====
builder.Services
    .AddDatabaseServices(dbConnection)
    .AddRedisServices(redisConnection)
    .AddRepositoryServices()
    .AddMediatorServices()
    .AddValidationServices();

builder.Services.AddCarter();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ===== APP =====
var app = builder.Build();

await app.InitializeDatabaseAsync();

app.UseExceptionHandler();
app.MapCarter();

app.Run();