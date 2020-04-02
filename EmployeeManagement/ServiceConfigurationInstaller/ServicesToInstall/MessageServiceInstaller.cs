using MessageManager.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.ServiceConfigurationInstaller.ServicesToInstall
{
    public class MessageServiceInstaller : IInstaller
    {
        public void InstallerService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageService(configuration);
        }
    }
}
