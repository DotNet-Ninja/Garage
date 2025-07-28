using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Garage.Constants;
using Garage.Entities;

namespace Garage.Models;

public class SiteModel
{
    public SiteModel(){}

    public SiteModel(SiteSummary site)
    {
        SiteId=site.Id;
        Text = site.Text;
        SortIndex = site.SortIndex;
        Slug = site.Slug;
        DefaultPage = site.DefaultPage;
    }

    [Required]
    public Guid SiteId { get; set; } = Guid.Empty;

    [DisplayName("Site Name")]
    [Required, StringLength(128, MinimumLength = 2)]
    public string Text { get; set; } = string.Empty;


    [DisplayName("Sort Index")]
    [Required, Range(1, 200)]
    public int SortIndex { get; set; } = 0;

    [Required, StringLength(64, MinimumLength = 2)]
    public string Slug { get; set; } = Defaults.Sites.Slug;


    [DisplayName("Default Page")]
    [Required, StringLength(128, MinimumLength = 2)]
    public string DefaultPage { get; set; } = string.Empty;
}