using Garage.Models;
using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Controllers;

public class HomeController : MvcController<HomeController>
{
    private readonly IUserPageService _service;

    public HomeController(IMvcContext<HomeController> context, IUserPageService service) : base(context)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var pages = await _service.GetUserPageCollectionAsync(User);
        var page = pages.First();
        return Redirect(page.Slug);
    }

    public async Task<IActionResult> StartPage(string slug)
    {
        var pages = await _service.GetUserPageCollectionAsync(User);
        var page = pages.FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.CurrentCultureIgnoreCase));
        if(page == null)
        {
            Logger.LogWarning("StartPage: Page not found for slug '{Slug}'", slug);
            return NotFoundView($"Page not found: {slug}");
        }

        var model = new StartPageModel(page);
        return View(model);
    }
}
