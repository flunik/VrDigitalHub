using VRDigitalHubSeniorBackendTest.Messages;

namespace VRDigitalHubSeniorBackendTest.AsnMonitor;

public interface IAsnDirectoryMonitor {
  event EventHandler<FileCreatedEventArgs>? FileCreated;
  void StartWatching();
  void StopWatching();
}