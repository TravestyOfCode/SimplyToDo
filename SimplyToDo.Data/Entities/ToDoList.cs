namespace SimplyToDo.Data.Entities;

internal class ToDoList
{
    public int Id { get; set; }
    public required string OwnerId { get; set; }
    public AppUser? Owner { get; set; }
    public int Ordinal { get; set; }
    public required string Title { get; set; }
    public List<ToDoItem>? Items { get; set; }
}
