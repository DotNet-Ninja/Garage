namespace Garage.Models;

public class SiteCopyModel
{
    public string Text { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int SortIndex { get; set; } = 0;
    public string SourceSlug { get; set; } = string.Empty;
    public string SourceText { get; set; } = string.Empty;
}