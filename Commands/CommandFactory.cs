using System;
using TaskTracker.Cli.Constants;
using TaskTracker.Cli.Logger;

namespace TaskTracker.Cli.Commands;

public class CommandFactory
{
  /// <summary>
  /// The dictionary of commands
  /// </summary>
  protected Dictionary<string, BaseCommand> Commands { get; set; } = [];
  protected BaseLogger _logger;
  public CommandFactory()
  {
    _logger = new ConsoleLogger();
    RegisterCommand(CommandConstants.AddTaskName, new AddTaskCommand());
    RegisterCommand(CommandConstants.DeleteTaskName, new DeleteTaskCommand());
    RegisterCommand(CommandConstants.ListTaskName, new ListCommand());
    RegisterCommand(CommandConstants.MarkDoneTaskName, new MarkDoneCommand());
    RegisterCommand(CommandConstants.MarkInProgressTaksName, new MarkInProgressCommand());
    RegisterCommand(CommandConstants.UpdateTaskName, new UpdateTaskCommand());
  }

  /// <summary>
  /// Registers a command to the factory
  /// </summary>
  /// <param name="command">The command to be registered</param>
  protected void RegisterCommand(string commandName, BaseCommand command)
  {
    _logger.Debug($"Registering command to factory. Command name: {commandName}", this);
    if (Commands.ContainsKey(commandName)) return;
    Commands.Add(commandName, command);
  }

  /// <summary>
  /// Try get a command from Commands dictionary, return false if no command can fulfil user input
  /// </summary>
  /// <param name="args">CLI input from user</param>
  /// <param name="command">The command to be returned if exists</param>
  /// <returns></returns>

  public bool TryGetCommand(string[] args, out BaseCommand? command, out string error)
  {
    command = null;
    if (args.Length == 0)
    {
      error = "Invalid CLI input. You need to spcify a command and necessary parameters";
      return false;
    }
    var commandToRetrieve = args[0];
    _logger.Debug($"Trying to retrieve command from factory. Command name: {commandToRetrieve}", this);
    if (!Commands.ContainsKey(commandToRetrieve))
    {
      error = $"Unable to find a command that match {args[0]}";
      return false;
    }
    command = Commands[args[0]];
    var shouldProcess = command.ShouldProcess(args);

    if (!shouldProcess)
    {
      error = "We find a command for you, but your parameters doesn't match";
      return false;
    }

    error = string.Empty;
    return true;
  }
}
