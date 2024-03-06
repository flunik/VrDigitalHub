namespace VRDigitalHubSeniorBackendTest.Logging;

public class Logger : ILogger
{
  public void LogInfo(string message)
  {
    Console.ResetColor();
    Console.WriteLine(message);
  }
  public void LogError(string message)
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
  }

  public void LogError(Exception ex) => LogError(ex.Message);
}