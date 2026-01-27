using Archive.API.Behaviors;
using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseNpgsql(builder.Configuration.GetConnectionString("ArchiveDb")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
    await DatabaseSeeder.SeedDatabaseAsync(app.Services, app.Services.GetRequiredService<ILogger<Program>>());

app.UseExceptionHandler();

app.MapCarter();

app.Run();