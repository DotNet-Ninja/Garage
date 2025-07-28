using System.ComponentModel.DataAnnotations;
using Garage.Entities;

namespace Garage.Models;

public class PageEditModel: PageModel
{
    public PageEditModel() : base()
    {
    }

    public PageEditModel(SitePage page, string siteSlug) : base(page)
    {
        SiteSlug = siteSlug;
    }

    [Required]
    public string SiteSlug { get; set; } = string.Empty;

    public void ApplyChanges(SitePage page)
    {
        page.Slug = Slug;
        page.Text = Text;
        page.SortIndex = SortIndex;
    }
}