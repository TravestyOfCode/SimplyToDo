namespace SimplyToDo.Data.Models;

public class ToDoTask
{
    public int Id { get; set; }

    public int ToDoListId { get; set; }

    // A custom user task status
    public string? TaskStatus { get; set; }

    // A user settable background color in HTML Hex format
    public string? BackgroundColor { get; set; }

    // A user settable foreground color in HTML Hex format
    public string? ForegroundColor { get; set; }

    public bool IsCompleted { get; set; }

    public required string Description { get; set; }

    public DateOnly? DueByDate { get; set; }

    public TimeOnly? DueByTime { get; set; }
}

internal static class ToDoTaskExtensions
{
    public static ToDoTask AsEntity(this Entities.ToDoTask source) =>
        new ToDoTask()
        {
            Id = source.Id,
            ToDoListId = source.ToDoListId,
            TaskStatus = source.TaskStatus,
            BackgroundColor = source.BackgroundColor,
            ForegroundColor = source.ForegroundColor,
            IsCompleted = source.IsCompleted,
            Description = source.Description,
            DueByDate = source.DueByDate,
            DueByTime = source.DueByTime
        };

    public static IQueryable<ToDoTask> AsEntity(this IQueryable<Entities.ToDoTask> source) => source.Select(t => t.AsEntity());
}
