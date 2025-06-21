namespace SimplyToDo.Data.Models;

public class ToDoList
{
    public int Id { get; set; }
    public required string OwnerId { get; set; }
    public int Ordinal { get; set; }
    public required string Title { get; set; }
    public List<ToDoItem>? Items { get; set; }
}

internal static class ToDoListExtensions
{
    public static IQueryable<ToDoList> ProjectToModel(this IQueryable<Entities.ToDoList> source) =>
        source.Select(p => new ToDoList()
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Ordinal = p.Ordinal,
            Title = p.Title,
            Items = p.Items == null ? null : p.Items.Select(i => i.AsModel()).ToList()
        });

    public static ToDoList AsModel(this Entities.ToDoList source) =>
        new ToDoList()
        {
            Id = source.Id,
            OwnerId = source.OwnerId,
            Ordinal = source.Ordinal,
            Title = source.Title,
            Items = source.Items?.Select(i => i.AsModel()).ToList()
        };
}