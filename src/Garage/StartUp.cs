using System.IO.Abstractions;
using Garage.Configuration;
using Garage.Data;
using Garage.Services;

namespace Garage;

public class StartUp(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDotNetNinjaMvcCore(configuration, out var settings)
            .AddApplicationHealthChecks(settings)
            .AddSingleton<IFileSystem, FileSystem>()
            .AddScoped<ISiteService, SiteService>()
            .AddSingleton<IEmbeddedDataReader, EmbeddedDataReader>()
            .AddHttpContextAccessor()
            .AddScoped<IStateService ,StateService>()
            .AddControllersWithViews()
            .WithContext();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDotNetNinjaMvc(env);
    }
}