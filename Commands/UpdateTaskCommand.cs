using TaskTracker.Cli.Constants;

namespace TaskTracker.Cli.Commands;

public class UpdateTaskCommand : BaseCommand
{
  public override bool ShouldProcess(string[] args)
  {
    var result = args.Length == 3 && args[0].Equals(CommandConstants.UpdateTaskName) && IsTaskId(args[1]);
    _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
    return result;
  }

  public override bool TryProcess(string[] args, out string error)
  {
    var taskId = int.Parse(args[1]);
    var newTaskDescription = args[2];
    _logger.Debug($"Updating task. ID: {taskId}, Description: {newTaskDescription}", this);
    var getTaskSuccess = _dataService.TryGetTask(taskId, out var task, out error);

    if (!getTaskSuccess || task is null)
    {
      return false;
    }

    task.Description = newTaskDescription;
    task.UpdatedAt = DateTime.Now;

    var updateTaskSuccess = _dataService.TryUpdateTask(task, out error);
    if (!updateTaskSuccess)
    {
      return false;
    }

    _logger.Info($"Task {taskId} is updated successfully", this);
    return true;
  }
}
