using System;

namespace TaskTracker.Cli.Logger;

public class ConsoleLogger : BaseLogger
{
  public override void Debug(string debugMessage, object messageSource)
  {
    Debug(debugMessage, messageSource.GetType().Name);
  }

  public override void Debug(string debugMessage, string messageSourceName)
  {
    if (!IsDebugModeOn()) return;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"DEBUG: {debugMessage} [{messageSourceName}]");
    Console.ResetColor();
  }

  public override void Error(string errorMessage, object messageSource)
  {
    Error(errorMessage, messageSource.GetType().Name);
  }

  public override void Error(string errorMessage, string messageSourceName)
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"ERROR: {errorMessage} [{messageSourceName}]");
    Console.ResetColor();
  }

  public override void Info(string message, object messageSource)
  {
    Info(message, messageSource.GetType().Name);
  }

  public override void Info(string message, string messageSourceName)
  {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"INFO: {message} [{messageSourceName}]");
    Console.ResetColor();
  }
}
