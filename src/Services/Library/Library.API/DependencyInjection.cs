    namespace Library.API;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureHttpJsonOptions(options =>
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddCarter();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.AddGrpc();

            var connectionString = configuration.GetConnectionString(ConnectionStrings.LibraryDatabase);
            services.AddHealthChecks()
                .AddNpgSql(connectionString ?? string.Empty, name: "postgresql");

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.UseExceptionHandler();
            app.MapCarter();
            app.MapGrpcService<LibraryService>();
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            return app;
        }
    }
