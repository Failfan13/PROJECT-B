using Postgrest.Models;
using Postgrest;
using static Postgrest.QueryOptions;
using System.Data.SQLite;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DbLogic
{
    private static Supabase.Client? _supabase = DbAccess.GetClient()!;

    public static async Task<List<T>> GetAll<T>() where T : BaseModel, new()
    {
        if (_supabase != null)
        {
            var result = await _supabase.From<T>().Get();

            return result.Models;
        }
        else
        {
            try
            {
                using (SQLiteConnection localConn = DbAccess.LocalDbConn)
                {
                    localConn.Open();
                    using (SQLiteCommand cmd = localConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM " + (typeof(T).ToString() + "s").Replace("Model", "").ToLower();

                        //read the data from database and return it in list format
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            List<T> list = new List<T>();
                            while (await reader.ReadAsync())
                            {
                                var vals = reader.GetValues();
                                Console.WriteLine(vals.AllKeys.Count());

                                if (typeof(T) == typeof(AccountModel))
                                {
                                    list.Add(new AccountModel()
                                    {
                                        Id = Convert.ToInt32(vals[0]),
                                        FirstName = vals[1].ToString(),
                                        LastName = vals[2].ToString(),
                                        EmailAddress = vals[3].ToString(),
                                        Password = vals[4].ToString(),
                                        AdMails = (vals[5] == "1"),
                                        Admin = (vals[6] == "1"),
                                    } as T);
                                }
                                else if (typeof(T) == typeof(ReservationModel))
                                {
                                    list.Add(new ReservationModel()
                                    {
                                        Id = Convert.ToInt32(vals[0]),
                                        AccountId = Convert.ToInt32(vals[1]),
                                        TimeSlotId = Convert.ToInt32(vals[2]),
                                        Seats = JsonSerializer.Deserialize<List<SeatModel>>(vals[3].ToString())!,
                                        Snacks = JsonSerializer.Deserialize<Dictionary<int, int>>(vals[4].ToString())!,
                                        DateTime = Convert.ToDateTime(vals[5]),
                                        Format = vals[6].ToString()
                                    } as T);

                                }
                                else if (typeof(T) == typeof(LocationModel))
                                {
                                    list.Add(new LocationModel()
                                    {
                                        Id = Convert.ToInt32(vals[0]),
                                        Name = vals[1].ToString(),
                                        Address = vals[2].ToString(),
                                        BaseUrl = vals[3].ToString(),
                                        Description = vals[4].ToString(),
                                    } as T);
                                }
                            }

                            return list;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

    }

    // insert new data
    public static Task<T> InsertItem<T>(T model) where T : BaseModel, new()
    {
        var sussy = _supabase.From<T>().Insert(model, new QueryOptions { Returning = ReturnType.Representation });

        return Task.FromResult(sussy.Result.Models[0]);
    }

    // update an item
    public static Task UpdateItem<T>(T changedModel) where T : BaseModel, new()
    {
        //return changedModel.Update<T>();
        return _supabase.From<T>().Update(changedModel);
    }

    // update or add an item
    public static Task UpsertItem<T>(T changedModel) where T : BaseModel, new()
    {
        return _supabase.From<T>().Upsert(changedModel);
    }

    // remove an item
    public static Task RemoveItem<T>(T model) where T : BaseModel, new()
    {
        return _supabase.From<T>().Delete(model);
    }

    // remove an item by auto generated id
    public static Task RemoveItemById<T>(int id) where T : BaseModel, new()
    {
        return _supabase.From<T>()
            .Where(x => (x as DbIndex).Id == id)
            .Delete();
    }

    // check if exists
    public static async Task<bool> ItemExists<T>(T model) where T : BaseModel, new()
    {
        List<T> result = (List<T>)Convert.ChangeType(await _supabase.From<T>().Match(model).Single(), typeof(List<T>))!;

        return result.Count > 0;
    }

    // get item by id
    public static Task<T> GetById<T>(int id) where T : BaseModel, new()
    {
        if (_supabase != null)
        {
            var result = _supabase.From<T>()
            .Where(x => (x as DbIndex).Id == id)
            .Single();

            return result!;
        }
        else // local db
        {
            try
            {
                using (SQLiteConnection localConn = DbAccess.LocalDbConn)
                {
                    using (SQLiteCommand cmd = localConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM " + (typeof(T).ToString() + "s").Replace("Model", "").ToLower() + @"
                    WHERE id = @id";

                        cmd.Parameters.AddWithValue("@id", id);

                        //read the data from database and return it in list format
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var vals = reader.GetValues();
                                return Task.FromResult<T>(new T()
                                {

                                });
                            }

                            Console.ReadKey();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null!;
            }

            return null!;
        }
    }

    public static Task<List<T>> GetAllById<T>(int id) where T : BaseModel, new()
    {
        var result = _supabase.From<T>()
        .Where(x => (x as DbIndex).Id == id)
        .Get();

        return Task.FromResult(result.Result.Models);
    }

    // ----------- AccountLogic ---------- //

    // get account by login details
    public static Task<T> LoginAs<T>(Dictionary<string, string> loginDetails) where T : AccountModel, new()
    {
        if (_supabase != null)
        {
            var result = _supabase.From<T>()
                        .Match(loginDetails)
                        .Single();

            return result!;
        }
        else // local db
        {
            try
            {
                using (SQLiteConnection localConn = DbAccess.LocalDbConn)
                {
                    localConn.Open();
                    using (SQLiteCommand cmd = localConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM " + (typeof(T).ToString() + "s").Replace("Model", "").ToLower() + @"
        WHERE email_address = @email AND password = @password";

                        cmd.Parameters.AddWithValue("@email", loginDetails["email_address"]);
                        cmd.Parameters.AddWithValue("@password", loginDetails["password"]);

                        //read the data from database and return it in list format
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var vals = reader.GetValues();
                                return Task.FromResult<T>(new T()
                                {
                                    Id = Convert.ToInt32(vals[0]),
                                    FirstName = vals[1].ToString(),
                                    LastName = vals[2].ToString(),
                                    EmailAddress = vals[3].ToString(),
                                    Password = vals[4].ToString(),
                                    AdMails = (vals[5] == "1"),
                                    Admin = (vals[6] == "1"),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
            return Task.FromResult<T>(null!);
        }
    }

    // get account by email
    public static Task<T> GetByEmail<T>(string emailAddress) where T : AccountModel, new()
    {

        var eDict = new Dictionary<string, string>
        {
            {"email_address", emailAddress}
        };

        var result = _supabase.From<T>()
            .Match(eDict)
            .Single();

        return result!;
    }

    // ----------- LocalDb ---------- //

    public static void ConnFailedDetails()
    {
        Console.Clear();
        Console.WriteLine(@"Below are the details about the failed connection
        
The Connection to the database failed. 
You will not be able to save any data until the connection is re-established.

To fix the error
1. Restart the application
2. Check the network connection
3. Check if our server is up and running: http://CinemaServerOrDatabase.capweb");
        QuestionLogic.AskEnter();
    }
}