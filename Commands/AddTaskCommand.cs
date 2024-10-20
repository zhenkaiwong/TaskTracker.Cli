using TaskTracker.Cli.Constants;
using TaskTracker.Cli.DataServices;
using TaskTracker.Cli.Models;

namespace TaskTracker.Cli.Commands;

public class AddTaskCommand : BaseCommand
{
  public override bool ShouldProcess(string[] args)
  {
    var result = args.Length == 2 || args[0].Equals(CommandConstants.AddTaskName);
    _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
    return result;
  }

  public override bool TryProcess(string[] args, out string error)
  {
    bool tryGetLastTaskId(out int taskId, out string error)
    {
      if (_dataService is FileStorageJsonDataService fileStorageJsonDataService)
      {
        _logger.Debug("Data service is FileStorageJsonDataService", this);
        return fileStorageJsonDataService.TryGetLastTaskId(out taskId, out error);
      }
      else
      {
        _logger.Debug("Data service isn't FileStorageJsonDataService", this);
        var getTaskCountSuccess = _dataService.TryGetTaskCountFromDatasource(out var currentTaskCount, out error);
        if (!getTaskCountSuccess)
        {
          taskId = 0;
          return false;
        }

        taskId = currentTaskCount;
        return true;
      }
    }
    var taskToAddDescription = args[1];
    var getLastTaskIdSuccess = tryGetLastTaskId(out var lastTaskId, out error);
    if (!getLastTaskIdSuccess)
    {
      return false;
    }
    var taskID = lastTaskId + 1;
    DateTime now = DateTime.UtcNow;

    _logger.Debug($"Creating a task using following information --> ID: {taskID}, Task description: \"{taskToAddDescription}\", Status: {TaskStatusConstants.TodoStatus}, Created at: {now}, Updated at: {now}", this);
    var createTaskSuccess = Models.Task.TryCreate(taskID, taskToAddDescription, TaskStatusConstants.TodoStatus, now, now, out var task, out error);

    if (!createTaskSuccess || task is null)
    {
      return false;
    }

    _logger.Debug($"Task \"{task.Description}\" is created", this);
    var writeTaskSuccess = _dataService.TryInsertTask(task, out error);

    if (!writeTaskSuccess)
    {
      return false;
    }

    _logger.Info("Task added successfully", this);

    return true;
  }
}
