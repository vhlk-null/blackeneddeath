using Microsoft.AspNetCore.Server.Kestrel.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, o => o.Protocols = HttpProtocols.Http1);
    options.ListenAnyIP(8082, o => o.Protocols = HttpProtocols.Http2);
    options.ListenAnyIP(8081, o =>
    {
        o.Protocols = HttpProtocols.Http1AndHttp2;
        string? certPath = builder.Configuration["Kestrel:Certificates:Default:Path"];
        if (!string.IsNullOrEmpty(certPath))
        {
            string? certPassword = builder.Configuration["Kestrel:Certificates:Default:Password"];
            o.UseHttps(certPath, certPassword);
        }
        else
        {
            o.UseHttps();
        }
    });
});

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