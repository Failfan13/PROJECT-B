using Postgrest.Models;
using System.Data.SQLite;
using System.Data.SqlClient;
static class DbAccess
{
    private static Supabase.Client _supabase;
    private static string url = "https://hjeahgjdjzczyynonskt.supabase.co";
    private static string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImhqZWFoZ2pkanpjenl5bm9uc2t0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE2ODQzMjQ1NzQsImV4cCI6MTk5OTkwMDU3NH0.sr1XZEzuOXsUFiB0IZb1s2i2TLFQToFV7PL66IDc-AM";

    // auto calls Start()
    static DbAccess()
    {
        _supabase = Start();
    }

    // gets Client
    private static Supabase.Client Start()
    {
        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true,
        };

        Supabase.Client supabase = new Supabase.Client(url, key, options);
        return supabase;
    }

    // loads all T types
    public static async Task<List<T>> LoadAll<T>() where T : BaseModel, new()
    {
        var result = await _supabase.From<T>().Get();
        var models = result.Models;

        return (List<T>)Convert.ChangeType(models, typeof(List<T>));
    }

    // insert new data
    public static async Task InsertData<T>(T model) where T : BaseModel, new()
    {
        await _supabase.From<T>().Insert(model);
    }

    public static async Task ExecuteNonQuery()
    {
        var sussy = await _supabase.Rpc("getnewestid", null!);

        Console.WriteLine(sussy.Content);
    }

    public static async Task<T> ExecuteQuery<T>(string funcName, Dictionary<string, object>? parameters = null)
    {
        var result = await _supabase.Rpc(funcName, parameters!);

        return (T)Convert.ChangeType(result.Content, typeof(T))!;
    }
}