namespace UserContent.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string dbConnection = configuration.GetConnectionString("UserContentDB")
                              ?? throw new InvalidOperationException("UserContentDB connection string is missing");

        string redisConnection = configuration.GetConnectionString("Redis")
                                 ?? throw new InvalidOperationException("Redis connection string is missing");

        // Database
        services.AddDbContext<UserContentContext>(options =>
            options.UseNpgsql(dbConnection));

        services.AddScoped<IRepository<UserContentContext>, UserContentRepository>();
        services.Decorate<IRepository<UserContentContext>, CachedUserContentRepository>();

        // Redis
        ConfigurationOptions redisOptions = ConfigurationOptions.Parse(redisConnection);
        redisOptions.AbortOnConnectFail = false;
        redisOptions.ConnectTimeout = 10000;
        redisOptions.ConnectRetry = 5;
        redisOptions.ReconnectRetryPolicy = new ExponentialRetry(5000);

        ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(redisOptions);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConnectionMultiplexerFactory = () => Task.FromResult<IConnectionMultiplexer>(multiplexer);
            options.InstanceName = "UserContent:";
        });

        // Health checks
        services.AddHealthChecks()
            .AddNpgSql(dbConnection)
            .AddRedis(redisConnection);

        return services;
    }
}
