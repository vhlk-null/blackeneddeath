using Library.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using UserContent.Infrastructure.Data.Extensions;
using UserContent.Infrastructure.gRPC;
using UserContent.Infrastructure.Repositories;

namespace UserContent.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("UserContentDB")
            ?? throw new InvalidOperationException("UserContentDB connection string is missing");

        var redisConnection = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Redis connection string is missing");

        var grpcUrl = configuration["GrpcSettings:LibraryUrl"]
            ?? throw new InvalidOperationException("GrpcSettings:LibraryUrl is missing");

        // Database
        services.AddDbContext<UserContentContext>(options =>
            options.UseNpgsql(dbConnection));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<UserContentContext>());

        // Repository + cache decorator
        services.AddScoped<IUserContentRepository, UserContentRepository>();
        services.Decorate<IUserContentRepository, CachedUserContentRepository>();

        // gRPC
        services.AddGrpcClient<LibraryProtoService.LibraryProtoServiceClient>(options =>
        {
            options.Address = new Uri(grpcUrl);
        });
        services.AddScoped<ILibraryService, LibraryGrpcService>();

        // Redis
        var redisOptions = ConfigurationOptions.Parse(redisConnection);
        redisOptions.AbortOnConnectFail = false;
        redisOptions.ConnectTimeout = 10000;
        redisOptions.ConnectRetry = 5;
        redisOptions.ReconnectRetryPolicy = new ExponentialRetry(5000);

        var multiplexer = ConnectionMultiplexer.Connect(redisOptions);
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
