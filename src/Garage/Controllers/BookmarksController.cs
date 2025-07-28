using Garage.Constants;
using Garage.Entities;
using Garage.Models;
using Garage.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StringExtensions = Humanizer.StringExtensions;

namespace Garage.Controllers;
public class BookmarksController : MvcController<BookmarksController>
{
    private readonly ISiteService _service;

    public BookmarksController(IMvcContext<BookmarksController> context, ISiteService service) : base(context)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] string siteSlug, [FromRoute] string pageSlug, [FromRoute] Guid groupId)
    {
        var entities = await FindGroupEntities(siteSlug, pageSlug, groupId);
        if(!entities.Found)
        {
            var message = entities.Site == null
                ? $"Site '{siteSlug}' not found."
                : entities.Page == null
                    ? $"Page '{pageSlug}' not found in site '{siteSlug}'."
                    : entities.Group == null
                        ? $"Group '{groupId}' not found in page '{pageSlug}' of site '{siteSlug}'."
                        : "Unknown error.";
            return NotFoundView(message);
        }

        var model = new BookmarkIndexModel()
        {
            SiteSlug = entities.Site!.Slug,
            SiteText = entities.Site!.Text,
            PageSlug = entities.Page!.Slug,
            PageText = entities.Page!.Text,
            GroupId = entities.Group!.Id,
            GroupText = entities.Group!.Text,
            Bookmarks = entities.Group!.Bookmarks.ToSortedList().Select(x => new BookmarkModel(x)).ToList()
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Add([FromRoute] string siteSlug, [FromRoute] string pageSlug,
        [FromRoute] Guid groupId)
    {
        var entities = await FindGroupEntities(siteSlug, pageSlug, groupId);
        if (!entities.Found)
        {
            var message = entities.Site == null
                ? $"Site '{siteSlug}' not found."
                : entities.Page == null
                    ? $"Page '{pageSlug}' not found in site '{siteSlug}'."
                    : entities.Group == null
                        ? $"Group '{groupId}' not found in page '{pageSlug}' of site '{siteSlug}'."
                        : "Unknown error.";
            return NotFoundView(message);
        }
        var model = new BookmarkEditModel()
        {
            SiteSlug = entities.Site!.Slug,
            SiteText = entities.Site!.Text,
            PageSlug = entities.Page!.Slug,
            PageText = entities.Page!.Text,
            GroupId = entities.Group!.Id,
            GroupText = entities.Group!.Text,
            BookmarkId = Guid.NewGuid(),
            Text = string.Empty,
            Href = string.Empty,
            Icon = BootstrapIcons.GarageLink,
            IconColor = Defaults.Colors.IconColor,
            OpenInNewTab = true,
            SortIndex = (entities.Group!.Bookmarks.Count > 0)
                ? entities.Group!.Bookmarks.Max(b => b.SortIndex) + 1
                : 1
        };
        PopulateModelLists(model, entities.Site!.Pages, model.GroupId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromRoute] string siteSlug, [FromRoute] string pageSlug,
        [FromRoute] Guid groupId, BookmarkEditModel model)
    {
        var entities = await FindGroupEntities(siteSlug, pageSlug, model.GroupId);
        if (!entities.Found)
        {
            var message = entities.Site == null
                ? $"Site '{siteSlug}' not found."
                : entities.Page == null
                    ? $"Page '{pageSlug}' not found in site '{siteSlug}'."
                    : entities.Group == null
                        ? $"Group '{groupId}' not found in page '{pageSlug}' of site '{siteSlug}'."
                        : "Unknown error.";
            return NotFoundView(message);
        }
        if (!ModelState.IsValid)
        {
            PopulateModelLists(model, entities.Site!.Pages, model.GroupId);
            return View(model);
        }

        var bookmark = new Bookmark
        {
            Id = model.BookmarkId,
            Text = model.Text,
            Href = model.Href,
            Icon = model.Icon,
            IconColor = model.IconColor,
            OpenInNewTab = model.OpenInNewTab,
            SortIndex = model.SortIndex
        };
        entities.Group!.Bookmarks.Add(bookmark);
        await _service.SaveAsync(entities.Site!);
        TempData["SuccessMessage"] = $"Bookmark '{bookmark.Text}' added successfully.";
        return RedirectToAction("Index", new { siteSlug = entities.Site!.Slug, pageSlug = entities.Page!.Slug, groupId = entities.Group!.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] string siteSlug, [FromRoute] string pageSlug,
        [FromRoute] Guid groupId, [FromRoute] Guid id)
    {
        var entities = await FindGroupEntities(siteSlug, pageSlug, groupId);
        if (!entities.Found)
        {
            var message = entities.Site == null
                ? $"Site '{siteSlug}' not found."
                : entities.Page == null
                    ? $"Page '{pageSlug}' not found in site '{siteSlug}'."
                    : entities.Group == null
                        ? $"Group '{groupId}' not found in page '{pageSlug}' of site '{siteSlug}'."
                        : "Unknown error.";
            return NotFoundView(message);
        }
        var bookmark = entities.Group!.Bookmarks.FirstOrDefault(b => b.Id == id);
        if (bookmark == null)
        {
            var message = $"Bookmark '{id}' not found in group '{entities.Group!.Text}' of page '{entities.Page!.Text}' in site '{entities.Site!.Text}'.";
            Logger.LogWarning(message);
            return NotFoundView(message);
        }
        var model = new BookmarkEditModel(siteSlug, pageSlug, groupId, bookmark)
        {
            SiteText = entities.Site!.Text,
            PageText = entities.Page!.Text,
            GroupText = entities.Group!.Text
        };
        PopulateModelLists(model, entities.Site!.Pages, model.GroupId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] string siteSlug, [FromRoute] string pageSlug,
        [FromRoute] Guid groupId, [FromRoute] Guid id, BookmarkEditModel model)
    {
        var entities = await FindGroupEntities(siteSlug, pageSlug, groupId);
        if (!entities.Found)
        {
            var message = entities.Site == null
                ? $"Site '{siteSlug}' not found."
                : entities.Page == null
                    ? $"Page '{pageSlug}' not found in site '{siteSlug}'."
                    : entities.Group == null
                        ? $"Group '{groupId}' not found in page '{pageSlug}' of site '{siteSlug}'."
                        : "Unknown error.";
            return NotFoundView(message);
        }
        var bookmark = entities.Group!.Bookmarks.FirstOrDefault(b => b.Id == id);
        if (bookmark == null)
        {
            var message = $"Bookmark '{id}' not found in group '{entities.Group!.Text}' of page '{entities.Page!.Text}' in site '{entities.Site!.Text}'.";
            Logger.LogWarning(message);
            return NotFoundView(message);
        }
        if (!ModelState.IsValid)
        {
            PopulateModelLists(model, entities.Site!.Pages, model.GroupId);
            return View(model);
        }
        // Update the bookmark
        bookmark.Text = model.Text;
        bookmark.Href = model.Href;
        bookmark.Icon = model.Icon;
        bookmark.IconColor = model.IconColor;
        bookmark.OpenInNewTab = model.OpenInNewTab;
        bookmark.SortIndex = model.SortIndex;

        if (model.GroupId != entities.Group.Id)
        {
            var newGroup = entities.Site?.Pages.SelectMany(p => p.Groups)
                .FirstOrDefault(g => g.Id == model.GroupId);
            if (newGroup is not null)
            {
                newGroup.Bookmarks.Add(bookmark);
                entities.Group.Bookmarks.Remove(bookmark);
            }
        }

        await _service.SaveAsync(entities.Site!);
        TempData["SuccessMessage"] = $"Bookmark '{bookmark.Text}' updated successfully.";
        return RedirectToAction("Index", new { siteSlug = entities.Site!.Slug, pageSlug = entities.Page!.Slug, groupId = entities.Group!.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute] string siteSlug, [FromRoute] string pageSlug, [FromRoute] Guid groupId,
        [FromRoute] Guid id)
    {
        var entities = await FindGroupEntities(siteSlug, pageSlug, groupId);
        if (!entities.Found)
        {
            var message = entities.Site == null
                ? $"Site '{siteSlug}' not found."
                : entities.Page == null
                    ? $"Page '{pageSlug}' not found in site '{siteSlug}'."
                    : entities.Group == null
                        ? $"Group '{groupId}' not found in page '{pageSlug}' of site '{siteSlug}'."
                        : "Unknown error.";
            return NotFoundView(message);
        }
        var bookmark = entities.Group!.Bookmarks.FirstOrDefault(b => b.Id == id);
        if (bookmark == null)
        {
            var message = $"Bookmark '{id}' not found in group '{entities.Group!.Text}' of page '{entities.Page!.Text}' in site '{entities.Site!.Text}'.";
            Logger.LogWarning(message);
            return NotFoundView(message);
        }
        var model = new BookmarkDeleteModel()
        {
            SiteSlug = entities.Site!.Slug,
            SiteText = entities.Site!.Text,
            PageSlug = entities.Page!.Slug,
            PageText = entities.Page!.Text,
            GroupId = entities.Group!.Id,
            GroupText = entities.Group!.Text,
            BookmarkId = bookmark.Id,
            BookmarkText = bookmark.Text
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromRoute] string siteSlug, [FromRoute] string pageSlug, [FromRoute] Guid groupId,
        [FromRoute] Guid id, BookmarkDeleteModel model)
    {
        var entities = await FindGroupEntities(siteSlug, pageSlug, groupId);
        if (!entities.Found)
        {
            var message = entities.Site == null
                ? $"Site '{siteSlug}' not found."
                : entities.Page == null
                    ? $"Page '{pageSlug}' not found in site '{siteSlug}'."
                    : entities.Group == null
                        ? $"Group '{groupId}' not found in page '{pageSlug}' of site '{siteSlug}'."
                        : "Unknown error.";
            return NotFoundView(message);
        }
        var bookmark = entities.Group!.Bookmarks.FirstOrDefault(b => b.Id == id);
        if (bookmark == null)
        {
            var message = $"Bookmark '{id}' not found in group '{entities.Group!.Text}' of page '{entities.Page!.Text}' in site '{entities.Site!.Text}'.";
            Logger.LogWarning(message);
            return NotFoundView(message);
        }
        entities.Group!.Bookmarks.Remove(bookmark);
        await _service.SaveAsync(entities.Site!);
        TempData["SuccessMessage"] = $"Bookmark '{bookmark.Text}' deleted successfully.";
        return RedirectToAction("Index", new { siteSlug = entities.Site!.Slug, pageSlug = entities.Page!.Slug, groupId = entities.Group!.Id });
    }

    private async Task<(Site? Site, SitePage? Page, BookmarkGroup? Group, bool Found)> FindGroupEntities(string siteSlug,
        string pageSlug, Guid groupId)
    {
        var site = await _service.GetSiteAsync(siteSlug);
        if (site == null)
        {
            Logger.LogWarning("Site not found: {SiteSlug}", siteSlug);
            return (null, null, null, false);
        }
        var page = site.Pages.FirstOrDefault(p => string.Equals(p.Slug, pageSlug, StringComparison.OrdinalIgnoreCase));
        if (page == null)
        {
            Logger.LogWarning("Page not found: {SiteSlug}/{PageSlug}", siteSlug, pageSlug);
            return (site, null, null, false);
        }
        var group = page.Groups.FirstOrDefault(g => g.Id == groupId);
        if (group == null)
        {
            Logger.LogWarning("Group not found: {SiteSlug}/{PageSlug} - {GroupId}", siteSlug, pageSlug, groupId);
            return (site, page, null, false);
        }
        return (site, page, group, true);
    }

    private void PopulateModelLists(BookmarkEditModel model, List<SitePage> pages, Guid selectedGroupId)
    {
        model.IconList = Enum.GetValues<BootstrapIcons>()
            .Select(x => new SelectListItem(x.Humanize(), ((int)x).ToString())).OrderBy(x => x.Text).ToList();
        var groups = new List<SelectListItem>();
        foreach (var page in pages.ToSortedList())
        {
            var grouping = new SelectListGroup() { Name = page.Text };
            var current = page.Groups.ToSortedList().Select(g => 
                new SelectListItem
                { 
                    Text=g.Text, 
                    Value=g.Id.ToString(), 
                    Selected = g.Id == selectedGroupId, 
                    Group = grouping
                }
            ).ToList();
            groups.AddRange(current);
        }
        model.Groups = groups;
    }
}
