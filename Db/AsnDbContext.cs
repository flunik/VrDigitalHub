using Microsoft.EntityFrameworkCore;
using VRDigitalHubSeniorBackendTest.Db.Entities;

namespace VRDigitalHubSeniorBackendTest.Db;

/// usually i'd go with app/domain/infrastructure design in my own projects, so the DB stuff resides inside DB folder just to keep it simple.
public class AsnDbContext(DbContextOptions<AsnDbContext> options) : DbContext(options)
{
  public DbSet<Box> Boxes => Set<Box>();

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.ApplyConfigurationsFromAssembly(typeof(AsnDbContext).Assembly);
  }
}
