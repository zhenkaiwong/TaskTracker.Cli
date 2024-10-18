namespace TaskTracker.Cli.DataServices;

public interface IDataService
{
  public bool TryInsertTask(Models.Task task, out string error);
  public bool TryGetAllTasks(out List<Models.Task> tasks, out string error);
  public bool TryGetTask(int id, out Models.Task? task, out string error);
  public bool TryUpdateTask(Models.Task task, out string error);
  public bool TryDeleteTask(int id, out string error);
  public bool TryGetTaskCountFromDatasource(out int taskCount, out string error);
  public bool TryUpdateTaskStatus(int id, string status, out string error);
}
