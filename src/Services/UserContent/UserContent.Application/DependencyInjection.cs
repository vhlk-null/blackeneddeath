using Library.Grpc;
using UserContent.Application.Services.gRPC;

namespace UserContent.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var grpcUrl = configuration["GrpcSettings:LibraryUrl"]
            ?? throw new InvalidOperationException("GrpcSettings:LibraryUrl is missing");

        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        services.AddScoped<IUserContentService, UserContentService>();

        // gRPC
        services.AddGrpcClient<LibraryProtoService.LibraryProtoServiceClient>(options =>
        {
            options.Address = new Uri(grpcUrl);
        });
        services.AddScoped<ILibraryService, LibraryGrpcService>();

        return services;
    }
}
