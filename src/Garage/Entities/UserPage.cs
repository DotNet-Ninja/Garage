namespace Garage.Entities;

public class UserPage: Entity
{
    public string UserId { get; set; } = Guid.Empty.ToString();
    public string Slug { get; set; } = string.Empty;
    public List<BookmarkGroup> Groups { get; set; } = [];
}