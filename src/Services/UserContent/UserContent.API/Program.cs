WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ===== SERVICES =====
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

MappingConfig.RegisterMappings();

// ===== APP =====
WebApplication app = builder.Build();

await app.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// gRPC warm-up — establish connection on startup to avoid cold-start latency
_ = Task.Run(async () =>
{
    await Task.Delay(TimeSpan.FromSeconds(5));
    try
    {
        using IServiceScope scope = app.Services.CreateScope();
        ILibraryService libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        await libraryService.GetAlbumByIdAsync(Guid.Empty);
    }
    catch { /* expected — just warming up the channel */ }
});

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

namespace UserContent.API
{
    public partial class Program { }
}
