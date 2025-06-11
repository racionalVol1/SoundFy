using Microsoft.Extensions.Configuration;
using System.IO;
 
namespace Data.Config
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // ou AppContext.BaseDirectory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
 
            return builder.Build();
        }
    }
}