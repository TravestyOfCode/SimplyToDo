using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimplyToDo.Data.Models;
using SimplyToDo.Data.Models.ToDoTasks;

namespace SimplyToDo.Data.Services.ToDoTasks;

internal class UpdateToDoTaskService(Entities.AppDbContext _dbContext, PropertyValidator _propValidator, IUserAccessor _userAccessor, ILogger<UpdateToDoTaskService> _logger) : IUpdateToDoTaskService
{
    // TODO: Is this function doing too much? Should there be separate services for property validation,
    // rules validation, and business logic? How best to implement and allow passing context so we don't
    // query the DB multiple times?
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

            // If the current user is not the owner, check if they have shared permission to perform the change.
            // TODO: Is there a better way to check these permissions? Worry that if future properties get added,
            // we may miss check if they have changed.
            if (!currentUser.Equals(entity.ToDoList?.OwnerId))
            {
                var shared = await _dbContext.SharedToDoTasks
                    .Where(s => s.SharedUserId.Equals(currentUser) && s.ToDoTaskId.Equals(request.Id))
                    .SingleOrDefaultAsync(cancellationToken);

                if (shared != null)
                {
                    // If the user is not allowed to change the IsCompleted, check if it's being changed.
                    if (!shared.CanComplete)
                    {
                        if (request.IsCompleted != entity.IsCompleted)
                        {
                            return Result<ToDoTask>.Error("You are not allowed to make changes to this task.");
                        }
                    }
                    // Check if anything other than IsCompleted is being changed.
                    if (!shared.CanEdit)
                    {
                        // TODO: Perhaps add an extension method that takes an entity and returns true or false?
                        if (request.TaskStatus != entity.TaskStatus || request.ToDoListId != entity.ToDoListId ||
                            request.ForegroundColor != entity.ForegroundColor || request.BackgroundColor != entity.BackgroundColor ||
                            request.Description != entity.Description || request.DueByDate != entity.DueByDate ||
                            request.DueByTime != entity.DueByTime)
                        {
                            return Result<ToDoTask>.Error("You are not allowed to make changes to this task.");
                        }
                    }
                }
                else // There is no share with the current user.
                {
                    return Result<ToDoTask>.Error("You are not allowed to make changes to this task.");
                }
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
