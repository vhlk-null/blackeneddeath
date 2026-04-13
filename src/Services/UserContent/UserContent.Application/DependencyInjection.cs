using Library.Grpc;
using UserContent.Application.Services.gRPC;

namespace UserContent.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        string grpcUrl = configuration["GrpcSettings:LibraryUrl"]
                         ?? throw new InvalidOperationException("GrpcSettings:LibraryUrl is missing");

        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
        services.AddHttpContextAccessor();

        services.AddScoped<IUserContentService, UserContentService>();

        // gRPC — force HTTP/2 cleartext (h2c) for Railway private network
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        services.AddGrpcClient<LibraryProtoService.LibraryProtoServiceClient>(options =>
        {
            options.Address = new Uri(grpcUrl);
        }).ConfigureChannel(o =>
        {
            o.HttpHandler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true,
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
            };
            o.UnsafeUseInsecureChannelCallCredentials = false;
        });
        services.AddScoped<ILibraryService, LibraryGrpcService>();

        return services;
    }
}
