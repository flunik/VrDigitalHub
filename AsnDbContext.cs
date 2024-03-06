using Microsoft.EntityFrameworkCore;
using VRDigitalHubSeniorBackendTest.Entities;

namespace VRDigitalHubSeniorBackendTest;

public class AsnDbContext(DbContextOptions<AsnDbContext> options) : DbContext(options)
{
  public DbSet<Box> Boxes => Set<Box>();

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.ApplyConfigurationsFromAssembly(typeof(AsnDbContext).Assembly);
  }
}
