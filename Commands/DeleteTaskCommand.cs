using TaskTracker.Cli.Constants;

namespace TaskTracker.Cli.Commands;

public class DeleteTaskCommand : BaseCommand
{
  public override bool ShouldProcess(string[] args)
  {
    var result = args.Length == 2 && args[0].Equals(CommandConstants.DeleteTaskName) && IsTaskId(args[1]);
    _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
    return result;
  }

  public override bool TryProcess(string[] args, out string error)
  {
    var taskId = int.Parse(args[1]);
    _logger.Debug($"Deleting task. ID: {taskId}", this);
    var deleteTaskSuccess = _dataService.TryDeleteTask(taskId, out error);
    if (!deleteTaskSuccess)
    {
      return false;
    }
    _logger.Info($"Task {taskId} is deleted successfully", this);
    return true;
  }
}
