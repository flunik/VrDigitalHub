using VRDigitalHubSeniorBackendTest.Entities;

namespace VRDigitalHubSeniorBackendTest;

/// <summary>
/// my own simple asn reader implementation for the test file case
/// </summary>
public class MyAsnReader(string filePath)
{
  private static readonly char separator = ' ';
  public const string HDR = "HDR";
  public const string LINE = "LINE";

  public async IAsyncEnumerable<Box> ReadBoxes()
  {
    using var file = new StreamReader(filePath);

    List<string> lines = [];

    while (await file.ReadLineAsync() is { } line)
    {
      if (line.StartsWith(HDR))
      {
        if (lines.Count > 0)
        {
          yield return LinesToBox(lines);
          lines.Clear();
        }
      }

      lines.Add(line);
    }

    if (lines.Count > 0)
    {
      yield return LinesToBox(lines);
    }
  }

  private Box LinesToBox(List<string> lines)
  {
    Box? box = null;

    foreach (var line in lines)
    {
      if (line.StartsWith(HDR))
      {
        box = ParseHeader(line);
      }
      else if (line.StartsWith(LINE))
      {
        box?.AddContent(ParseLine(line));
      }
    }

    if (box == null) 
      throw new AsnReaderError($"HDR line missing: {String.Join(Environment.NewLine, lines)}");
    
    return box;
  }

  private static Box ParseHeader(string headerLine)
  {
    var parts = headerLine.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length != 3) 
      throw new AsnReaderError($"wrong HDR params count (2 expected): {headerLine}");
    
    return new Box
    {
      SupplierIdentifier = parts[1],
      Identifier = parts[2]
    };
  }

  private static Box.Content ParseLine(string line)
  {
    var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length != 4) 
      throw new AsnReaderError($"wrong LINE params count (3 expected): {line}");
    if (!int.TryParse(parts[3], out var quantity))
      throw new AsnReaderError($"couldn't parse quantity: {line}");
        
    return new Box.Content
    {
      PoNumber = parts[1],
      Isbn = parts[2],
      Quantity = quantity
    };
  }
}

internal class AsnReaderError(string message) : Exception(message: message);