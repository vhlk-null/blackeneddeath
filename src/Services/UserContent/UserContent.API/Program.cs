using BuildingBlocks.Behaviors;
using BuildingBlocks.Repositories;
using Microsoft.EntityFrameworkCore;
using UserContent.API.Data;

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

builder.Services.AddDbContext<UserContentContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter();

app.Run();
