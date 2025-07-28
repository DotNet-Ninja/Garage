using Garage.Entities;

namespace Garage.Models;

public class PageIndexModel
{
    public PageIndexModel() { }
    public PageIndexModel(string siteSlug, string siteText, List<PageModel> pages)
    {
        SiteSlug = siteSlug;
        SiteText = siteText;
        Pages = pages;
    }

    public PageIndexModel(Site site)
    {
        SiteSlug = site.Slug;
        SiteText = site.Text;
        Pages = site.Pages.Select(x => new PageModel(x)).ToSortedList();
    }

    public string SiteSlug { get; set; } = string.Empty;
    public string SiteText { get; set; } = string.Empty;
    public List<PageModel> Pages { get; set; } = [];
}