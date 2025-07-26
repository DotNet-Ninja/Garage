using Garage.Entities;

namespace Garage.Constants;

public static class Data
{
    public static readonly List<UserPage> AnonymousDefaults =
    [
        new UserPage
        {
            Id = Guid.Parse("ec95764c-a8a6-402c-92d3-311e57b8d9f8"),
            Slug = "start",
            Text = "Start",
            SortIndex = 1,
            UserId = Defaults.Users.UserId,
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
                            Icon = BootstrapIcons.PinMap,
                            IconColor = Defaults.Colors.IconColor
                        },

                        new Bookmark
                        {
                            SortIndex = 2,
                            Id = Guid.Parse("cc44a305-a010-4dae-a9e9-7ee5b234ac5c"),
                            Text = "Weather.com",
                            Href = "https://www.weather.com/",
                            Icon = BootstrapIcons.PinMap,
                            IconColor = Defaults.Colors.IconColor
                        },

                        new Bookmark
                        {
                            SortIndex = 3,
                            Id = Guid.Parse("0ac1532f-fa36-4958-a759-48cba476dc4a"),
                            Text = "Accuweather",
                            Href = "https://www.accuweather.com/",
                            Icon = BootstrapIcons.PinMap,
                            IconColor = Defaults.Colors.IconColor
                        },

                        new Bookmark
                        {
                            SortIndex = 4,
                            Id = Guid.Parse("1c258596-570b-4bd3-a66d-18f97ed700f5"),
                            Text = "Weather Underground",
                            Href = "https://www.wunderground.com/",
                            Icon = BootstrapIcons.PinMap,
                            IconColor = Defaults.Colors.IconColor
                        },

                        new Bookmark
                        {
                            SortIndex = 5,
                            Id = Guid.Parse("22b259ce-d0cb-49f3-a86f-6d6caf2ba2ea"),
                            Text = "Lightning Maps",
                            Href = "https://www.lightningmaps.org/",
                            Icon = BootstrapIcons.PinMap,
                            IconColor = Defaults.Colors.IconColor
                        }
                    ]
                }
            ]
        }
    ];
}