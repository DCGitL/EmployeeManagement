using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Services
{
    public static class ServicesConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            //Note register claims before it can be used
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DelectRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));

                options.AddPolicy("EditRolePolicy",
                   policy => policy.RequireAssertion(context =>
                   context.User.IsInRole("Admin") && context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") || context.User.IsInRole("Super Admin")));

                options.AddPolicy("AdminRolePolicy",
                   policy => policy.RequireRole("Admin"));

            });
            //this is used to change default setting example, accessdenied path, login path, logout path ...etc
            services.ConfigureApplicationCookie(options =>
            {
                
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            
        }
    }
}
