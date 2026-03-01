using UserContent.Application;
using UserContent.Application.Mappings;
using UserContent.Infrastructure;
using UserContent.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ===== SERVICES =====
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

MappingConfig.RegisterMappings();

// ===== APP =====
var app = builder.Build();

await app.InitializeDatabaseAsync();

app.UseExceptionHandler();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
