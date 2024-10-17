using TaskTracker.Cli.Constants;

namespace TaskTracker.Cli.Models;
/// <summary>
/// The model that is used to store task information in the datasource file
/// </summary>
public class TaskModel
{
  public int Id { get; set; }
  public string Description { get; set; } = string.Empty;
  public string Status { get; set; } = TaskStatusConstants.TodoStatus;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
