using Garage.Constants;

namespace Garage.Configuration;

public static class Endpoints
{
    public static void Build(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "Bookmarks",
            pattern: "Bookmarks/{siteSlug}/{pageSlug}/{groupId}/{action}/{id?}",
            defaults: new { controller = "Bookmarks", action = "Index" });

        endpoints.MapControllerRoute(
            name: "Groups",
            pattern: "Groups/{siteSlug}/{pageSlug}/{action}/{id?}",
            defaults: new { controller = "Groups", action = "Index" });

        endpoints.MapControllerRoute(
            name: "Pages",
            pattern: "Pages/{siteSlug}/{action}/{pageSlug}",
            defaults: new { controller = "Pages" });

        endpoints.MapControllerRoute(
            name: "Default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        endpoints.MapControllerRoute(
            name: "StartPages",
            pattern: "{siteSlug}/{pageSlug?}",
            defaults: new { controller = "Home", action = "StartPage" });
    }
}
