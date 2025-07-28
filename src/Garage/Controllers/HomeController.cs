using Garage.Constants;
using Garage.Models;
using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Controllers;

public class HomeController : MvcController<HomeController>
{
    private readonly ISiteService _service;

    public HomeController(IMvcContext<HomeController> context, ISiteService service) : base(context)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var slug = State.SiteSlug ?? Defaults.Sites.Slug;
        var site = await _service.GetSiteAsync(slug);
        if (site is null)
        {
            Logger.LogWarning("Index: Site not found for slug '{Slug}'", slug);
            return NotFoundView($"Site not found: {slug}");
        }

        return RedirectToRoute("StartPages", new { siteSlug = site.Slug, pageSlug = site.DefaultPage });
    }

    [HttpGet]
    public async Task<IActionResult> StartPage(string siteSlug, string? pageSlug)
    {
        var site = await _service.GetSiteAsync(siteSlug);
        if (site is null)
        {
            Logger.LogWarning("Index: Site not found for slug '{Slug}'", siteSlug);
            return NotFoundView($"Site not found: {siteSlug}");
        }
        if (string.IsNullOrWhiteSpace(pageSlug))
        {
            pageSlug = site.DefaultPage;
        }
        var page = site.Pages.FirstOrDefault(x => x.Slug.Equals(pageSlug, StringComparison.CurrentCultureIgnoreCase));
        if(page == null)
        {
            Logger.LogWarning("StartPage: Page not found for slug '{Slug}'", pageSlug);
            return NotFoundView($"Page not found: {pageSlug}");
        }
        State.SiteSlug = site.Slug;
        var model = new StartPageModel(page);
        return View(model);
    }
}
