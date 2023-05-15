using System.Text.Json;
static class PromoAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/promos.json"));

    public static List<PromoModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<PromoModel>>(json);
    }

    public static void WriteAll(List<PromoModel> categories)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(categories, options);
        File.WriteAllText(path, json);
    }
}