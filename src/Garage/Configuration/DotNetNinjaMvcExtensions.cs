using DotNetNinja.AutoBoundConfiguration;
using Garage.Constants;
using Garage.Controllers;
using Garage.Services;

namespace Garage.Configuration;

public static class DotNetNinjaMvcExtensions
{
    public static IServiceCollection AddDotNetNinjaMvcCore(this IServiceCollection services, IConfiguration configuration,
        out IAutoBoundConfigurationProvider provider)
    {
        return services.AddAutoBoundConfigurations(configuration, out provider)
            .AddApplicationServices()
            .AddApplicationHealthChecks(provider)
            ;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddHttpContextAccessor()
            .AddSingleton<ITimeProvider, SystemTimeProvider>();
    }

    public static IServiceCollection AddAutoBoundConfigurations
        (this IServiceCollection services, IConfiguration configuration, out IAutoBoundConfigurationProvider provider)
    {
        provider = services.AddAutoBoundConfigurations(configuration).FromAssemblyOf<StartUp>().Provider;
        return services;
    }

    public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IAutoBoundConfigurationProvider provider)
    {
        var checks = services.AddHealthChecks();
        return services;
    }

    public static IApplicationBuilder UseDotNetNinjaMvc(this IApplicationBuilder builder, IWebHostEnvironment environment)
    {
        return builder.UseStrictTransportSecurity(environment)
            .UseGlobalExceptionHandler(environment)
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseApplicationEndpoints();
    }

    public static IApplicationBuilder UseApplicationEndpoints(this IApplicationBuilder app)
    {
        return app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks(WellKnownEndpoint.HealthChecks.Liveliness, CustomHealthCheckOptions.LivelinessOptions);
            endpoints.MapHealthChecks(WellKnownEndpoint.HealthChecks.Databases, CustomHealthCheckOptions.TaggedDefaultOptions("Database"));
            endpoints.MapHealthChecks(WellKnownEndpoint.HealthChecks.Readiness, CustomHealthCheckOptions.Default);
            Endpoints.Build(endpoints);
        });
    }

    public static IApplicationBuilder UseStrictTransportSecurity(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseHsts();
        }
        return app;
    }

    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            return app.UseDeveloperExceptionPage();
        }

        return app.UseExceptionHandler(WellKnownEndpoint.ErrorHandler);
    }

    public static IServiceCollection WithContext(this IMvcBuilder builder)
    {
        builder.Services
            .AddScoped(typeof(IMvcContext<>), typeof(MvcContext<>));
        return builder.Services;
    }
}