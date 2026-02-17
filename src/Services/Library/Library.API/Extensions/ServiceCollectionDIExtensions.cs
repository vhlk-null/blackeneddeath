using Library.API.Data;

namespace Library.API.Extensions
{
    public static class ServiceCollectionDIExtensions
    {
        public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        string connectionString)
        {
            services.AddDbContext<LibraryContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<DbContext>(sp => sp.GetRequiredService<LibraryContext>());

            return services;
        }

        public static IServiceCollection AddMediatorServices(this IServiceCollection services)
        {
            services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(Behavior.UnitOfWorkBehavior<,>));

            return services;
        }

        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection AddHealthCheckServices(
            this IServiceCollection services,
            string dbConnection)
        {
            services.AddHealthChecks()
                .AddNpgSql(dbConnection, name: "postgresql");

            return services;
        }

        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            return services;
        }

        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<LibraryContext>, LibraryRepository>();

            return services;
        }


        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddGrpc();
            return services;
        }
    }
}
