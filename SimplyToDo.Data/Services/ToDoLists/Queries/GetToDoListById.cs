using Microsoft.EntityFrameworkCore;
using SimplyToDo.Data.Models;

namespace SimplyToDo.Data.Services.ToDoLists.Queries;

public class GetToDoListById : IRequest<Result<ToDoList>>
{
    public int Id { get; set; }
}

internal class GetToDoListByIdHandler(AppDbContext dbContext, ILogger<GetToDoListByIdHandler> logger)
    : IRequestHandler<GetToDoListById, Result<ToDoList>>
{
    public async Task<Result<ToDoList>> Handle(GetToDoListById request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await dbContext.ToDoLists
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
