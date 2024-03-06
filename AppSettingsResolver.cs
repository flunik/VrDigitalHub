using Microsoft.Extensions.Configuration;

namespace VRDigitalHubSeniorBackendTest;

// usually it would be done using DI, but i simulate appsettings, to get a strongly typed AppSettings 
public class AppSettingsResolver
{
  public AppSettings GetAppSettings()
  {
    var builder = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory()) // the appsettings gets copied to the debug/release folder, so we're using it instead of solution root
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

    IConfigurationRoot configuration = builder.Build();
    var appSettingsSection = configuration.GetSection("AppSettings");
    var appSettings = appSettingsSection.Get<AppSettings>() ?? throw new Exception("Failed to load AppSettings");
    return appSettings;
  }
}