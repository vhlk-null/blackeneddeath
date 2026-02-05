namespace UserContent.API.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<UserContentContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<DbContext>(sp => sp.GetRequiredService<UserContentContext>());

            return services;
        }

        public static IServiceCollection AddRedisServices(this IServiceCollection services, string connectionString)
        {
            var redisOptions = ConfigurationOptions.Parse(connectionString);
            redisOptions.AbortOnConnectFail = false; 
            redisOptions.ConnectTimeout = 10000;
            redisOptions.ConnectRetry = 5;
            redisOptions.ReconnectRetryPolicy = new ExponentialRetry(5000);

            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                try
                {
                    var connection = ConnectionMultiplexer.Connect(redisOptions);
                    return connection;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redisOptions;
                options.InstanceName = "UserContent:";
            });

            return services;
        }

        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<UserContentContext>, UserContentRepository>();
            services.Decorate<IRepository<UserContentContext>, CachedUserContentRepository>();

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
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            return services;
        }

        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
