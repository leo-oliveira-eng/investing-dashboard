using BaseEntity.Domain.Mediator.Contracts;
using DataProvider.CrossCutting.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace DataProvider.CrossCutting
{
    public static class ServiceRegistration
    {
        public static void AddCrossCuttingServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, InternalBus>();
        }
    }
}
