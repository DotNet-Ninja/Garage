using System.ComponentModel.DataAnnotations;

namespace Garage.Models;

public class BookmarkDeleteModel
{
    [Required]
    public string SiteSlug { get; set; } = string.Empty;

    [Required]
    public string PageSlug { get; set; } = string.Empty;

    [Required]
    public string PageText { get; set; } = string.Empty;

    [Required]
    public string SiteText { get; set; } = string.Empty;

    [Required]
    public Guid GroupId { get; set; } = Guid.Empty;

    [Required]
    public string GroupText { get; set; } = string.Empty;

    [Required]
    public Guid BookmarkId { get; set; } = Guid.Empty;

    [Required]
    public string BookmarkText { get; set; } = string.Empty;
}