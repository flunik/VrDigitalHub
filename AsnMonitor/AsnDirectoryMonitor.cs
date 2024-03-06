using VRDigitalHubSeniorBackendTest.Messages;

namespace VRDigitalHubSeniorBackendTest.AsnMonitor;

public class AsnDirectoryMonitor : IAsnDirectoryMonitor
{
  public event EventHandler<FileCreatedEventArgs>? FileCreated;

  private readonly FileSystemWatcher _watcher;

  // here i go with assumption that we're only monitoring NEW files and not the existing ones
  // real-life example - the client drops ASN file to an shared ftp, which is monitored by azure function, which then is found, and an event with file path is sent 
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
  }

  private void OnCreated(object sender, FileSystemEventArgs e)
  {
    FileCreated?.Invoke(this, new FileCreatedEventArgs { FileName = e.Name!, FullPath = e.FullPath });
  }
  
  public void StartWatching()
  {
    _watcher.EnableRaisingEvents = true;
  }

  public void StopWatching()
  {
    _watcher.EnableRaisingEvents = false;
    _watcher.Dispose();
  }
}