using TaskTracker.Cli.Constants;

namespace TaskTracker.Cli.Commands;

public class UpdateTaskCommand : BaseCommand
{
  public override bool ShouldProcess(string[] args)
  {
    var result = args.Length == 3 && args[0].Equals(CommandConstants.UpdateTaskName) && IsTaskId(args[1]);
    _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
    return result;
  }

  public override bool TryProcess(string[] args, out string error)
  {
    throw new NotImplementedException();
  }
}
