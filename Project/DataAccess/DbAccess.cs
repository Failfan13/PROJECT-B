using Postgrest.Models;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Net;
using System.IO;

static class DbAccess
{
    private static Supabase.Client? _supabase = null;
    private static string _restUrl = "https://ksxxjlppohmiiwosokyl.supabase.co";
    private static string _key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImtzeHhqbHBwb2htaWl3b3Nva3lsIiwicm9sZSI6ImFub24iLCJpYXQiOjE2ODY4NjUwOTAsImV4cCI6MjAwMjQ0MTA5MH0.DLTafRXxXxKHglclmEicEX1TyDj7i-mG0o57CdcvLaE";

    // Wrong string for disconnect test
    //private static string _key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlImhqZWFoZ2pkanpjenl5bm9uc2t0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE2ODQzMjQ1NzQsImV4cCI6MTk5OTkwMDU3NH0.sr1XZEzuOXsUFiB0IZb1s2i2TLFQToFV7PL66IDc-AM";

    public static SQLiteConnection LocalDbConn
    {
        get
        {
            return new SQLiteConnection(@"Data Source=DataSources\backupDb.db");
        }
    }

    private static readonly HttpClient _httpClient = new HttpClient()
    {
        BaseAddress = new Uri(_restUrl + "/rest/v1/")
    };

    // Starts API connection
    private static Supabase.Client Start()
    {
        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true,
        };

        Supabase.Client supabase = new Supabase.Client(_restUrl, _key, options);

