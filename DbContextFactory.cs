using Microsoft.EntityFrameworkCore;
using VRDigitalHubSeniorBackendTest.Db;

namespace VRDigitalHubSeniorBackendTest;

// just an in-memory db context, in real-life scenario it would be instantiated using DI and configured somewhere in infrastructure layer (provided DDD with App/Domain/Infrastructure layers is used) 
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
