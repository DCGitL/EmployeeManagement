using EmployeeManagement.ServiceConfigurationInstaller.ServicesToInstall;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EmployeeManagement.ServiceConfigurationInstaller
{
    public static class InstallerServiceExtension
    {
        public static void AddInstallerServices(this IServiceCollection services, IConfiguration configuration)
        {

            var installers = typeof(Program).Assembly.ExportedTypes.Where(t => typeof(IInstaller).IsAssignableFrom(t) && !t.IsInterface & !t.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
            installers.ForEach(installer => installer.InstallerService(services, configuration));

        }
    }
}
