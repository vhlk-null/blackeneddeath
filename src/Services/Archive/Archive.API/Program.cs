var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddDbContext<ArchiveContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ArchiveDb")));

builder.Services.AddScoped<IRepository<ArchiveContext>, ArchiveRepository>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapCarter();

app.Run();