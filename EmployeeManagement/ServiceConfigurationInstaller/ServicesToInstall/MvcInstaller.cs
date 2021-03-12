using AutoMapper;
using EmployeeManagement.AutoMapperProfile;
using EmployeeManagement.PartialRender;
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
            services.AddScoped<IPartialRenderService, PartialRenderService>();

            // Auto mapper configuration
            var mappingConfig = new MapperConfiguration(c =>
            {
                c.AddProfile(new MapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
