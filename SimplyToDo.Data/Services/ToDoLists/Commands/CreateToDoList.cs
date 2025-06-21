using SimplyToDo.Data.Models;

namespace SimplyToDo.Data.Services.ToDoLists.Commands;

public class CreateToDoList : IRequest<Result<ToDoList>>
{
    public required string OwnerId { get; set; }
    public int Ordinal { get; set; }
    public required string Title { get; set; }
}

internal class CreateToDoListHandler(AppDbContext dbContext, ILogger<CreateToDoListHandler> logger)
    : IRequestHandler<CreateToDoList, Result<ToDoList>>
{
    public async Task<Result<ToDoList>> Handle(CreateToDoList request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = dbContext.ToDoLists.Add(new Entities.ToDoList()
            {
                OwnerId = request.OwnerId,
                Ordinal = request.Ordinal,
                Title = request.Title
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