namespace Library.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddOpenApi();
        services.AddMessageBroker(configuration, "library");
        services.AddCarter();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddSingleton(_ =>
            new MeilisearchClient(configuration["Meilisearch:Url"]!, configuration["Meilisearch:ApiKey"]));

        services.AddAuthentication("GatewayHeader")
            .AddScheme<GatewayHeaderAuthenticationOptions, GatewayHeaderAuthenticationHandler>(
                "GatewayHeader", _ => { });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"));
        });

        string? connectionString = configuration.GetConnectionString(ConnectionStrings.LibraryDatabase);
        services.AddHealthChecks()
            .AddNpgSql(connectionString ?? string.Empty, name: "postgresql");

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapCarter();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = []
        });
        return app;
    }
}
