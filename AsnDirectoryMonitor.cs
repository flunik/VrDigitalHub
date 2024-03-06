namespace VRDigitalHubSeniorBackendTest;

public class AsnDirectoryMonitor
{
  public event EventHandler<FileCreatedEventArgs>? FileCreated;

  private readonly FileSystemWatcher _watcher;

  public AsnDirectoryMonitor(string path)
  {
    if (!Directory.Exists(path))
      throw new ArgumentException($"The directory {path} does not exist.");

    _watcher = new FileSystemWatcher(path)
    {
      Filter = "*.txt",
      NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
    };

    _watcher.Created += OnCreated;
    _watcher.EnableRaisingEvents = true;
  }

  private void OnCreated(object sender, FileSystemEventArgs e)
  {
    FileCreated?.Invoke(this, new FileCreatedEventArgs { FileName = e.Name!, FullPath = e.FullPath });
  }

  public void StopWatching()
  {
    _watcher.EnableRaisingEvents = false;
    _watcher.Dispose();
  }

  public class FileCreatedEventArgs : EventArgs
  {
    public required string FileName { get; init; }
    public required string FullPath { get; init; }
  }
}