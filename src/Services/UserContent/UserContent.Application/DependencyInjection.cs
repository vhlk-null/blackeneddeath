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

        // gRPC — force HTTP/2
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        bool isHttps = grpcUrl.StartsWith("https://");

        services.AddGrpcClient<LibraryProtoService.LibraryProtoServiceClient>(options =>
        {
            options.Address = new Uri(grpcUrl);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            SocketsHttpHandler handler = new()
            {
                EnableMultipleHttp2Connections = true,
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
            };

            if (isHttps)
                handler.SslOptions = new System.Net.Security.SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = (_, _, _, _) => true
                };

            return handler;
        });
        services.AddScoped<ILibraryService, LibraryGrpcService>();

        return services;
    }
}
