var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURATION =====
var userContentDbConnection = builder.Configuration.GetConnectionString("UserContentDB")
    ?? throw new InvalidOperationException("UserContentDB connection string is missing");
var redisConnection = builder.Configuration.GetConnectionString("Redis")
    ?? throw new InvalidOperationException("Redis connection string is missing");

// ===== DATABASE =====
builder.Services.AddDbContext<UserContentContext>(options =>
    options.UseNpgsql(userContentDbConnection));

builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<UserContentContext>());

// ===== REDIS =====
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "UserContent:";
});

// ===== REPOSITORY (with Caching Decorator) =====
builder.Services.AddScoped<IRepository<UserContentContext>, UserContentRepository>();
builder.Services.Decorate<IRepository<UserContentContext>, CachedUserContentRepository>();

// ===== MEDIATOR =====
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

// ===== MEDIATOR BEHAVIORS (порядок важливий!) =====
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

// ===== VALIDATION =====
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// ===== ENDPOINTS =====
builder.Services.AddCarter();

// ===== EXCEPTION HANDLING =====
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ===== BUILD & CONFIGURE APP =====
var app = builder.Build();

// Database initialization
await app.InitializeDatabaseAsync();

// Exception handling middleware
app.UseExceptionHandler();

// Map endpoints
app.MapCarter();

app.Run();