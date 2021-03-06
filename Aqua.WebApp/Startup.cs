using Aqua.Data;
using Aqua.Data.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aqua.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AquaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddScoped<ILocationRepo, LocationRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IAnimalRepo, AnimalRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "location",
                    pattern: "{controller=Location}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "customer",
                    pattern: "{controller=Customer}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "customer",
                    pattern: "{controller=Order}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "animal",
                    pattern: "{controller=Animal}/{action=Index}/{id?}");
            });
        }
    }
}
