WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration, builder.Environment)
    .AddApiServices(builder.Configuration);

MappingConfig.RegisterMappings();

WebApplication app = builder.Build();

app.UseApiServices();

await app.InitializeDatabaseAsync();

app.Run();

namespace Library.API
{
    public partial class Program { }
}