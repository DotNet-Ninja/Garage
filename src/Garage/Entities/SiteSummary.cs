using Garage.Constants;

namespace Garage.Entities;

public class SiteSummary : Entity
{
    public string Slug { get; set; } = Defaults.Sites.Slug;

    public string DefaultPage { get; set; } = string.Empty;
}