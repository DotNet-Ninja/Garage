using Garage.Entities;

namespace Garage.Models;

public class GroupIndexModel
{
    public GroupIndexModel()
    {
    }

    public GroupIndexModel(Site site, SitePage page)
    {
        SiteSlug = site.Slug;
        SiteText = site.Text;
        PageSlug = page.Slug;
        PageText = page.Text;
        Groups = page.Groups.Select(x => new GroupModel(x)).ToSortedList();
    }

    public string SiteSlug { get; set; } = string.Empty;
    public string SiteText { get; set; } = string.Empty;
    public string PageSlug { get; set; } = string.Empty;
    public string PageText { get; set; } = string.Empty;
    public List<GroupModel> Groups { get; set; } = [];
}