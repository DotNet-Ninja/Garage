using Garage.Constants;

namespace Garage.Models;

public class StartLinkModel
{
    public int SortIndex { get; set; } = 0;
    public string Text { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public BootstrapIcons Icon { get; set; } = BootstrapIcons.NotSet;
    public string IconColor { get; set; } = string.Empty;

    public string IconStyle => $"color: {IconColor};";
}