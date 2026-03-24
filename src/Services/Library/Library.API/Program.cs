using Library.API.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

MappingConfig.RegisterMappings();

var app = builder.Build();

app.UseApiServices();
app.UseRouting();

await app.InitializeDatabaseAsync();

app.Run();

namespace Library.API
{
    public partial class Program { }
}