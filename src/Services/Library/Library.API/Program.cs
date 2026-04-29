using Serilog;
using Serilog.Debugging;

SelfLog.Enable(Console.Error);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .Enrich.FromLogContext()
       .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

    string? seqUrl = ctx.Configuration["Seq:ServerUrl"];
    if (!string.IsNullOrWhiteSpace(seqUrl))
        cfg.WriteTo.Seq(seqUrl, apiKey: ctx.Configuration["Seq:ApiKey"]);
});

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration, builder.Environment)
    .AddApiServices(builder.Configuration);

MappingConfig.RegisterMappings();

WebApplication app = builder.Build();

app.UseApiServices();

await app.InitializeDatabaseAsync();

await app.InitializeMeilisearchAsync();

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