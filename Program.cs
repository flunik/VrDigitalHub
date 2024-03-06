using Microsoft.EntityFrameworkCore;
using VRDigitalHubSeniorBackendTest;
using VRDigitalHubSeniorBackendTest.AsnMonitor;
using VRDigitalHubSeniorBackendTest.AsnProcessor;
using VRDigitalHubSeniorBackendTest.AsnReader;
using VRDigitalHubSeniorBackendTest.Logging;

// simulate appSetting configuration. in real life scenario the config could go from console app args, or could env variables or whatever
var appSettings = new AppSettingsResolver().GetAppSettings();

// generate dependencies, usually done with di
var dbContext = DbContextFactory.CreateDbContext(); // this is a stub, so we won't build a container with db context etc in console app.
var logger = new Logger(); // in real life it could be MS ILogger implementation
var monitor = new AsnDirectoryMonitor(GetAbsolutePathRelativeToSolutionRoot(appSettings.DirectoryToWatch)); 
var processor = new AsnProcessor(dbContext, monitor, new MyAsnReader(), logger); // could be injected if containers are used in real-life scenario
processor.Start();

// wait for input
Console.ReadKey(); 

processor.End();

// output work done
var importedBoxes = await dbContext.Boxes.ToListAsync();
logger.LogInfo($"db now has {importedBoxes.Count} boxes with {importedBoxes.Sum(x =>x.Contents.Count)} contents");

// for the ease of portability - the files are monitored to the root of the solution folder, and not the bin/debug/net8/etc
string GetAbsolutePathRelativeToSolutionRoot(string relativeDirectoryToMonitor)
{
  var baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // get directory we're running from
  var projectDirectory = new DirectoryInfo(baseDirectory).Parent?.Parent?.Parent?.FullName!; // navigate to solution/proj root directory
  return Path.Combine(projectDirectory, relativeDirectoryToMonitor);
}