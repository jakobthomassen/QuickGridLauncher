using System;
using System.IO;
using System.Text.Json;
using QuickGridLauncher.Models;

namespace QuickGridLauncher.Services
{
    public static class ConfigService
    {
        private static readonly string AppFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "QuickGridLauncher");

        private static readonly string ConfigPath =
            Path.Combine(AppFolder, "config.json");

        private static readonly JsonSerializerOptions JsonOptions =
            new()
            {
                WriteIndented = true
            };

        public static AppConfig Load()
        {
            try
            {
                if (!Directory.Exists(AppFolder))
                    Directory.CreateDirectory(AppFolder);

                if (!File.Exists(ConfigPath))
                {
                    var defaultConfig = CreateDefaultConfig();
                    Save(defaultConfig);
                    return defaultConfig;
                }

                var json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<AppConfig>(json);

                return config ?? CreateDefaultConfig();
            }
            catch
            {
                return CreateDefaultConfig();
            }
        }

        public static void Save(AppConfig config)
        {
            if (!Directory.Exists(AppFolder))
                Directory.CreateDirectory(AppFolder);

            var json = JsonSerializer.Serialize(config, JsonOptions);
            File.WriteAllText(ConfigPath, json);
        }

        private static AppConfig CreateDefaultConfig()
        {
            return new AppConfig
            {
                Columns = 4,
                BackgroundColor = "#AA666666",
                Apps =
                {
                    new AppEntry { Name = "Calc", Path = "C:\\Windows\\System32\\calc.exe" },
                    new AppEntry { Name = "Notepad", Path = "C:\\Windows\\System32\\notepad.exe" }
                }
            };
        }
    }
}