using Garage.Entities;

namespace Garage.Models;

public class StartPageModel
{
    public StartPageModel()
    {
    }

    public StartPageModel(UserPage page)
    {
        if (page == null) throw new ArgumentNullException(nameof(page));
        Text = page.Text ?? "Garage";
        Groups = page.Groups
            .OrderBy(g => g.SortIndex)
            .Select(g => new StartGroupModel
            {
                Text = g.Text,
                SortIndex = g.SortIndex,
                Links = g.Bookmarks?
                    .OrderBy(l => l.SortIndex)
                    .Select(l => new StartLinkModel
                    {
                        Text = l.Text,
                        Url = l.Href,
                        Icon = l.Icon,
                        IconColor = l.IconColor,
                        SortIndex = l.SortIndex
                    })
                    .ToList() ?? new List<StartLinkModel>()
            })
            .ToList();
    }

    public string Text { get; set; } = "Garage";
    public List<StartGroupModel> Groups { get; set; } = new();
}