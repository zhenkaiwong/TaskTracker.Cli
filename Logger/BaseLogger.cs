namespace TaskTracker.Cli.Logger;

public abstract class BaseLogger
{
  /// <summary>
  /// Return true if debug mode is enabled, else return false
  /// </summary>
  /// <returns></returns>
  protected bool IsDebugModeOn()
  {
    return false;
  }

  public abstract void Info(string message, object messageSource);
  public abstract void Info(string message, string messageSourceName);
  public abstract void Error(string errorMessage, object messageSource);
  public abstract void Error(string errorMessage, string messageSourceName);
  public abstract void Debug(string debugMessage, object messageSource);
  public abstract void Debug(string debugMessage, string messageSourceName);
}
