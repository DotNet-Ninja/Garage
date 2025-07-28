using Garage.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage.Models;

public class BookmarkEditModel: BookmarkModel
{
    public BookmarkEditModel() : base()
    {
    }

    public BookmarkEditModel(string siteSlug, string pageSlug, Guid groupId, Bookmark bookmark) : base(bookmark)
    {
        SiteSlug = siteSlug;
        PageSlug = pageSlug;
        GroupId = groupId;
    }

    [Required]
    public string SiteSlug { get; set; } = string.Empty;

    [Required]
    public string SiteText { get; set; } = string.Empty;

    [Required]
    public string PageSlug { get; set; } = string.Empty;

    [Required]
    public string PageText { get; set; } = string.Empty;

    [Required]
    public Guid GroupId { get; set; } = Guid.Empty;

    [Required]
    public string GroupText { get; set; } = string.Empty;

    public List<SelectListItem> IconList { get; set; } = new List<SelectListItem>();

    public List<SelectListItem> Groups { get; set; } = new List<SelectListItem>();
}