using BuildingBlocks.Behaviors;
using Microsoft.EntityFrameworkCore;
using UserContent.API.Data;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("UserContentDB")!;

// Add services to the container.

builder.Services.AddCarter();
builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
    conf.AddOpenBehavior(typeof(LogginBehavior<,>));
});

builder.Services.AddDbContext<UserContentContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter();

app.Run();
