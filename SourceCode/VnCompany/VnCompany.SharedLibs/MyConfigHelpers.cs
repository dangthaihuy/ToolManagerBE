using Microsoft.Extensions.Configuration;
using System.IO;

namespace VnCompany.SharedLibs
{
    public class AppConfiguration
    {
        public static IConfiguration _root = null;
        public static AppConfiguration Instance { get; protected set; } = new AppConfiguration();

        public AppConfiguration()
        {
            if(_root == null)
            {
                _root = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false)
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"), optional: false)
                .Build();
            }
        }

        public static string GetAppsetting(string name)
        {
            Instance = AppConfiguration.Instance;
            return _root.GetSection(name).Value;
        }
    }

    public class MyConfigHelpers
    {
        private readonly IConfiguration Configuration;
        public MyConfigHelpers(IConfiguration config)
        {
            Configuration = config;            
        }

        public string GetAppsetting(string key)
        {
            return Configuration["AllowedHosts"];
        }
    }
}
