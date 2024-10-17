using TaskTracker.Cli.Constants;

namespace TaskTracker.Cli.Models;

public class Task
{
  public int Id { get; set; }
  public string Description { get; set; } = string.Empty;
  public string Status { get; private set; } = TaskStatusConstants.TodoStatus;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  private Task(int id, string description, string status, DateTime createdAt, DateTime updatedAt)
  {
    Id = id;
    Description = description;
    Status = status;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

  /// <summary>
  /// Checks if the given status is valid by comparing it with predefined status values.
  /// </summary>
  /// <param name="status">The status to be validated</param>
  private static bool IsStatusValid(string status)
  {
    string[] statuses = { TaskStatusConstants.DoneStatus, TaskStatusConstants.InProgressStatus, TaskStatusConstants.TodoStatus };
    return statuses.Contains(status);
  }

  /// <summary>
  /// Attempts to create a task object with given parameters. Fails if the status is not one of the predefined status values.
  /// </summary>
  /// <param name="id">The unique identifier of the task</param>
  /// <param name="description">The description of the task</param>
  /// <param name="status">The status of the task. Must be one of the values in <see cref="TaskStatusConstants"/></param>
  /// <param name="createdAt">The time when the task was created</param>
  /// <param name="updatedAt">The time when the task was last updated</param>
  /// <param name="task">The created task object if the operation is successful, otherwise null</param>
  /// <param name="error">The error message if the operation fails</param>
  /// <returns>True if the task is successfully created, otherwise false</returns>
  public static bool TryCreate(int id, string description, string status, DateTime createdAt, DateTime updatedAt, out Task? task, out string error)
  {
    if (!IsStatusValid(status))
    {
      error = $"Status '{status}' is not a valid status. Valid statuses are '{TaskStatusConstants.DoneStatus}', '{TaskStatusConstants.InProgressStatus}' and '{TaskStatusConstants.TodoStatus}'";
      task = null;
      return false;
    }

    error = string.Empty;
    task = new Task(id, description, status, createdAt, updatedAt);
    return true;
  }

  /// <summary>
  /// Attempts to update the status of the task to the given new status. Fails if the new status is not one of the predefined status values.
  /// </summary>
  /// <param name="newStatus">The new status of the task. Must be one of the values in <see cref="TaskStatusConstants"/></param>
  /// <param name="error">The error message if the operation fails</param>
  /// <returns>True if the status is successfully updated, otherwise false</returns>
  public bool TryUpdateStatus(string newStatus, out string error)
  {
    if (!IsStatusValid(newStatus))
    {
      error = $"Status '{newStatus}' is not a valid status. Valid statuses are '{TaskStatusConstants.DoneStatus}', '{TaskStatusConstants.InProgressStatus}' and '{TaskStatusConstants.TodoStatus}'";
      return false;
    }

    Status = newStatus;
    UpdatedAt = DateTime.UtcNow;
    error = string.Empty;
    return true;
  }
}
