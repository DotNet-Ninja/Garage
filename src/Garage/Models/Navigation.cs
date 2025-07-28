using DotNetNinja.AutoBoundConfiguration;
using Garage.Constants;

namespace Garage.Models;

[AutoBind("Navigation")]
public class Navigation
{
    public string BrandName { get; set; } = Defaults.ApplicationName;
    public List<NavigationItem> Items { get; set; } = new();

    public static Navigation Admin => new Navigation
    {
        Items = new List<NavigationItem>
        {
            new NavigationItem
            {
                Icon = BootstrapIcons.House,
                Text = string.Empty,
                Url = "/"
            },
            new NavigationItem
            {
                Icon = BootstrapIcons.NotSet,
                Text = "Sites",
                Url = "/Sites"
            }
        }
    };
}