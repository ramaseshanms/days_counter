using System.IO;
using System.Text.Json;
using System.Windows.Media;

namespace DaysCounter
{
    public class AppConfig
    {
        public string TextColor { get; set; } = "#FFFFFFFF"; // Default White
        public double FontSize { get; set; } = 48;
        public double OverlayScale { get; set; } = 1.0;
        public DateTime TargetDate { get; set; } = new DateTime(2025, 12, 18);
        public bool ShowTimeDetail { get; set; } = false;
        public bool FirstRun { get; set; } = true;
        public bool RunOnStartup { get; set; } = true;

        private static string ConfigPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DaysCounter", "config.json");

        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var json = File.ReadAllText(ConfigPath);
                    return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                }
            }
            catch { }
            return new AppConfig();
        }

        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var json = JsonSerializer.Serialize(this);
                File.WriteAllText(ConfigPath, json);
            }
            catch { }
        }
    }
}
