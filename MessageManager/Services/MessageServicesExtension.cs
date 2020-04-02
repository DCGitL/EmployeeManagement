using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManager.Services
{
    public static class MessageServicesExtension
    {

        public static void AddMessageService(this IServiceCollection services, IConfiguration configuration)
        {
            var to = configuration.GetSection("EmailConfiguration:To");
            var from = configuration.GetSection("EmailConfiguration:From");
            var subject = configuration.GetSection("EmailConfiguration:Subject");
            var smptserver = configuration.GetSection("EmailConfiguration:SmtpServer");
            var port = configuration.GetSection("EmailConfiguration:Port");
            var password = configuration.GetSection("EmailConfiguration:Password");
            services.Configure<EmailConfiguration>(options =>
            {
                options.To = to.Value;
                options.From = from.Value;
                options.Subject = subject.Value;
                options.SmtpServer = smptserver.Value;
                options.Port = int.Parse(port.Value);
                options.Password = password.Value;
            });

            services.AddScoped<IMessageServices, MessageServices>();

        }
    }
}
