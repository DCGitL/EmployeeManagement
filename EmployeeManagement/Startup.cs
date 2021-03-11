using EmployeeManagement.Models;
using EmployeeManagement.Models.Adventurework.DemoScrollingPaging;
using EmployeeManagement.ServiceConfigurationInstaller;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;
        private readonly string adventureworksConnectionStr;
        public Startup(IConfiguration config)
        {
            _config = config;
            this.adventureworksConnectionStr = config.GetConnectionString("AdventureWork");

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options => {
                options.AutomaticAuthentication = false;
            });
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            services.AddTransient<IAdventureWorksRepository>(s => new AdventureWorksRepository(adventureworksConnectionStr));

            services.AddInstallerServices(_config);
            services.AddHttpClient();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {              
   
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");

            }


            app.UseStaticFiles();
            // app.UseMvcWithDefaultRoute();
            app.UseAuthentication(); 
           
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

            });
            //app.Run(async (context) =>
            //{

              
            //    //System.Diagnostics.Process.GetCurrentProcess().ProcessName
            //    //_config["MyKey"]
            //    await context.Response.WriteAsync("Hello world");

            //});
        }
    }
}
