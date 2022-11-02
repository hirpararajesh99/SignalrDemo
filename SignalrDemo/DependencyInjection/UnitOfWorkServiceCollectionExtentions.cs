using SignalrDemo.Service.Repository;
using Microsoft.EntityFrameworkCore;
using SignalrDemo.Service.UnitofWork;

namespace SignalrDemo.DependencyInjection
{
    public static class UnitOfWorkServiceCollectionExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IRepositoryFactory, UnitofWorkRepository<TContext>>();
            services.AddScoped<IUnitofWorkRepository, UnitofWorkRepository<TContext>>();
            services.AddScoped<IUnitofWorkRepository<TContext>, UnitofWorkRepository<TContext>>();
            return services;
        }
    }
}
