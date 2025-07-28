using Garage.Constants;
using Garage.Entities;
using Garage.Models;
using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.ViewComponents;

public class NavigationViewComponent:ViewComponent
{
    private readonly ISiteService _service;

    public NavigationViewComponent(ISiteService service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Check if the route is "StartPages" and retrieve siteSlug and pageSlug from route data  
        var endpoint = HttpContext.GetEndpoint();
        var routeName = endpoint?.Metadata
            .OfType<RouteNameMetadata>()
            .FirstOrDefault()
            ?.RouteName;
        Navigation menu;

        if (routeName == "StartPages")
        {
            var siteSlug = RouteData.Values["siteSlug"]?.ToString() ?? Defaults.Sites.Slug;
            var site = await _service.GetSiteAsync(siteSlug);
            menu = new Navigation
            {
                Items = (site is null)
                    ?new List<NavigationItem>()
                    : site.Pages.ToSortedList().Select(p => new NavigationItem
                    {
                        Icon = BootstrapIcons.NotSet,
                        Text = p.Text,
                        Url = (Url.RouteUrl("StartPages", new { siteSlug = siteSlug, pageSlug = p.Slug }) 
                               ?? Url.Action("Index", "Home")) ?? string.Empty
                    }).ToList()
            };
            menu.Items.Insert(0, new NavigationItem()
            {
                Icon = BootstrapIcons.Gear,
                Text = string.Empty,
                Url = Url.Action("Index", "Sites") ?? string.Empty
            });
            menu.BrandName = (site is null)? Defaults.ApplicationName : site.Text;

        }
        else
        {
            // Implement logic here to build navigation for Admin Mode
            menu = Navigation.Admin;
        }

        return View(menu);
    }
}