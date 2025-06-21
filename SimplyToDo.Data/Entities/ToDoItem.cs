namespace SimplyToDo.Data.Entities;

internal class ToDoItem
{
    public int Id { get; set; }
    public int ToDoListId { get; set; }
    public ToDoList? ToDoList { get; set; }
    public int Ordinal { get; set; }
    public bool IsCompleted { get; set; }
    public required string Description { get; set; }
    public DateOnly? DueDate { get; set; }
    public TimeOnly? DueTime { get; set; }
}
