using System.ComponentModel.DataAnnotations;

namespace Garage.Models;

public class BookmarkIndexModel
{
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

    public List<BookmarkModel> Bookmarks { get; set; } = new List<BookmarkModel>();

}