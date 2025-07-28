using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Garage.Entities;

namespace Garage.Models;

public class PageModel: ISortable
{
    public PageModel() { }

    public PageModel(SitePage page)
    {
        PageId= page.Id;
        Slug = page.Slug;
        Text = page.Text;
        SortIndex = page.SortIndex;
    }

    [Required]
    public Guid PageId { get; set; } = Guid.Empty;

    [Required, StringLength(128, MinimumLength = 2)]
    public string Slug { get; set; } = string.Empty;

    [DisplayName("Page Name")]
    [Required, StringLength(128, MinimumLength = 2)]
    public string Text { get; set; } = string.Empty;

    [Required, Range(1, 200)]
    public int SortIndex { get; set; } = 0;
}