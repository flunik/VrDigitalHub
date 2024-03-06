using VRDigitalHubSeniorBackendTest.Db.Entities;

namespace VRDigitalHubSeniorBackendTest.AsnReader;

public interface IAsnReader
{
  IAsyncEnumerable<Box> ReadBoxes(string filePath);
}