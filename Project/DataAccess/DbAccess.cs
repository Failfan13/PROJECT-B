using Postgrest.Models;
using System.Data.SQLite;
using System.Data.SqlClient;
static class DbAccess
{
    public static Supabase.Client _supabase;
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

    public static Supabase.Client GetClient()
    {
        return _supabase;
    }
}