using Garage.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage.Models;

public class SiteEditModel: SiteModel
{
    public SiteEditModel() : base()
    {
    }

    public SiteEditModel(Site site) : base(site)
    {
        Pages = site.Pages
            .OrderBy(p => p.SortIndex)
            .Select(p => new SelectListItem
            {
                Text = p.Text,
                Value = p.Slug,
                Selected = p.Slug == site.DefaultPage
            })
            .ToList();
    }

    public List<SelectListItem> Pages { get; set; } = [];
}