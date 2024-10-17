using System;

namespace TaskTracker.Cli.Logger;

public abstract class BaseLogger
{
  /// <summary>
  /// Return true if debug mode is enabled, else return false
  /// </summary>
  /// <returns></returns>
  protected bool IsDebugModeOn()
  {
    return true;
  }

  /// <summary>
  /// Logs an informational message with the provided message and message source (an object).
  /// </summary>
  /// <param name="message">The message to be logged</param>
  /// <param name="messageSource">The source of the message (the object that invoke this method)</param>
  public abstract void Info(string message, object messageSource);

  /// <summary>
  /// Logs an informational message with the provided message and message source.
  /// </summary>
  /// <param name="message">The message to be logged</param>
  /// <param name="messageSource">The source of the message</param>
  public abstract void Info(string message, string messageSourceName);
  /// <summary>
  /// Logs an error message with the provided message and message source (an object).
  /// </summary>
  /// <param name="message">The error message to be logged</param>
  /// <param name="messageSource">The source of the error message (the object that invoke this method)</param>
  public abstract void Error(string errorMessage, object messageSource);

  /// <summary>
  /// Logs an error message with the provided message and message source.
  /// </summary>
  /// <param name="message">The error message to be logged</param>
  /// <param name="messageSource">The source of the error message</param>
  public abstract void Error(string errorMessage, string messageSourceName);

  /// <summary>
  /// Logs an debug message with the provided message and message source (an object).
  /// </summary>
  /// <param name="message">The debug message to be logged</param>
  /// <param name="messageSource">The source of the debug message (the object that invoke this method)</param>
  public abstract void Debug(string debugMessage, object messageSource);

  /// <summary>
  /// Logs an debug message with the provided message and message source.
  /// </summary>
  /// <param name="message">The debug message to be logged</param>
  /// <param name="messageSource">The source of the debug message</param>
  public abstract void Debug(string debugMessage, string messageSourceName);
}
