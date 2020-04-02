using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.ServiceConfigurationInstaller.ServicesToInstall
{
    public class MvcInstaller : IInstaller
    {
        public void InstallerService(IServiceCollection services, IConfiguration configuration)
        {

            //This sets authorization attributes on all controller 

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }
            ).AddXmlSerializerFormatters();

            //  services.AddMvc().AddXmlSerializerFormatters();
            services.AddCustomServices();

        }
    }
}
