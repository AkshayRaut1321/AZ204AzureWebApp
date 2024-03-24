using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.FeatureManagement;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

var azureAppConfigConnectionString = "Endpoint=https://testappconfigakshay.azconfig.io;Id=jQjs;Secret=wF4/dGhYNb3yNOPEguNd6Bl/AZze8vdOU72fvtfKKH4=";

// Add services to the container.
builder.Services.AddControllersWithViews();

if (false)
{
    #region Example of Configurations stored in Azure App Configuration
    //Install nuget named Microsoft.Extensions.Configuration.AzureAppConfiguration
    // 1. Connect to AzureAppConfig using this
    //      builder.Configuration.AddAzureAppConfiguration(azureAppConfigConnectionString);
    // 2. OR using this
    //      builder.Host.ConfigureAppConfiguration(builder =>
    //      {
    //          builder.AddAzureAppConfiguration(azureAppConfigConnectionString);
    //      });
    #endregion

    #region Example of Features flags in Azure App Configuration
    //Install Microsoft.Extensions.Configuration.AzureAppConfiguration
    // and Microsoft.FeatureManagement
    builder.Host.ConfigureAppConfiguration(builder =>
    {
        builder.AddAzureAppConfiguration((options) =>
        {
            options.Connect(azureAppConfigConnectionString)
                .UseFeatureFlags();
        });
    });
    builder.Services.AddFeatureManagement();
    #endregion
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
