var builder = WebApplication.CreateBuilder(args);

string userContentDBConnectionString = builder.Configuration.GetConnectionString("UserContentDB")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

// Add services to the container.

builder.Services.AddCarter();

builder.Services.AddMediator((MediatorOptions options) =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = redisConnectionString;
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "UserContent:";
});

builder.Services.AddScoped<UserContentRepository>();
builder.Services.AddScoped<IRepository<UserContentContext>>(sp =>
{
    var baseRepo = sp.GetRequiredService<UserContentRepository>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    var redis = sp.GetRequiredService<IConnectionMultiplexer>();
    return new CachedUserContentRepository(baseRepo, cache, redis);
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<UserContentContext>(options => options.UseNpgsql(userContentDBConnectionString));
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<UserContentContext>());

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

await app.InitializeDatabaseAsync();

app.UseExceptionHandler(options => { });
app.MapCarter();

app.Run();
