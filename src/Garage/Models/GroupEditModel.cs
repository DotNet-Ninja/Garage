using System.ComponentModel.DataAnnotations;
using Garage.Entities;

namespace Garage.Models;

public class GroupEditModel: GroupModel
{
    public GroupEditModel() : base()
    {
    }

    public GroupEditModel(string siteSlug, string pageSlug, BookmarkGroup group) : base(group)
    {
        SiteSlug = siteSlug;
        PageSlug = pageSlug;
    }

    [Required]
    public string SiteSlug { get; set; } = string.Empty;

    [Required]
    public string SiteText { get; set; } = string.Empty;

    [Required]
    public string PageSlug { get; set; } = string.Empty;

    [Required]
    public string PageText { get; set; } = string.Empty;
}