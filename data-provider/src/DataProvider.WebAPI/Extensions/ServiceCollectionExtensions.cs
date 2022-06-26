using DataProvider.CrossCutting;

namespace DataProvider.WebAPI.Extensions
{
    /// <summary>
    /// Service Collection extensions methods
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension to configure health checks
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration</param>
        public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddMySql(configuration["ConnectionStrings:SqlDatabase"]);
        }

        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddCrossCuttingServices();
        }
    }
}
