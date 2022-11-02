using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using SignalrDemo.DbEntity;
using SignalrDemo.DependencyInjection;
using SignalrDemo.Hubs;
using SignalrDemo.Service.Repository.Interface;

namespace SignalrDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string DBConnectionString = Configuration["DBConnection"];
                        
            services.AddSignalR();
            services.AddHangfire(config =>
             config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseDefaultTypeSerializer()
             .UseMemoryStorage());
            services.AddHangfireServer();
            services.AddSingleton<StockTicker>();

            UnitOfWorkServiceCollectionExtentions.AddUnitOfWork<DBContext>(services); 
            DomainCollectionExtension.AddDomains(services);
                      
            services.AddDbContext<DBContext>(options => options.UseSqlServer(DBConnectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
            }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthorization();
            app.UseHangfireDashboard();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MyData>("/stocks");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
