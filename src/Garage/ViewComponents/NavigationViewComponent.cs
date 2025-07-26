using Garage.Constants;
using Garage.Entities;
using Garage.Models;
using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.ViewComponents;

public class NavigationViewComponent:ViewComponent
{
    private readonly IUserPageService _service;

    public NavigationViewComponent(IUserPageService service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var pages = await _service.GetUserPageCollectionAsync(HttpContext.User);
        var menu = new Navigation
        {
            Items = pages.ToSortedList().Select(p => new NavigationItem
            {
                Icon = BootstrapIcons.NotSet,
                Text = p.Text,
                Url = (Url.RouteUrl("StartPages", new {slug = p.Slug})??Url.Action("Index", "Home")) ?? string.Empty
            }).ToList()
        };
        return View(menu);
    }
}