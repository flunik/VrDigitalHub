using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VRDigitalHubSeniorBackendTest.Db.Entities;

namespace VRDigitalHubSeniorBackendTest.Db.Configs;

public class BoxConfig : IEntityTypeConfiguration<Box>
{
  public void Configure(EntityTypeBuilder<Box> builder)
  {
    builder.HasKey(x => new { x.SupplierIdentifier, x.Identifier }); // assuming a clustered key would contain both identifier
    
    builder.Property(x => x.SupplierIdentifier).HasMaxLength(200); // not sure on exact business requirements for lengths of fields, so assuming 200 is max, 
    
    builder.OwnsMany(x => x.Contents, p => // using 'owned' types, which might not be ideal, since ef core will always do the join when querying for data in relational sql, depends on whether the box contents make sense without the box itself and the other way around
    {
      p.Property(x => x.PoNumber).HasMaxLength(200); // not sure on exact business requirements for lengths of fields, so assuming 200 is max, 
      p.Property(x => x.Isbn).HasMaxLength(200); // not sure on exact business requirements for lengths of fields, so assuming 200 is max, 
    });
  }
}