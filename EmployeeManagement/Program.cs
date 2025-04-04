using EmployeeManagement.Models;
using EmployeeManagement.Models.Adventurework.DemoScrollingPaging;
using EmployeeManagement.ServiceConfigurationInstaller;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var adventureWorksConnectionStr = configuration.GetConnectionString("AdventureWork");
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();




builder.Services.Configure<IISServerOptions>(options =>
{
    options.AutomaticAuthentication = false;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AutomaticAuthentication = false;
});
var employeeConnectionString = configuration.GetConnectionString("EmployeeDBConnection");
builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(employeeConnectionString));
builder.Services.AddTransient<IAdventureWorksRepository>(s => new AdventureWorksRepository(adventureWorksConnectionStr));

builder.Services.AddInstallerServices(configuration);
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();






