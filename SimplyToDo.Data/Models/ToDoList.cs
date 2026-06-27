namespace SimplyToDo.Data.Models;

public class ToDoList
{
    public int Id { get; set; }

    public required string OwnerId { get; set; }

    // A user settable background color in HTML Hex format
    public string? BackgroundColor { get; set; }

    // A user settable foreground color in HTML Hex format
    public string? ForegroundColor { get; set; }

    // Used to allow the user to hide fully completed lists.
    public bool IsCompleted { get; set; }
}

internal static class ToDoListExtensions
{
    public static ToDoList AsEntity(this Entities.ToDoList source) =>
        new ToDoList()
        {
            Id = source.Id,
            OwnerId = source.OwnerId,
            BackgroundColor = source.BackgroundColor,
            ForegroundColor = source.ForegroundColor,
            IsCompleted = source.IsCompleted
        };

    public static IQueryable<ToDoList> AsEntity(this IQueryable<Entities.ToDoList> source) => source.Select(l => l.AsEntity());
}