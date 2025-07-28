using Garage.Constants;
using Garage.Entities;
using Garage.Models;
using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Controllers;
public class PagesController : MvcController<PagesController>
{
    private readonly ISiteService _service;

    public PagesController(IMvcContext<PagesController> context, ISiteService service) : base(context)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromRoute]string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            id = Defaults.Sites.Slug;
        }
        var site = await _service.GetSiteAsync(id);
        if(site is null)
        {
            return NotFoundView($"Site '{id}' not found.");
        }

        var model = new PageIndexModel(site);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute]string siteSlug, [FromRoute]string pageSlug)
    {
        var site = await _service.GetSiteAsync(siteSlug);
        if (site is null)
        {
            return NotFoundView($"Site '{siteSlug}' not found.");
        }
        var page = site.Pages.FirstOrDefault(p => p.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase));
        if (page is null)
        {
            return NotFoundView($"Page '{pageSlug}' not found in site '{siteSlug}'.");
        }

        var model = new PageEditModel(page, siteSlug);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute]string siteSlug, [FromRoute]string pageSlug, [FromForm]PageEditModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        var site = await _service.GetSiteAsync(siteSlug);
        if (site is null)
        {
            return NotFoundView($"Site '{siteSlug}' not found.");
        }
        var page = site.Pages.FirstOrDefault(p => p.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase));
        if (page is null)
        {
            return NotFoundView($"Page '{pageSlug}' not found in site '{siteSlug}'.");
        }
        if (site.Pages.Any(p => p.Slug.Equals(model.Slug, StringComparison.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError(nameof(model.Slug), $"A page with the slug '{model.Slug}' already exists in site '{site.Slug}'.");
            return View(model);
        }
        model.ApplyChanges(page);
        await _service.SaveAsync(site);
        return RedirectToAction("Index", "Pages", new { id = siteSlug });
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute] string siteSlug, [FromRoute] string pageSlug)
    {
        var site = await _service.GetSiteAsync(siteSlug);
        if (site is null)
        {
            return NotFoundView($"Site '{siteSlug}' not found.");
        }
        var page = site.Pages.FirstOrDefault(p => p.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase));
        if (page is null)
        {
            return NotFoundView($"Page '{pageSlug}' not found in site '{siteSlug}'.");
        }

        var model = new PageDeleteModel
        {
            PageSlug = page.Slug,
            PageText = page.Text,
            SiteSlug = site.Slug,
            SiteText = site.Text
        };

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeletePost([FromRoute] string siteSlug, [FromRoute] string pageSlug)
    {
        var site = await _service.GetSiteAsync(siteSlug);
        if (site is null)
        {
            return NotFoundView($"Site '{siteSlug}' not found.");
        }
        var page = site.Pages.FirstOrDefault(p => p.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase));
        if (page is null)
        {
            return NotFoundView($"Page '{pageSlug}' not found in site '{siteSlug}'.");
        }
        site.Pages.Remove(page);
        await _service.SaveAsync(site);
        return RedirectToAction("Index", "Pages", new { id = siteSlug });
    }

    [HttpGet]
    public async Task<IActionResult> Add(string id)
    {
        var site = await _service.GetSiteAsync(id);
        if (site is null)
        {
            return NotFoundView($"Site '{id}' not found.");
        }
        
        var model = new PageEditModel
        {
            SiteSlug = site.Slug,
            PageId = Guid.NewGuid(),
            Slug = string.Empty,
            Text = string.Empty,
            SortIndex = site.Pages.Count > 0 ? site.Pages.Max(p => p.SortIndex) + 1 : 1
        };

        return View(model);
    }

    public async Task<IActionResult> Add([FromRoute] string id, [FromForm] PageEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var site = await _service.GetSiteAsync(id);
        if (site is null)
        {
            return NotFoundView($"Site '{id}' not found.");
        }
        if (site.Pages.Any(p => p.Slug.Equals(model.Slug, StringComparison.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError(nameof(model.Slug), $"A page with the slug '{model.Slug}' already exists in site '{site.Slug}'.");
            return View(model);
        }
        var page = new SitePage
        {
            Id = model.PageId != Guid.Empty ? model.PageId : Guid.NewGuid(),
            Slug = model.Slug,
            Text = model.Text,
            SortIndex = model.SortIndex,
            Groups = []
        };
        site.Pages.Add(page);
        await _service.SaveAsync(site);
        return RedirectToAction("Index", "Pages", new { id = site.Slug });
    }
}
