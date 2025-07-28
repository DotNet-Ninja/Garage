using System.ComponentModel.DataAnnotations;
using Garage.Entities;

namespace Garage.Models;

public class GroupModel: ISortable
{
    public GroupModel()
    {
    }

    public GroupModel(BookmarkGroup group)
    {
        GroupId = group.Id;
        Text = group.Text;
        SortIndex = group.SortIndex;
    }

    [Required]
    public Guid GroupId { get; set; }

    [Required, StringLength(127, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    [Required, Range(1, 200)]
    public int SortIndex { get; set; }
}