namespace Archive.API.Extenstions
{
    public static class ServiceCollectionDIExtensions
    {
        public static void InjectValidators(this IServiceCollection services)
        {

        }

        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<ArchiveContext>, ArchiveRepository>();
        }
    }
}
