namespace Garage.Entities;

public class SitePage: Entity
{
    public string Slug { get; set; } = string.Empty;
    public List<BookmarkGroup> Groups { get; set; } = [];

    public static SitePage Default => new SitePage
    {
        Id = Guid.NewGuid(),
        Text = "Default",
        SortIndex = 1,
        Slug = "default",
        Groups = []
    };
}