using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimplyToDo.Data.Entities;
using SimplyToDo.Data.Models;
using SimplyToDo.Data.Models.ToDoTasks;

namespace SimplyToDo.Data.Services.ToDoTasks;

internal class UpdateToDoTaskService(AppDbContext _dbContext, PropertyValidator _propValidator, IUserAccessor _userAccessor, ILogger<UpdateToDoTaskService> _logger) : IUpdateToDoTaskService
{
    public async Task<Result<ToDoTask>> Save(UpdateToDoTask request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the request properties
            var propResults = _propValidator.ValidateModel(request);
            if (propResults.Any())
            {
                return Result<ToDoTask>.Error(propResults);
            }

            // Get the current user so we can check if they have permission to update task.
            var currentUser = _userAccessor.GetCurrentUser();
            if (currentUser == null)
            {
                return Result<ToDoTask>.Error("There was an error getting the current logged in user.");
            }

            // Ensure the original entity exists
            var entity = await _dbContext.ToDoTasks
                .Include(t => t.ToDoList)
                .SingleOrDefaultAsync(t => t.Id.Equals(request.Id), cancellationToken);

            if (entity == null)
            {
                return Result<ToDoTask>.Error("The requested task was not found.");
            }

            // Check that the current user is allowed to make the requested changes
            // For now, we only have to make sure the associated list is owned by the user.
            // Later, a task may be shared with other users, which will be a separate table lookup.
            if (!currentUser.Equals(entity.ToDoList?.OwnerId))
            {
                return Result<ToDoTask>.Error("You are not allowed to make changes to this task.");
            }

            // Everything should be good, try to update
            request.MapTo(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<ToDoTask>.Ok(entity.AsEntity());

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating task {request}", request);
            return Result<ToDoTask>.Error("There was an unexpected error updating the task.");
        }
    }
}
