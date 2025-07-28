using Garage.Entities;
using Garage.Models;
using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Controllers;

public class GroupsController: MvcController<GroupsController>
{
    private readonly ISiteService _service;

    public GroupsController(IMvcContext<GroupsController> context, ISiteService service) : base(context)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] string siteSlug, [FromRoute] string pageSlug)
    {
        var entities = await FindPageAsync(siteSlug, pageSlug);
        if (!entities.Found)
        {
            return NotFoundView($"Page '{pageSlug}' not found in site '{siteSlug}'.");
        }
        var model = new GroupIndexModel(entities.Site!, entities.Page!);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Add([FromRoute] string siteSlug, [FromRoute] string pageSlug)
    {
        var entities = await FindPageAsync(siteSlug, pageSlug);
        if(!entities.Found)
        {
            return NotFoundView($"Page '{pageSlug}' not found in site '{siteSlug}'.");
        }
        var model = new GroupEditModel
        {
            SiteSlug = entities.Site!.Slug,
            SiteText = entities.Site.Text,
            PageSlug = entities.Page!.Slug,
            GroupId = Guid.NewGuid(),
            Text = string.Empty,
            PageText = entities.Page!.Text,
            SortIndex = (entities.Page!.Groups.Count > 0) ? entities.Page.Groups.Max(g => g.SortIndex) + 1 : 1
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromRoute] string siteSlug, [FromRoute] string pageSlug,
        [FromForm] GroupEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var entities = await FindPageAsync(siteSlug, pageSlug);
        if (!entities.Found)
        {
            return NotFoundView($"Page '{pageSlug}' not found in site '{siteSlug}'.");
        }
        var site = entities.Site!;
        var page = entities.Page!;
        var newGroup = new BookmarkGroup
        {
            Id = model.GroupId,
            Text = model.Text,
            SortIndex = model.SortIndex,
            Bookmarks = []
        };
        page.Groups.Add(newGroup);

        await _service.SaveAsync(site);
        Logger.LogInformation("Added group '{GroupText}' to page '{PageSlug}' in site '{SiteSlug}'.",
            model.Text, page.Slug, site.Slug);
        return RedirectToAction("Index", new { siteSlug = site.Slug, pageSlug = page.Slug });
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] string siteSlug, [FromRoute] string pageSlug, [FromRoute] Guid id)
    {
        var entities = await FindGroupAsync(siteSlug, pageSlug, id);
        if (!entities.Found)
        {
            return NotFoundView($"Group '{id}' not found in page '{pageSlug}' in site '{siteSlug}'.");
        }
        var model = new GroupEditModel
        {
            SiteSlug = entities.Site!.Slug,
            SiteText = entities.Site.Text,
            PageSlug = entities.Page!.Slug,
            PageText = entities.Page.Text,
            GroupId = entities.Group!.Id,
            Text = entities.Group.Text,
            SortIndex = entities.Group.SortIndex
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] string siteSlug, [FromRoute] string pageSlug, [FromRoute] Guid id,
        [FromForm] GroupEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var entities = await FindGroupAsync(siteSlug, pageSlug, id);
        if (!entities.Found)
        {
            return NotFoundView($"Group '{id}' not found in page '{pageSlug}' in site '{siteSlug}'.");
        }
        var site = entities.Site!;
        var page = entities.Page!;
        var group = entities.Group!;
        group.Text = model.Text;
        group.SortIndex = model.SortIndex;
        await _service.SaveAsync(site);
        Logger.LogInformation("Edited group '{GroupText}' in page '{PageSlug}' in site '{SiteSlug}'.",
            model.Text, page.Slug, site.Slug);
        return RedirectToAction("Index", new { siteSlug = site.Slug, pageSlug = page.Slug });
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute] string siteSlug, [FromRoute] string pageSlug,
        [FromRoute] Guid id)
    {
        var entities = await FindGroupAsync(siteSlug, pageSlug, id);
        if (!entities.Found)
        {
            return NotFoundView($"Group '{id}' not found in page '{pageSlug}' in site '{siteSlug}'.");
        }

        var model = new GroupDeleteModel()
        {
            SiteSlug = entities.Site!.Slug,
            SiteText = entities.Site.Text,
            PageSlug = entities.Page!.Slug,
            PageText = entities.Page.Text,
            GroupId = entities.Group!.Id,
            GroupText = entities.Group.Text
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromRoute] string siteSlug, [FromRoute] string pageSlug,
        [FromRoute] Guid id, [FromForm] GroupDeleteModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var entities = await FindGroupAsync(siteSlug, pageSlug, id);
        if (!entities.Found)
        {
            return NotFoundView($"Group '{id}' not found in page '{pageSlug}' in site '{siteSlug}'.");
        }
        var site = entities.Site!;
        var page = entities.Page!;
        var group = entities.Group!;
        page.Groups.Remove(group);
        await _service.SaveAsync(site);
        Logger.LogInformation("Deleted group '{GroupText}' from page '{PageSlug}' in site '{SiteSlug}'.",
            group.Text, page.Slug, site.Slug);
        return RedirectToAction("Index", new { siteSlug = site.Slug, pageSlug = page.Slug });
    }

    private async Task<(Site? Site, SitePage? Page, bool Found)> FindPageAsync(string siteSlug, string pageSlug)
    {
        var site = await _service.GetSiteAsync(siteSlug);
        if (site is null)
        {
            Logger.LogWarning("Site '{SiteSlug}' not found.", siteSlug);
            return (null, null, false);
        }
        var page = site.Pages.FirstOrDefault(p => p.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase));
        if (page is null)
        {
            Logger.LogWarning("Page '{PageSlug}' not found in site '{SiteSlug}'.", pageSlug, siteSlug);
            return (site, null, false);
        }
        return (site, page, true);
    }

    private async Task<(Site? Site, SitePage? Page, BookmarkGroup? Group, bool Found)> FindGroupAsync(string siteSlug, string pageSlug, Guid groupId)
    {
        var info = await FindPageAsync(siteSlug, pageSlug);
        if (!info.Found)
        {
            return (info.Site, info.Page, null, false);
        }
        var group = info.Page!.Groups.FirstOrDefault(g => g.Id == groupId);
        if (group is null)
        {
            Logger.LogWarning("Group '{GroupId}' not found in page '{PageSlug}' in site '{SiteSlug}'.", groupId, pageSlug, siteSlug);
            return (info.Site, info.Page, null, false);
        }
        return (info.Site, info.Page, group, true);
    }
}