using System.Text;
using Microsoft.VisualBasic;
using TaskTracker.Cli.Constants;

namespace TaskTracker.Cli.Commands;

public class ListCommand : BaseCommand
{

  public override bool ShouldProcess(string[] args)
  {
    bool isLitSubCommands(string subCommand)
    {
      var result = subCommand.Equals(TaskStatusConstants.TodoStatus) || subCommand.Equals(TaskStatusConstants.InProgressStatus) || subCommand.Equals(TaskStatusConstants.DoneStatus);
      _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
      return result;
    }

    var result = (args.Length == 1 && args[0].Equals(CommandConstants.ListTaskName)) || (args.Length == 2 && args[0].Equals(CommandConstants.ListTaskName) && isLitSubCommands(args[1]));
    _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
    return result;
  }

  public override bool TryProcess(string[] args, out string error)
  {
    error = string.Empty;
    switch (args.Length)
    {
      case 1:
        return TryPrintTasks(out error);
      case 2:
        return TryPrintTasksWithStatus(args[1], out error);
      default:
        error = "Invalid CLI input. You need to spcify a command and necessary parameters.";
        return false;
    }
  }

  protected void PrintTasks(List<Models.Task> tasks)
  {

    StringBuilder sb = new StringBuilder();
    sb.AppendLine($"Printing total {tasks.Count} tasks.");
    sb.AppendLine(string.Format("{0,5}\t{1,-50}\t{2,-11}", "ID", "Description", "Status"));

    foreach (var t in tasks)
    {
      sb.AppendLine(string.Format("{0,5}\t{1,-50}\t{2,-11}", t.Id, t.Description, t.Status));
    }

    _logger.Info(sb.ToString(), this);
  }

  protected virtual bool TryPrintTasks(out string error)
  {
    var getAllTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getAllTasksSuccess)
    {
      return false;
    }

    PrintTasks(tasks);
    return true;
  }

  protected virtual bool TryPrintTasksWithStatus(string status, out string error)
  {
    var getTasksWithStatusSuccess = TryGetTasksWithStatus(status, out var tasks, out error);
    if (!getTasksWithStatusSuccess)
    {
      return false;
    }

    PrintTasks(tasks);
    return true;
  }

  protected virtual bool TryGetTasksWithStatus(string status, out List<Models.Task> tasks, out string error)
  {
    switch (status)
    {
      case TaskStatusConstants.TodoStatus:
        return TryGetAllTasksInTodoState(out tasks, out error);
      case TaskStatusConstants.InProgressStatus:
        return TryGetAllTasksInInProgressState(out tasks, out error);
      case TaskStatusConstants.DoneStatus:
        return TryGetAllTasksInDoneState(out tasks, out error);
      default:
        tasks = new List<Models.Task>();
        error = $"Status '{status}' is not a valid status. Valid statuses are '{TaskStatusConstants.TodoStatus}', '{TaskStatusConstants.InProgressStatus}' and '{TaskStatusConstants.DoneStatus}'.";
        return false;
    }
  }

  protected virtual bool TryGetAllTasks(out List<Models.Task> tasks, out string error)
  {
    return _dataService.TryGetAllTasks(out tasks, out error);
  }

  protected virtual bool TryGetAllTasksInTodoState(out List<Models.Task> todoTasks, out string error)
  {
    todoTasks = new List<Models.Task>();
    var getAllTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getAllTasksSuccess)
    {
      return false;
    }

    todoTasks.AddRange(tasks.Where(t => t.Status.Equals(TaskStatusConstants.TodoStatus)));
    return true;
  }

  protected virtual bool TryGetAllTasksInInProgressState(out List<Models.Task> inProgressTasks, out string error)
  {
    inProgressTasks = new List<Models.Task>();
    var getAllTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getAllTasksSuccess)
    {
      return false;
    }

    inProgressTasks.AddRange(tasks.Where(t => t.Status.Equals(TaskStatusConstants.InProgressStatus)));
    return true;
  }
  protected virtual bool TryGetAllTasksInDoneState(out List<Models.Task> doneTasks, out string error)
  {
    doneTasks = new List<Models.Task>();
    var getAllTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getAllTasksSuccess)
    {
      return false;
    }

    doneTasks.AddRange(tasks.Where(t => t.Status.Equals(TaskStatusConstants.DoneStatus)));
    return true;
  }
}
