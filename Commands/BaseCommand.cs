using System;
using TaskTracker.Cli.DataServices;
using TaskTracker.Cli.Logger;

namespace TaskTracker.Cli.Commands;

public abstract class BaseCommand
{
  protected BaseLogger _logger = new ConsoleLogger();
  protected IDataService _dataService = new FileStorageJsonDataService();
  /// <summary>
  /// Return true if this command should be executed, else return false.
  /// </summary>
  /// <param name="args">CLI args from user</param>
  /// <returns></returns>
  public abstract bool ShouldProcess(string[] args);

  /// <summary>
  /// Execute the command, return true if success, else return false and set error message. This method will assume the command should be executed.
  /// </summary>
  /// <param name="args">CLI args from user</param>
  /// <param name="error">Error message if command execution fail</param>
  /// <returns></returns>
  public abstract bool TryProcess(string[] args, out string error);

  /// <summary>
  /// Check if the raw input is a valid task id
  /// </summary>
  /// <param name="rawInput">A string value that can be the task ID</param>
  /// <returns></returns>
  protected bool IsTaskId(string rawInput)
  {
    return int.TryParse(rawInput, out int _);
  }
}
