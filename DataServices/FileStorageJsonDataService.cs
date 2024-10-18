using System.Diagnostics;
using System.Text.Json;
using TaskTracker.Cli.Constants;
using TaskTracker.Cli.Logger;
using TaskTracker.Cli.Models;

namespace TaskTracker.Cli.DataServices;

public class FileStorageJsonDataService : IDataService
{
  protected readonly BaseLogger _logger = new ConsoleLogger();
  protected readonly string _fileName = "data.json";
  protected List<Models.Task> _tasks = new List<Models.Task>();


  /// <summary>
  /// Checks if the datasource file exists.
  /// </summary>
  protected bool IsDatasourceExist()
  {
    var fileExist = File.Exists(_fileName);
    _logger.Debug($"Checking if datasource exist. Result: {fileExist}", this);
    return fileExist;
  }

  /// <summary>
  /// Attempts to write the list of tasks to the datasource file.
  /// </summary>
  /// <param name="tasks">The list of tasks to be serialized and written to the datasource.</param>
  /// <param name="error">Outputs the error message if writing to the datasource fails.</param>
  /// <returns>True if the tasks are successfully written to the datasource, otherwise false.</returns>
  protected bool TryWriteTasksToDatasource(List<Models.Task> tasks, out string error)
  {
    _logger.Debug("Trying to write tasks to datasource", this);
    try
    {
      var options = new JsonSerializerOptions { WriteIndented = true };
      string tasksJson = JsonSerializer.Serialize(tasks, options);
      _logger.Debug($"Writing tasks to datasource. Result: {tasksJson}", this);
      File.WriteAllText(_fileName, tasksJson);
      _tasks.Clear();
      _logger.Debug("Clear tasks cache since the content has been written to datasource", this);
      error = string.Empty;
      return true;
    }
    catch (Exception e)
    {
      error = e.Message;
      return false;
    }
  }

  /// <summary>
  /// Try to get the total task count from the datasource
  /// </summary>
  /// <param name="taskCount">The total task count if the datasource file can be read
  /// successfully</param>
  /// <param name="error">The error message if the task count cannot be retrieved</param>
  /// <returns>True if the task count can be retrieved, else false</returns>
  public bool TryGetTaskCountFromDatasource(out int taskCount, out string error)
  {
    _logger.Debug("Trying to get task count from datasource", this);
    var getTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getTasksSuccess)
    {
      taskCount = 0;
      return false;
    }

