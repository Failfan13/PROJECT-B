using System.Text.Json;

static class SnackAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/snacks.json"));


    public static List<SnackModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<SnackModel>>(json);
    }


    public static void WriteAll(List<SnackModel> snack)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(snack, options);
        File.WriteAllText(path, json);
    }

}