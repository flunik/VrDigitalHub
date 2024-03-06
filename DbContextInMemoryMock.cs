using Microsoft.EntityFrameworkCore;
using VRDigitalHubSeniorBackendTest.Entities;

namespace VRDigitalHubSeniorBackendTest;

/// <summary>
/// singleton mock for in-memory db context, instead of di
/// </summary>
public static class DbContextInMemoryMock
{
  private static AsnDbContext? _instance;
  // ReSharper disable once InconsistentNaming
  private static readonly object _lock = new object();

  public static AsnDbContext Instance
  {
    get
    {
      lock (_lock)
      {
        if (_instance == null)
        {
          var options = new DbContextOptionsBuilder<AsnDbContext>()
            .UseInMemoryDatabase("AsnDb")
            .Options;

          _instance = new AsnDbContext(options);
        }

        return _instance;
      }
    }
  }
}