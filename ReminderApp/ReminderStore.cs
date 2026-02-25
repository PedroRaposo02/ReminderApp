using System.IO;
using System.Text.Json;
using ReminderApp.Scheduling;

namespace ReminderApp.Persistence
{
    public static class ReminderStore
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ReminderApp",
            "reminder.json"
        );

        public static List<Reminder> Load()
        {
            if (!File.Exists(FilePath)) return [];

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Reminder>>(json) ?? [];
        }

        public static void Save(List<Reminder> reminders)
        {
            var dir = Path.GetDirectoryName(FilePath);
            if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(reminders, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(FilePath, json);
        }
    }
}
