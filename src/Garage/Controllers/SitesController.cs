using Garage.Constants;
using Garage.Entities;
using Garage.Models;
using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Controllers;

public class SitesController : MvcController<SitesController>
{
    private readonly ISiteService _service;

    public SitesController(IMvcContext<SitesController> context, ISiteService service) : base(context)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var sites = await _service.ListSitesAsync();
        var model = sites.ToSortedList().Select(x => new SiteModel(x)).ToList();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var site = await _service.GetSiteAsync(id);
        if (site is null)
        {
            return NotFoundView($"Site '{id}' not found.");
        }
        var model = new SiteModel(site);
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var site = await _service.GetSiteAsync(id);
        if (site is null)
        {
            return NotFoundView($"Site '{id}' not found.");
        }

        await _service.DeleteSiteAsync(site.Slug);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Add()
    {
        var model = new SiteModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(SiteEditModel model)
    {
        var site = new Site
        {
            Id = Guid.NewGuid(),
            Slug = model.Slug,
            Text = model.Text,
            SortIndex = model.SortIndex,
            DefaultPage = "home",
            Pages = new List<SitePage>
            {
                new SitePage
                {
                    Id = Guid.NewGuid(),
                    Slug = "home",
                    Text = "Home",
                    SortIndex = 1,
                    Groups = new List<BookmarkGroup>()
                    {
                        new BookmarkGroup()
                        {
                            Text = "Default Group",
                            SortIndex = 1,
                            Id = Guid.NewGuid(),
                            Bookmarks = new List<Bookmark>()
                            {
                                new Bookmark()
                                {
                                    Id = Guid.NewGuid(),
                                    Text = "Welcome to Garage",
                                    SortIndex = 1,
                                    Icon = BootstrapIcons.Link45Deg,
                                    IconColor = Defaults.Colors.IconColor,
                                    Href = "https://github.com/dotnet-ninja/garage",
                                    OpenInNewTab = true
                                }
                            }
                        }
                    }
                }
            }
        };
        await _service.SaveAsync(site);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var site = await _service.GetSiteAsync(id);
        if (site is null)
        {
            return NotFoundView($"Site '{id}' not found.");
        }
        var model = new SiteEditModel(site);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(SiteEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var site = await _service.GetSiteAsync(model.Slug);
        if (site is null)
        {
            return NotFoundView($"Site '{model.Slug}' not found.");
        }
        site.Text = model.Text;
        site.SortIndex = model.SortIndex;
        site.DefaultPage = model.DefaultPage;
        site.Slug = model.Slug;
        await _service.SaveAsync(site);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Copy(string id)
    {
        var site = await _service.GetSiteAsync(id);
        if (site is null)
        {
            return NotFoundView($"Site '{id}' not found.");
        }
        var model = new SiteCopyModel()
        {
            SourceSlug = id,
            SourceText = site.Text,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Copy(SiteCopyModel model)
    {
        var source = await _service.GetSiteAsync(model.SourceSlug);
        if (source is null)
        {
            return NotFoundView($"Source site '{model.SourceSlug}' not found.");
        }

        var copy = source.ToCopy(model.Text, model.Slug, model.SortIndex);
        await _service.SaveAsync(copy);
        return RedirectToAction("Index");
    }
}