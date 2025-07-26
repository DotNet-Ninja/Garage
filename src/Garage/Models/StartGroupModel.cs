namespace Garage.Models;

public class StartGroupModel
{
    public string Text { get; set; } = string.Empty;
    public int SortIndex { get; set; } = 0;
    public List<StartLinkModel> Links { get; set; } = new();
}