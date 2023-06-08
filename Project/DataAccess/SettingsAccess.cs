using System.Text.Json;
static class SettingsAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/settings.json"));

    public static SettingModel Load()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<SettingModel>(json);
    }

    public static void Write(SettingModel settings)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(settings, options);
        File.WriteAllText(path, json);
    }
}