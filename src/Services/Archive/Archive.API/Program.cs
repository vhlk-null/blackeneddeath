using Archive.API.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InjectValidators();
builder.Services.InjectServices();

builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<ArchiveContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ArchiveDb")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseExceptionHandler();

app.MapCarter();

app.Run();