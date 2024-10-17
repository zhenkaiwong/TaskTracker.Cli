using System;

namespace TaskTracker.Cli.Logger;

public class ConsoleLogger : BaseLogger
{
  /// <summary>
  /// Logs an debug message with the provided message and message source (an object).
  /// </summary>
  /// <param name="message">The debug message to be logged</param>
  /// <param name="messageSource">The source of the debug message (the object that invoke this method)</param>
  public override void Debug(string debugMessage, object messageSource)
  {
    Debug(debugMessage, messageSource.GetType().Name);
  }
  /// <summary>
  /// Logs an debug message with the provided message and message source.
  /// </summary>
  /// <param name="message">The debug message to be logged</param>
  /// <param name="messageSource">The source of the debug message</param>
  public override void Debug(string debugMessage, string messageSourceName)
  {
    if (!IsDebugModeOn()) return;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"DEBUG: {debugMessage} [{messageSourceName}]");
    Console.ResetColor();
  }
  /// <summary>
  /// Logs an error message with the provided message and message source (an object).
  /// </summary>
  /// <param name="message">The error message to be logged</param>
  /// <param name="messageSource">The source of the error message (the object that invoke this method)</param>
  public override void Error(string errorMessage, object messageSource)
  {
    Error(errorMessage, messageSource.GetType().Name);
  }
  /// <summary>
  /// Logs an error message with the provided message and message source.
  /// </summary>
  /// <param name="message">The error message to be logged</param>
  /// <param name="messageSource">The source of the error message</param>
  public override void Error(string errorMessage, string messageSourceName)
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"ERROR: {errorMessage} [{messageSourceName}]");
    Console.ResetColor();
  }
  /// <summary>
  /// Logs an informational message with the provided message and message source (an object).
  /// </summary>
  /// <param name="message">The message to be logged</param>
  /// <param name="messageSource">The source of the message (the object that invoke this method)</param>
  public override void Info(string message, object messageSource)
  {
    Info(message, messageSource.GetType().Name);
  }
  /// <summary>
  /// Logs an informational message with the provided message and message source.
  /// </summary>
  /// <param name="message">The message to be logged</param>
  /// <param name="messageSource">The source of the message</param>
  public override void Info(string message, string messageSourceName)
  {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"INFO: {message} [{messageSourceName}]");
    Console.ResetColor();
  }
}
