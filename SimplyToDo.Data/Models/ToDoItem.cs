namespace SimplyToDo.Data.Models;

public class ToDoItem
{
    public int Id { get; set; }
    public int ToDoListId { get; set; }
    public int Ordinal { get; set; }
    public bool IsCompleted { get; set; }
    public required string Description { get; set; }
    public DateOnly? DueDate { get; set; }
    public TimeOnly? DueTime { get; set; }
}

internal static class ToDoItemExtensions
{
    public static IQueryable<ToDoItem> ProjectToModel(this IQueryable<Entities.ToDoItem> source) =>
        source.Select(p => new ToDoItem()
        {
            Id = p.Id,
            ToDoListId = p.ToDoListId,
            Ordinal = p.Ordinal,
            IsCompleted = p.IsCompleted,
            Description = p.Description,
            DueDate = p.DueDate,
            DueTime = p.DueTime
        });

    public static ToDoItem AsModel(this Entities.ToDoItem source) =>
         new ToDoItem()
         {
             Id = source.Id,
             ToDoListId = source.ToDoListId,
             Ordinal = source.Ordinal,
             IsCompleted = source.IsCompleted,
             Description = source.Description,
             DueDate = source.DueDate,
             DueTime = source.DueTime
         };
}
