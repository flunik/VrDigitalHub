using Microsoft.EntityFrameworkCore;
using VRDigitalHubSeniorBackendTest;

var dbContext = DbContextFactory.CreateDbContext(); // this is a stub, so we won't build a container with db context etc in console app.
string folderToMonitor = GetMonitorDirectoryPath(); // this is a stub, so it would select DirectoryToMonitor from the solution dir.

var processor = new AsnProcessor(dbContext); // could be injected if containers are used in real-life scenario
processor.Start(folderToMonitor);

Console.ReadKey(); 

processor.End();

var importedBoxes = await dbContext.Boxes.ToListAsync();
Console.WriteLine($@"db now has {importedBoxes.Count} boxes with {importedBoxes.Sum(x =>x.Contents.Count)} contents");


string GetMonitorDirectoryPath()
{
  var baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // get directory we're running from
  var projectDirectory = new DirectoryInfo(baseDirectory).Parent?.Parent?.Parent?.FullName; // navigate to solution/proj directory
  return $"{projectDirectory}\\DirectoryToMonitor"; // hardcoded
}