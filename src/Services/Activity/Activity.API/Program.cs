using Serilog;
using Serilog.Debugging;

SelfLog.Enable(Console.Error);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .Enrich.FromLogContext()
       .Enrich.WithProperty("Service", "Activity")
       .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

    string? seqUrl = ctx.Configuration["Seq:ServerUrl"];
    if (!string.IsNullOrWhiteSpace(seqUrl) && Uri.TryCreate(seqUrl, UriKind.Absolute, out _))
        cfg.WriteTo.Seq(seqUrl, apiKey: ctx.Configuration["Seq:ApiKey"]);
});

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

WebApplication app = builder.Build();

await app.EnsureMongoIndexesAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

namespace Activity.API
{
    public partial class Program { }
}
