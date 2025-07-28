namespace Garage.Entities;

public abstract class Entity: ISortable
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Text { get; set; } = string.Empty;
    public int SortIndex { get; set; } = 0;
}