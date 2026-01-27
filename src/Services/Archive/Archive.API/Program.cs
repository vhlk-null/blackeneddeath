using Archive.API.Behaviors;
using BuildingBlocks.Behaviors;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("ArchiveDb")!;

builder.Services.InjectValidators();
builder.Services.InjectServices();

builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

    config.AddOpenBehavior(typeof(LogginBehavior<,>));
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<ArchiveContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
    await DatabaseSeeder.SeedDatabaseAsync(app.Services, app.Services.GetRequiredService<ILogger<Program>>());

app.UseExceptionHandler(options => { });

app.MapCarter();

app.UseHealthChecks("/health",
    new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();