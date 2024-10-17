using TaskTracker.Cli.Constants;

namespace TaskTracker.Cli.Commands;

public class ListCommand : BaseCommand
{

  public override bool ShouldProcess(string[] args)
  {
    bool isLitSubCommands(string subCommand)
    {
      var result = subCommand.Equals("done") || subCommand.Equals("todo") || subCommand.Equals("in-progress");
      _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
      return result;
    }

    var result = (args.Length == 1 && args[0].Equals(CommandConstants.ListTaskName)) || (args.Length == 2 && args[0].Equals("list") && isLitSubCommands(args[1]));
    _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
    return result;
  }

  public override bool TryProcess(string[] args, out string? error)
  {
    throw new NotImplementedException();
  }
}
