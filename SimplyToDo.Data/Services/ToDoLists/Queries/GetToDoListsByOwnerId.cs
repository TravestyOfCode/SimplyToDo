using Microsoft.EntityFrameworkCore;
using SimplyToDo.Data.Models;

namespace SimplyToDo.Data.Services.ToDoLists.Queries;

public class GetToDoListsByOwnerId : IRequest<Result<List<ToDoList>>>
{
    public required string OwnerId { get; set; }
}

internal class GetToDoListsByOwnerIdHandler(AppDbContext dbContext, ILogger<GetToDoListsByOwnerIdHandler> logger)
    : IRequestHandler<GetToDoListsByOwnerId, Result<List<ToDoList>>>
{
    public async Task<Result<List<ToDoList>>> Handle(GetToDoListsByOwnerId request, CancellationToken cancellationToken)
    {
        try
        {
            return await dbContext.ToDoLists
                .Where(p => p.OwnerId.Equals(request.OwnerId))
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