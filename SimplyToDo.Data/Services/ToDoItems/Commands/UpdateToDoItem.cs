using Microsoft.EntityFrameworkCore;
using SimplyToDo.Data.Models;

namespace SimplyToDo.Data.Services.ToDoItems.Commands;

public class UpdateToDoItem : IRequest<Result<ToDoItem>>
{
    public int Id { get; set; }
    public int ToDoListId { get; set; }
    public int Ordinal { get; set; }
    public bool IsCompleted { get; set; }
    public required string Description { get; set; }
    public DateOnly? DueDate { get; set; }
    public TimeOnly? DueTime { get; set; }
}

internal class UpdateToDoItemHandler(AppDbContext dbContext, ILogger<UpdateToDoItemHandler> logger)
    : IRequestHandler<UpdateToDoItem, Result<ToDoItem>>
{
    public async Task<Result<ToDoItem>> Handle(UpdateToDoItem request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await dbContext.ToDoItems.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Error.NotFound();
            }

            entity.ToDoListId = request.ToDoListId;
            entity.Ordinal = request.Ordinal;
            entity.IsCompleted = request.IsCompleted;
            entity.Description = request.Description;
            entity.DueDate = request.DueDate;
            entity.DueTime = request.DueTime;

            await dbContext.SaveChangesAsync(cancellationToken);

            return entity.AsModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error.");

            return Error.ServerError();
        }
    }
}