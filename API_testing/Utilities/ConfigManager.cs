using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Utilities
{
    public static class ConfigManager
    {
        private static readonly JObject Config;

        static ConfigManager()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Configuration file not found at path: {configPath}");
            }

            Config = JObject.Parse(File.ReadAllText(configPath));
        }

        public static string Get(string key)
        {
            var tokens = key.Split(':');
            JToken value = Config;
            foreach (var token in tokens)
            {
                value = value[token];
                if (value == null) throw new ArgumentException($"Key '{key}' not found in configuration.");
            }

            return value.ToString();
        }
    }
}
