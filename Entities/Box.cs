// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace VRDigitalHubSeniorBackendTest.Entities;

public class Box
{
  public required string SupplierIdentifier { get; init; }
  public required string Identifier { get; init; }

  private readonly ICollection<Content> _contents = new List<Content>();
  public IReadOnlyCollection<Content> Contents => (IReadOnlyCollection<Content>)_contents; // in order to keep the Contents an IReadOnlyCollection, added backing field, and removed setter 

  public void AddContent(Content content) => _contents.Add(content); 

  public class Content
  {
    public required string PoNumber { get; init; }
    public required string Isbn { get; init; }
    public required int Quantity { get; init; }
  }
}
