using Garage.Constants;

namespace Garage.Entities;

public class Bookmark: Entity
{
    public string Href { get; set; } = string.Empty;
    public BootstrapIcons Icon { get; set; } = BootstrapIcons.Bookmark;
    public string IconColor { get; set; } = Defaults.Colors.IconColor;
    public bool OpenInNewTab { get; set; } = true;
}