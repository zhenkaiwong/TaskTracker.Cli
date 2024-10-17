using TaskTracker.Cli.Commands;
using TaskTracker.Cli.Logger;

namespace TaskTracker.Cli;

class Program
{
    static void Main(string[] args)
    {
        var commandFactory = new CommandFactory();
        var _logger = new ConsoleLogger();
        var getCommandSuccess = commandFactory.TryGetCommand(args, out var command);

        if (!getCommandSuccess || command is null)
        {
            if (args.Length == 0)
            {
                _logger.Error("Invalid CLI input. You need to spcify a command and necessary parameters", "Main");

            }
            else
            {
                _logger.Error($"Unable to find a command that match {args[0]}", "Main");
            }

            if (command is null)
            {
                _logger.Error("We should have a command for your input, but somehow it doesn't works", "Main");
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
