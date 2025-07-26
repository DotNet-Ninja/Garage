using Garage.Constants;

namespace Garage.Configuration;

public static class Endpoints
{
    public static void Build(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "Default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapControllerRoute(
            name: "StartPages",
            pattern: "{**slug}",
            defaults: new { controller = "Home", action = "StartPage" });
    }
}