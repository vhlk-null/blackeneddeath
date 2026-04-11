WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration, builder.Environment)
    .AddApiServices(builder.Configuration);

MappingConfig.RegisterMappings();

WebApplication app = builder.Build();

app.UseApiServices();

if (app.Environment.IsDevelopment())
    await app.InitializeDatabaseAsync();

using (IServiceScope warmUpScope = app.Services.CreateScope())
{
    LibraryContext ctx = warmUpScope.ServiceProvider.GetRequiredService<LibraryContext>();
    await ctx.Database.CanConnectAsync();
}

app.Run();

namespace Library.API
{
    public partial class Program { }
}