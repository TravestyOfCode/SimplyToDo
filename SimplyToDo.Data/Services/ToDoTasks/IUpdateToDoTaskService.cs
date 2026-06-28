using SimplyToDo.Data.Models;
using SimplyToDo.Data.Models.ToDoTasks;

namespace SimplyToDo.Data.Services.ToDoTasks;

public interface IUpdateToDoTaskService
{
    public Task<Result<ToDoTask>> Save(UpdateToDoTask request, CancellationToken cancellationToken);
}