    taskCount = tasks.Count;
    return true;
  }

  public bool TryGetLastTaskId(out int id, out string error)
  {

    _logger.Debug("Trying to get ID for new task", this);
    var getTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getTasksSuccess)
    {
      id = 0;
      return false;
    }

    if (tasks.Count == 0)
    {
      id = 0;
      return true;
    }

    var orderedTasks = tasks.OrderByDescending(t => t.Id);
    var taskWithMaxId = orderedTasks.First();

    id = taskWithMaxId.Id;
    return true;
  }

  /// <summary>
  /// Attempts to retrieve all tasks from the datasource file. If datasource doesn't exist, an empty list is returned.
  /// </summary>
  /// <param name="tasks">The list that will be populated with tasks retrieved from the datasource.</param>
  /// <param name="error">An error message if the operation fails; otherwise, an empty string.</param>
  /// <returns>True if all tasks are successfully retrieved and deserialized; otherwise, false.</returns>
  public bool TryGetAllTasks(out List<Models.Task> tasks, out string error)
  {
    error = string.Empty;

    _logger.Debug("Trying to get all tasks from datasource", this);
    _logger.Debug($"Checking if can serve from cache. Task count in cache: {_tasks.Count}", this);
    if (_tasks.Count > 0)
    {
      _logger.Debug("Cache has tasks, returning cache content instead", this);
      tasks = _tasks;
      return true;
    }

    tasks = new List<Models.Task>();

    if (!IsDatasourceExist())
    {
      error = string.Empty;
      return true;
    }

    var tasksFromDatasource = JsonSerializer.Deserialize<List<TaskModel>>(File.ReadAllText(_fileName));

    if (tasksFromDatasource is null)
    {
      error = "Unable to read tasks from datasource";
      return false;
    }

    if (tasksFromDatasource.Count == 0) return true;

    foreach (var t in tasksFromDatasource)
    {
      var createTaskSuccess = Models.Task.TryCreate(t.Id, t.Description, t.Status, t.CreatedAt, t.UpdatedAt, out var task, out error);
      if (!createTaskSuccess)
      {
        return false;
      }

      if (task is null)
      {
        error = $"Unable to map task from datasource. Problematic task: {t.Description}";
        return false;
      }

      tasks.Add(task);
    }
    _logger.Debug($"Storing tasks from datasource to cache", this);
    _tasks = tasks;

    return true;
  }
  /// <summary>
  /// Attempts to insert a task into the datasource.
  /// </summary>
  /// <param name="task">The task to be inserted.</param>
  /// <param name="error">Outputs the error message if the insertion fails.</param>
  /// <returns>True if the task is successfully inserted into the datasource, otherwise false.</returns>
  public bool TryInsertTask(Models.Task task, out string error)
  {
    _logger.Debug($"Trying to insert task in datasource. ID: {task.Id}, Status: {task.Status}, Description: {task.Description}", this);
    var getTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getTasksSuccess)
    {
      return false;
    }

    tasks.Add(task);

    var writeTasksSuccess = TryWriteTasksToDatasource(tasks, out error);
    if (!writeTasksSuccess)
    {
      return false;
    }

    return true;
  }
  /// <summary>
  /// Attempts to retrieve a task from the datasource by its ID.
  /// </summary>
  /// <param name="id">The ID of the task to retrieve</param>
  /// <param name="task">The retrieved task if the retrieval is successful</param>
  /// <param name="error">The error message if the retrieval fails</param>
  /// <returns>True if the retrieval is successful, otherwise false</returns>
  public bool TryGetTask(int id, out Models.Task? task, out string error)
  {
    _logger.Debug($"Trying to get task in datasource using ID: {id}", this);
    task = null;
    var getTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getTasksSuccess)
    {
      return false;
    }

    foreach (var t in tasks)
    {
      if (t.Id == id)
      {
        task = t;
        return true;
      }
    }

    error = $"Unable to find task with ID {id}";
    return false;
  }
  /// <summary>
  /// Attempts to update an existing task in the datasource.
  /// </summary>
  /// <param name="task">The task object containing updated information.</param>
  /// <param name="error">The error message if the task cannot be updated.</param>
  /// <returns>True if the task is successfully updated, otherwise false.</returns>
  public bool TryUpdateTask(Models.Task task, out string error)
  {
    _logger.Debug($"Trying to update task in datasource. ID: {task.Id}, Status: {task.Status}, Description: {task.Description}", this);
    var getTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getTasksSuccess)
    {
      return false;
    }

    foreach (var t in tasks)
    {
      if (t.Id == task.Id)
      {
        var updateTaskStatusSuccess = t.TryUpdateStatus(task.Status, out error);
        if (!updateTaskStatusSuccess)
        {
          return false;
        }
        t.Description = task.Description;
        t.UpdatedAt = task.UpdatedAt;
        break;
      }
    }

    return TryWriteTasksToDatasource(tasks, out error);
  }
  /// <summary>
  /// Attempts to remove a task with the given ID from the datasource and save the updated list of tasks to the datasource.
  /// </summary>
  /// <param name="id">The ID of the task to be deleted.</param>
  /// <param name="error">The error message if the deletion fails.</param>
  /// <returns>True if the task is successfully deleted, otherwise false.</returns>
  public bool TryDeleteTask(int id, out string error)
  {
    _logger.Debug($"Trying to delete task in datasource using ID {id}", this);
    var getTasksSuccess = TryGetAllTasks(out var tasks, out error);
    if (!getTasksSuccess)
    {
      return false;
    }

    foreach (var t in tasks)
    {
      if (t.Id == id)
      {
        tasks.Remove(t);
        break;
      }
    }

    return TryWriteTasksToDatasource(tasks, out error);
  }

  public bool TryUpdateTaskStatus(int id, string status, out string error)
  {
    _logger.Debug($"Trying to update task status in datasource. ID: {id}, Status: {status}", this);
    if (!status.Equals(TaskStatusConstants.InProgressStatus) && !status.Equals(TaskStatusConstants.DoneStatus))
    {
      error = $"Invalid status {status}. You should use either {TaskStatusConstants.InProgressStatus} or {TaskStatusConstants.DoneStatus}";
      return false;
    }

    var getTaskSuccess = TryGetTask(id, out var task, out error);
    if (!getTaskSuccess || task is null)
    {
      return false;
    }

    var updateTaskStatusSuccess = task.TryUpdateStatus(status, out error);
    if (!updateTaskStatusSuccess)
    {
      return false;
    }

    return TryUpdateTask(task, out error);
  }
}
