using SimplyToDo.Data.Models;

namespace SimplyToDo.Data.Services.ToDoItems.Commands;

public class CreateToDoItem : IRequest<Result<ToDoItem>>
{
    public int ToDoListId { get; set; }
    public int Ordinal { get; set; }
    public bool IsCompleted { get; set; }
    public required string Description { get; set; }
    public DateOnly? DueDate { get; set; }
    public TimeOnly? DueTime { get; set; }
}

internal class CreateToDoItemHandler(AppDbContext dbContext, ILogger<CreateToDoItemHandler> logger)
    : IRequestHandler<CreateToDoItem, Result<ToDoItem>>
{
    public async Task<Result<ToDoItem>> Handle(CreateToDoItem request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = dbContext.ToDoItems.Add(new Entities.ToDoItem()
            {
                ToDoListId = request.ToDoListId,
                Ordinal = request.Ordinal,
                IsCompleted = request.IsCompleted,
                Description = request.Description,
                DueDate = request.DueDate,
                DueTime = request.DueTime
            });

            await dbContext.SaveChangesAsync(cancellationToken);

            return entity.Entity.AsModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error.");

            return Error.ServerError();
        }
    }
}