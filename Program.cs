using TaskTracker.Cli.Commands;
using TaskTracker.Cli.Logger;

namespace TaskTracker.Cli;

class Program
{
    static void Main(string[] args)
    {
        var commandFactory = new CommandFactory();
        var _logger = new ConsoleLogger();
        var getCommandSuccess = commandFactory.TryGetCommand(args, out var command, out var getCommandError);

        if (!getCommandSuccess || command is null)
        {
            if (!string.IsNullOrEmpty(getCommandError))
            {
                _logger.Error(getCommandError, "Main");

            }
            else
            {
                _logger.Error($"Unexpected error in CommandFactory while trying to find a command using: {args[0]}", "Main");
            }

            return;
        }

        var processCommandSuccess = command.TryProcess(args, out var processCommandError);

        if (!processCommandSuccess)
        {
            if (string.IsNullOrEmpty(processCommandError))
            {
                _logger.Error("Unexpected error. Please try again", "Main");
            }
            else
            {

                _logger.Error(processCommandError, "Main");
            }

            return;
        }
    }
}
