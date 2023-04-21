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

    // public static class LoadAllCondition
    // {
    //     public static List<object> Conditions { get; private set; }

    //     public static List<object>? LoadAll(PromoModel promo)
    //     {
    //         var el = promo.Condition;

    //         foreach (var element in el)
    //         {

    //         }
    //         return null;
    //     }
    // }
}