        return supabase;
    }

    // Gets active client
    public static Supabase.Client? GetClient()
    {
        // Connection not yet asteblished
        if (_supabase == null && CheckClient())
        {
            _supabase = Start();
        }

        return _supabase;
    }

    public static bool CheckClient()
    {
        // Connect using API key
        if (!_httpClient.DefaultRequestHeaders.Contains("apikey"))
        {
            _httpClient.DefaultRequestHeaders.Add("apikey", _key);
        }
        _httpClient.DefaultRequestHeaders.Add("Connection", "close");

        // Get ASYNC response
        HttpResponseMessage response = _httpClient.GetAsync(_restUrl + "/rest/v1/").Result;

        // Check if response is successful
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        // Default not successful
        return false;
    }

    public static SQLiteConnection UseLocalDB()
    {
        return LocalDbConn;
    }

    public static async Task UpdateLocalWithDb()
    {
        if (_supabase == null) return;
        // Create file if nonexistant
        if (!File.Exists(@"DataSources\backupDb.db"))
        {
            SQLiteConnection.CreateFile(@"DataSources\backupDb.db");
        }

        // Delete all tables
        var accounts = await _supabase.From<AccountModel>()
            .Select("*")
            .Limit(500)
            .Get();

        var reservations = await _supabase.From<ReservationModel>()
            .Select("*")
            .Limit(500)
            .Get();

        var locations = await _supabase.From<LocationModel>()
            .Select("*")
            .Limit(500)
            .Get();

        using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=DataSources\backupDb.db"))
        {
            conn.Open();
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS accounts (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    first_name TEXT,
                    last_name TEXT,
                    date_of_birth DATE,
                    email_address TEXT,
                    password TEXT,
                    admin_rights BOOL,
                    ad_mails BOOL
                );

                CREATE TABLE IF NOT EXISTS reservations (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    account_id INTEGER,
                    timeslot_id INTEGER,
                    seats JSON,
                    snacks JSON,
                    date_time DATETIME,
                    format TEXT,
                    discount_code TEXT
                );

                CREATE TABLE IF NOT EXISTS locations (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    description TEXT,
                    gmaps_url TEXT,
                    address TEXT
                )";
                cmd.ExecuteNonQuery();

                string insertCmdAccounts = @"INSERT INTO accounts (id, first_name, last_name, date_of_birth, email_address, password, admin_rights, ad_mails)
VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

                string updateCmdAccounts = @"UPDATE accounts SET first_name = ?, last_name = ?, date_of_birth = ?, email_address = ?, password = ?, admin_rights = ?, ad_mails = ? WHERE id = ?";

                foreach (var account in accounts.Models)
                {
                    try
                    {
                        using (SQLiteCommand AccCmd = new SQLiteCommand(insertCmdAccounts, conn))
                        {
                            AccCmd.Parameters.AddWithValue("@id", account.Id);
                            AccCmd.Parameters.AddWithValue("@first_name", account.FirstName);
                            AccCmd.Parameters.AddWithValue("@last_name", account.LastName);
                            AccCmd.Parameters.AddWithValue("@date_of_birth", account.DateOfBirth);
                            AccCmd.Parameters.AddWithValue("@email_address", account.EmailAddress);
                            AccCmd.Parameters.AddWithValue("@password", account.Password);
                            AccCmd.Parameters.AddWithValue("@admin_rights", account.Admin);
                            AccCmd.Parameters.AddWithValue("@ad_mails", account.AdMails);
                            AccCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception)
                    {
                        using (SQLiteCommand AccCmd = new SQLiteCommand(updateCmdAccounts, conn))
                        {
                            AccCmd.Parameters.AddWithValue("@first_name", account.FirstName);
                            AccCmd.Parameters.AddWithValue("@last_name", account.LastName);
                            AccCmd.Parameters.AddWithValue("@date_of_birth", account.DateOfBirth);
                            AccCmd.Parameters.AddWithValue("@email_address", account.EmailAddress);
                            AccCmd.Parameters.AddWithValue("@password", account.Password);
                            AccCmd.Parameters.AddWithValue("@admin_rights", account.Admin);
                            AccCmd.Parameters.AddWithValue("@ad_mails", account.AdMails);
                            AccCmd.Parameters.AddWithValue("@id", account.Id);
                            AccCmd.ExecuteNonQuery();
                        }
                    }
                }

                string insertCmdReservations = @"INSERT INTO reservations (id, account_id, timeslot_id, seats, snacks, date_time, format, discount_code)
VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

                string updateCmdReservations = @"UPDATE reservations SET account_id = ?, timeslot_id = ?, seats = ?, snacks = ?, date_time = ?, format = ?, discount_code = ? WHERE id = ?";

                foreach (var reservation in reservations.Models)
                {
                    try
                    {
                        using (SQLiteCommand ResCmd = new SQLiteCommand(insertCmdReservations, conn))
                        {
                            ResCmd.Parameters.AddWithValue("@id", reservation.Id);
                            ResCmd.Parameters.AddWithValue("@account_id", reservation.AccountId);
                            ResCmd.Parameters.AddWithValue("@timeslot_id", reservation.TimeSlotId);
                            ResCmd.Parameters.AddWithValue("@seats", reservation.Seats);
                            ResCmd.Parameters.AddWithValue("@snacks", reservation.Snacks);
                            ResCmd.Parameters.AddWithValue("@date_time", reservation.DateTime);
                            ResCmd.Parameters.AddWithValue("@format", reservation.Format);
                            ResCmd.Parameters.AddWithValue("@discount_code", reservation.DiscountCode);
                            ResCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception)
                    {
                        using (SQLiteCommand ResCmd = new SQLiteCommand(updateCmdReservations, conn))
                        {
                            ResCmd.Parameters.AddWithValue("@account_id", reservation.AccountId);
                            ResCmd.Parameters.AddWithValue("@timeslot_id", reservation.TimeSlotId);
                            ResCmd.Parameters.AddWithValue("@seats", reservation.Seats);
                            ResCmd.Parameters.AddWithValue("@snacks", reservation.Snacks);
                            ResCmd.Parameters.AddWithValue("@date_time", reservation.DateTime);
                            ResCmd.Parameters.AddWithValue("@format", reservation.Format);
                            ResCmd.Parameters.AddWithValue("@discount_code", reservation.DiscountCode);
                            ResCmd.Parameters.AddWithValue("@id", reservation.Id);
                            ResCmd.ExecuteNonQuery();
                        }
                    }
                }

                string insertCmdLocations = @"INSERT INTO locations (id, name, description, gmaps_url, address)
VALUES (?, ?, ?, ?, ?)";

                string updateCmdLocations = @"UPDATE locations SET name = ?, description = ?, gmaps_url = ?, address = ? WHERE id = ?";

                foreach (var location in locations.Models)
                {
                    try
                    {
                        using (SQLiteCommand LocCmd = new SQLiteCommand(insertCmdLocations, conn))
                        {
                            LocCmd.Parameters.AddWithValue("@id", location.Id);
                            LocCmd.Parameters.AddWithValue("@name", location.Name);
                            LocCmd.Parameters.AddWithValue("@description", location.Description);
                            LocCmd.Parameters.AddWithValue("@gmaps_url", location.GmapsUrl);
                            LocCmd.Parameters.AddWithValue("@address", location.Address);
                            LocCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception)
                    {
                        using (SQLiteCommand LocCmd = new SQLiteCommand(updateCmdLocations, conn))
                        {
                            LocCmd.Parameters.AddWithValue("@name", location.Name);
                            LocCmd.Parameters.AddWithValue("@description", location.Description);
                            LocCmd.Parameters.AddWithValue("@gmaps_url", location.GmapsUrl);
                            LocCmd.Parameters.AddWithValue("@address", location.Address);
                            LocCmd.Parameters.AddWithValue("@id", location.Id);
                            LocCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}