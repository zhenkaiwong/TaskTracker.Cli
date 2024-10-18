using TaskTracker.Cli.Commands;
using TaskTracker.Cli.Logger;

namespace TaskTracker.Cli;

class Program
{
    static void Main(string[] args)
    {
        var commandFactory = new CommandFactory();
        var _logger = new ConsoleLogger();
        var getCommandSuccess = commandFactory.TryGetCommand(args, out var command, out var error);

        if (!getCommandSuccess || command is null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                _logger.Error(error, "Main");

            }
            else
            {
                _logger.Error($"Unexpected error in CommandFactory while trying to find a command using: {args[0]}", "Main");
            }

            return;
        }

        var processCommandSuccess = command.TryProcess(args, out var error);

        if (!processCommandSuccess)
        {
            if (string.IsNullOrEmpty(error))
            {
                _logger.Error("Unexpected error. Please try again", "Main");
            }
            else
            {

                _logger.Error(error, "Main");
            }

            return;
        }
    }
}
