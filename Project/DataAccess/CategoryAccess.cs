using System.Text.Json;
static class CategoryAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/categories.json"));

    public static List<CategoryModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<CategoryModel>>(json);
    }

    public static void WriteAll(List<CategoryModel> categories)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(categories, options);
        File.WriteAllText(path, json);
    }
}