using Microsoft.EntityFrameworkCore;
using SimplyToDo.Data.Models;

namespace SimplyToDo.Data.Services.ToDoLists.Commands;

public class UpdateToDoList : IRequest<Result<ToDoList>>
{
    public int Id { get; set; }
    public int Ordinal { get; set; }
    public required string Title { get; set; }
}

internal class UpdateToDoListHandler(AppDbContext dbContext, ILogger<UpdateToDoListHandler> logger)
    : IRequestHandler<UpdateToDoList, Result<ToDoList>>
{
    public async Task<Result<ToDoList>> Handle(UpdateToDoList request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await dbContext.ToDoLists.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Error.NotFound();
            }

            entity.Ordinal = request.Ordinal;
            entity.Title = request.Title;

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