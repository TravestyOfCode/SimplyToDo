using Microsoft.EntityFrameworkCore;
using SimplyToDo.Data.Models;

namespace SimplyToDo.Data.Services.ToDoItems.Queries;

public class GetToDoItemById : IRequest<Result<ToDoItem>>
{
    public int Id { get; set; }
}

internal class GetToDoItemByIdHandler(AppDbContext dbContext, ILogger<GetToDoItemByIdHandler> logger)
    : IRequestHandler<GetToDoItemById, Result<ToDoItem>>
{
    public async Task<Result<ToDoItem>> Handle(GetToDoItemById request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await dbContext.ToDoItems
                .Where(p => p.Id.Equals(request.Id))
                .ProjectToModel()
                .SingleOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                return Error.NotFound();
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error.");

            return Error.ServerError();
        }
    }
}