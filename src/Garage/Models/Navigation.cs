using DotNetNinja.AutoBoundConfiguration;

namespace Garage.Models;

[AutoBind("Navigation")]
public class Navigation
{
    public List<NavigationItem> Items { get; set; } = new();
}