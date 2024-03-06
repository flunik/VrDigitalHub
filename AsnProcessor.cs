using Microsoft.EntityFrameworkCore;

namespace VRDigitalHubSeniorBackendTest;

/// <summary>
/// the processor monitors the specified folder, and in case new files are found, reads the file, and saves to db
/// the error handling, logging, resiliency are omitted
/// real-life scenario would require not only monitoring new files, but keeping a list of which files were imported, whether the files were processed
/// also real-life scenario would require monitoring not only the newly created files, but the files which might have been created while the service wasn't running
/// </summary>
public class AsnProcessor(AsnDbContext dbContext)
{
  private AsnDirectoryMonitor? _watcher;

  public void Start(string folderToMonitor)
  {
    _watcher = new AsnDirectoryMonitor(folderToMonitor);
    _watcher.FileCreated += WatcherFileCreated; // just for the sake of decoupling. can use mediatr for non-durable queues, or redis or amazon sqs or azure service bus for event handling in serverless/microservice architectures
    Console.WriteLine($"started monitoring .txt files in {folderToMonitor}");
  }

  public void End()
  {
    _watcher?.StopWatching();
    Console.WriteLine($"ended monitoring new files");
  }

  private async void WatcherFileCreated(object? sender, AsnDirectoryMonitor.FileCreatedEventArgs e) // i hate async void, but it should work
  {
    Console.WriteLine($"new text file found: {e.FileName}, full Path: {e.FullPath}");
    await HandleFileCreatedAsync(e);
  }

  private async Task HandleFileCreatedAsync(AsnDirectoryMonitor.FileCreatedEventArgs e)
  {
    try
    {
      var processor = new MyAsnReader(e.FullPath);
      await foreach (var box in processor.ReadBoxes())
      {
        dbContext.Boxes.Add(box);
        await dbContext.SaveChangesAsync();
      }
    }
    catch (DbUpdateException ex)
    {
      Console.WriteLine($"failed to process {e.FullPath} due to DbUpdateException: {ex.Message}");
      // in real-life scenarion, if db connection fails should at least attempt to retry in case of transport db exception, in case of 'schema' or 'index' exceptions, should log critical and alert as well.
    }
    catch (InvalidOperationException ex)
    {
      // can be caused by Box configuration misconfiguration
      Console.WriteLine($"failed to process {e.FullPath} due to Box entity misconfiguration {ex.Message}");
    }
    catch (AsnReaderError ex)
    {
      // i don't like throwing errors usually, and instead i tend to return the Either monad with either result or error, but keeping it simple here, since using IAsyncEnumerable  
      Console.WriteLine($"failed to read {e.FullPath} due to {ex.Message}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"failed to process {e.FullPath} due to {ex.Message}");
      // obviously, in real-life scenario -
      // if reader fails, should log critical and alert, and attempt to reprocess the file, starting from the last position

      // in this situation - the exception itself shouldn't be rethrown due to 'async void' nature of WatcherFileCreated and c# events
    }
  }
}