using Microsoft.EntityFrameworkCore;

namespace SimplyToDo.Data.Services.ToDoItems.Commands;

public class DeleteToDoItem : IRequest<Result>
{
    public int Id { get; set; }
}

internal class DeleteToDoItemHandler(AppDbContext dbContext, ILogger<DeleteToDoItemHandler> logger)
    : IRequestHandler<DeleteToDoItem, Result>
{
    public async Task<Result> Handle(DeleteToDoItem request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await dbContext.ToDoItems.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Error.NotFound();
            }

            dbContext.ToDoItems.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error.");

            return Error.ServerError();
        }
    }
}