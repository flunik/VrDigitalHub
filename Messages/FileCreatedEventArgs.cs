namespace VRDigitalHubSeniorBackendTest.Messages;

/// this one could be a record and it also could be an queue message in real life scenario sent @ redis or any other queue like sqs or azure service bus, thus resides in Messages :) 
public class FileCreatedEventArgs : EventArgs
{
  public required string FileName { get; init; }
  public required string FullPath { get; init; }
}