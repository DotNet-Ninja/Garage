using Garage.Constants;

namespace Garage.Models;

public class NavigationItem
{
    public BootstrapIcons Icon { get; set; } = BootstrapIcons.NotSet;
    public string Text { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public List<NavigationItem> Items { get; set; } = new();
}