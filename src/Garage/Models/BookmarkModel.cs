using System.ComponentModel;
using Garage.Constants;
using System.ComponentModel.DataAnnotations;
using Garage.Entities;

namespace Garage.Models;

public class BookmarkModel
{
    public BookmarkModel()
    {
    }

    public BookmarkModel(Bookmark link)
    {
        BookmarkId = link.Id;
        Text = link.Text;
        SortIndex = link.SortIndex;
        Href = link.Href;
        Icon = link.Icon;
        IconColor = link.IconColor;
        OpenInNewTab = link.OpenInNewTab;
    }

    [Required]
    public Guid BookmarkId { get; set; } = Guid.Empty;

    [Required, StringLength(128, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    [Required, Range(0, int.MaxValue)]
    [DisplayName("Sort Index")]
    public int SortIndex { get; set; } = 0;

    [Required, Url, StringLength(1024, MinimumLength = 1)]
    public string Href { get; set; } = string.Empty;

    [Required]
    public BootstrapIcons Icon { get; set; } = BootstrapIcons.Bookmark;

    [Required, StringLength(7, MinimumLength = 4)]
    public string IconColor { get; set; } = Defaults.Colors.IconColor;

    [DisplayName("Open In New Tab")]
    public bool OpenInNewTab { get; set; } = true;

    public string IconStyle => $"color: {IconColor};";
}