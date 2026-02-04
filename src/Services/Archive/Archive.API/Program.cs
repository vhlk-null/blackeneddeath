var builder = WebApplication.CreateBuilder(args);

MappingConfig.RegisterMappings();

string connectionString = builder.Configuration.GetConnectionString("ArchiveDb")!;

builder.Services.InjectValidators();
builder.Services.InjectServices();
builder.Services.AddCarter();

builder.Services.AddMediator((MediatorOptions options) =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<ArchiveContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<ArchiveContext>());
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

var app = builder.Build();

await app.InitializeDatabaseAsync();

app.UseExceptionHandler(options => { });
app.MapCarter();
app.UseHealthChecks("/health",
    new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();