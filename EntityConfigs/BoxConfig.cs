using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VRDigitalHubSeniorBackendTest.Entities;

namespace VRDigitalHubSeniorBackendTest.EntityConfigs;

public class BoxConfig : IEntityTypeConfiguration<Box>
{
  public void Configure(EntityTypeBuilder<Box> builder)
  {
    builder.HasKey(x => new { x.SupplierIdentifier, x.Identifier }); // assuming a clustered key would work
    
    builder.Property(x => x.SupplierIdentifier).HasMaxLength(200); // not sure on exact business requirements for lengths of fields, so assuming 200 is max, 
    
    builder.OwnsMany(x => x.Contents, p => // using 'owned' types, which might not be ideal, since ef core will always do the join when querying for data in relational sql
    {
      p.Property(x => x.PoNumber).HasMaxLength(200); // not sure on exact business requirements for lengths of fields, so assuming 200 is max, 
      p.Property(x => x.Isbn).HasMaxLength(200); // not sure on exact business requirements for lengths of fields, so assuming 200 is max, 
    });
  }
}