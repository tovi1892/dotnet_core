
namespace myProject.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddTenBis();
            services.addUserService();
            services.UseActiveUser();
            services.AddSignalR();
            services.AddSingleton<IActivityRepository, ActivityRepository>();
            return services;
        }
    }
}
