using Garage.Constants;

namespace Garage.Entities;

public class Site: SiteSummary
{
    public List<SitePage> Pages { get; set; } = [];

    public static readonly Site Default = new Site
    {
        Pages =
        [
            new SitePage
            {
                Id = Guid.Parse("ec95764c-a8a6-402c-92d3-311e57b8d9f8"),
                Slug = "start",
                Text = "Start",
                SortIndex = 1,
                Groups =
                [
                    new BookmarkGroup
                    {
                        Id = Guid.Parse("3cc59094-b739-4dd7-af18-3085524b1256"),
                        Text = "Maps & Weather",
                        SortIndex = 1,
                        Bookmarks =
                        [
                            new Bookmark
                            {
                                SortIndex = 1,
                                Id = Guid.Parse("ecc179cc-4892-4e08-a838-69f6eb44f7c1"),
                                Text = "Google Maps",
                                Href = "https://www.google.com/maps",
                                Icon = BootstrapIcons.GeoAltFill,
                                IconColor = "#cc0000"
                            },

                            new Bookmark
                            {
                                SortIndex = 2,
                                Id = Guid.Parse("cc44a305-a010-4dae-a9e9-7ee5b234ac5c"),
                                Text = "Weather.com",
                                Href = "https://www.weather.com/",
                                Icon = BootstrapIcons.CloudSunFill,
                                IconColor = Defaults.Colors.IconColor
                            },

                            new Bookmark
                            {
                                SortIndex = 3,
                                Id = Guid.Parse("0ac1532f-fa36-4958-a759-48cba476dc4a"),
                                Text = "Accuweather",
                                Href = "https://www.accuweather.com/",
                                Icon = BootstrapIcons.CloudSunFill,
                                IconColor = Defaults.Colors.IconColor
                            },

                            new Bookmark
                            {
                                SortIndex = 4,
                                Id = Guid.Parse("1c258596-570b-4bd3-a66d-18f97ed700f5"),
                                Text = "Weather Underground",
                                Href = "https://www.wunderground.com/",
                                Icon = BootstrapIcons.CloudSunFill,
                                IconColor = Defaults.Colors.IconColor
                            },

                            new Bookmark
                            {
                                SortIndex = 5,
                                Id = Guid.Parse("22b259ce-d0cb-49f3-a86f-6d6caf2ba2ea"),
                                Text = "Lightning Maps",
                                Href = "https://www.lightningmaps.org/",
                                Icon = BootstrapIcons.CloudLightning,
                                IconColor = Defaults.Colors.IconColor
                            }
                        ]
                    }
                ]
            }
        ]
    };

    public void RegenerateIds()
    {
        this.Id = Guid.NewGuid();
        foreach(var page in this.Pages)
        {
            page.Id = Guid.NewGuid();
            foreach (var group in page.Groups)
            {
                group.Id = Guid.NewGuid();
                foreach (var bookmark in group.Bookmarks)
                {
                    bookmark.Id = Guid.NewGuid();
                }
            }
        }
    }

    public Site ToCopy(string text, string slug, int sortIndex)
    {
        var copy = new Site
        {
            Id = Guid.NewGuid(),
            Text = text,
            Slug = slug,
            SortIndex = sortIndex,
            DefaultPage = this.DefaultPage,
            Pages = this.Pages.Select(p => new SitePage
            {
                Id = Guid.NewGuid(),
                Text = p.Text,
                Slug = p.Slug,
                SortIndex = p.SortIndex,
                Groups = p.Groups.Select(g => new BookmarkGroup
                {
                    Id = Guid.NewGuid(),
                    Text = g.Text,
                    SortIndex = g.SortIndex,
                    Bookmarks = g.Bookmarks.Select(b => new Bookmark
                    {
                        Id = Guid.NewGuid(),
                        Text = b.Text,
                        Href = b.Href,
                        Icon = b.Icon,
                        IconColor = b.IconColor,
                        OpenInNewTab = b.OpenInNewTab,
                        SortIndex = b.SortIndex
                    }).ToList()
                }).ToList()
            }).ToList()
        };
        return copy;
    }
}