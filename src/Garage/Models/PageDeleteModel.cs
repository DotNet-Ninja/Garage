namespace Garage.Models;

public class PageDeleteModel
{
    public string SiteSlug { get; set; } = string.Empty;
    public string PageSlug { get; set; } = string.Empty;
    public string PageText { get; set; } = string.Empty;
    public string SiteText { get; set; } = string.Empty;
}