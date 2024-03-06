using Microsoft.EntityFrameworkCore;
using VRDigitalHubSeniorBackendTest.Entities;

namespace VRDigitalHubSeniorBackendTest;

public static class DbContextFactory
{
  public static AsnDbContext CreateDbContext()
  {
    var options = new DbContextOptionsBuilder<AsnDbContext>()
      .UseInMemoryDatabase("AsnDb")
      .Options;
    
    return new AsnDbContext(options);
  }
}
