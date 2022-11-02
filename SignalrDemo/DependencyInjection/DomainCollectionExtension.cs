using SignalrDemo.Service.Repository.Implementation;
using SignalrDemo.Service.Repository.Interface;

namespace SignalrDemo.DependencyInjection
{
    public static class DomainCollectionExtension
    {
        public static IServiceCollection AddDomains(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IOptionchain, Optionchain>();
            services.AddScoped<IChartData, ChartData>();
            return services;
        }
    }
}
