using Microsoft.EntityFrameworkCore;
using SimplyToDo.Data.Models;

namespace SimplyToDo.Data.Services.ToDoItems.Queries;

public class GetToDoItemsByToDoListId : IRequest<Result<List<ToDoItem>>>
{
    public int ToDoListId { get; set; }
}

internal class GetToDoItemsByToDoListIdHandler(AppDbContext dbContext, ILogger<GetToDoItemsByToDoListIdHandler> logger)
    : IRequestHandler<GetToDoItemsByToDoListId, Result<List<ToDoItem>>>
{
    public async Task<Result<List<ToDoItem>>> Handle(GetToDoItemsByToDoListId request, CancellationToken cancellationToken)
    {
        try
        {
            return await dbContext.ToDoItems
                .Where(p => p.ToDoListId.Equals(request.ToDoListId))
                .ProjectToModel()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error.");

            return Error.ServerError();
        }
    }
}