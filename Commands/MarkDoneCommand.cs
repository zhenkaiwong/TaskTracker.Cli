using TaskTracker.Cli.Constants;

namespace TaskTracker.Cli.Commands;

public class MarkDoneCommand : BaseCommand
{
  public override bool ShouldProcess(string[] args)
  {
    var result = args.Length == 2 && args[0].Equals(CommandConstants.MarkDoneTaskName) && IsTaskId(args[1]);
    _logger.Debug($"Checking if should process {GetType().Name}. Result: {result}", this);
    return result;
  }

  public override bool TryProcess(string[] args, out string? error)
  {
    throw new NotImplementedException();
  }
}
