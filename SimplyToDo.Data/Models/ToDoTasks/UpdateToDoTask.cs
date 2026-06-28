using System.ComponentModel.DataAnnotations;

namespace SimplyToDo.Data.Models.ToDoTasks;

public class UpdateToDoTask
{
    public int Id { get; set; }

    public int ToDoListId { get; set; }

    [MaxLength(32)]
    public string? TaskStatus { get; set; }

    [MaxLength(9)]
    public string? BackgroundColor { get; set; }

    [MaxLength(9)]
    public string? ForegroundColor { get; set; }

    public bool IsCompleted { get; set; }

    [Required]
    public required string Description { get; set; }

    public DateOnly? DueByDate { get; set; }

    public TimeOnly? DueByTime { get; set; }
}

internal static class UpdateToDoTaskExtensions
{
    public static void MapTo(this UpdateToDoTask source, Entities.ToDoTask destination)
    {
        destination.Id = source.Id;
        destination.ToDoListId = source.ToDoListId;
        destination.TaskStatus = source.TaskStatus;
        destination.BackgroundColor = source.BackgroundColor;
        destination.ForegroundColor = source.ForegroundColor;
        destination.IsCompleted = source.IsCompleted;
        destination.Description = source.Description;
        destination.DueByDate = source.DueByDate;
        destination.DueByTime = source.DueByTime;
    }
}