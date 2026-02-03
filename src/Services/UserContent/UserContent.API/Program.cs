using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("UserContentDB")!;

// Add services to the container.

builder.Services.AddCarter();

builder.Services.AddMediator((MediatorOptions options) =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

builder.Services.AddScoped<IRepository<UserContentContext>, UserConentRepository>();

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

builder.Services.AddDbContext<UserContentContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<UserContentContext>());

var app = builder.Build();

// Apply migrations and seed in Development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            var context = services.GetRequiredService<UserContentContext>();

            // 1. Apply migrations first (creates tables)
            logger.LogInformation("Applying UserContent database migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("UserContent database migrations applied successfully!");

            // 2. Then seed the data
            await DatabaseSeeder.SeedDatabaseAsync(services, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the UserContent database");
            throw;
        }
    }
}

// Configure the HTTP request pipeline.

app.MapCarter();

app.Run();
