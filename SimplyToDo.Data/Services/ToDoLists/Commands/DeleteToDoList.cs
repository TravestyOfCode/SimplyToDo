using Microsoft.EntityFrameworkCore;

namespace SimplyToDo.Data.Services.ToDoLists.Commands;

public class DeleteToDoList : IRequest<Result>
{
    public int Id { get; set; }
}

internal class DeleteToDoListHandler(AppDbContext dbContext, ILogger<DeleteToDoListHandler> logger)
    : IRequestHandler<DeleteToDoList, Result>
{
    public async Task<Result> Handle(DeleteToDoList request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await dbContext.ToDoLists.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Error.NotFound();
            }

            dbContext.ToDoLists.Remove(entity);

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