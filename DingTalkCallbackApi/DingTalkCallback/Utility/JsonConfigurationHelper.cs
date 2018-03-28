using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace Utility
{
    public class JsonConfigurationHelper
    {
        public static string GetAppSettings(string key, string value)
        {
            var baseDir = AppContext.BaseDirectory;
            var currentClassDir = baseDir;

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(currentClassDir)
                .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })
                .Build();

            return config.GetSection(key)[value];
        }
    }
}
