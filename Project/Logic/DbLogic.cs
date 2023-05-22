using Postgrest.Attributes;
using Postgrest.Models;

public class DbLogic
{
    private static Supabase.Client _supabase = DbAccess.GetClient();

    public static async Task<List<T>> GetAll<T>() where T : BaseModel, new()
    {
        var result = await _supabase.From<T>().Get();

        return result.Models;
    }

    // insert new data
    public static async Task InsertItem<T>(T model) where T : BaseModel, new()
    {
        await _supabase.From<T>().Insert(model);
    }

    // update an item
    public static async Task UpdateItem<T>(T changedModel) where T : BaseModel, new()
    {
        await changedModel.Update<T>();
    }

    // update or add an item
    public static async Task UpsertItem<T>(T changedModel) where T : BaseModel, new()
    {
        await _supabase.From<T>().Upsert(changedModel);
    }

    // remove an item
    public static async Task RemoveItem<T>(T model) where T : BaseModel, new()
    {
        await _supabase.From<T>().Delete(model);
    }

    // remove an item by auto generated id
    public static async Task RemoveItemById<T>(int id) where T : BaseModel, new()
    {
        await _supabase.From<T>()
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
    public static async Task<T> GetById<T>(int id) where T : BaseModel, new()
    {
        var result = await _supabase.From<T>()
            .Where(x => (x as DbIndex).Id == id)
            .Single();

        return result!;
    }

    // get account by login details
    public static async Task<T> LoginAs<T>(Dictionary<string, string> loginDetails) where T : BaseModel, new()
    {
        var result = await _supabase.From<T>()
            .Match(loginDetails)
            .Single();

        return result!;
    }

    // get account by email
    public static async Task<T> GetByEmail<T>(string emailAddress) where T : BaseModel, new()
    {
        var eDict = new Dictionary<string, string>
        {
            {"email", emailAddress}
        };

        var result = await _supabase.From<T>()
            .Match(eDict)
            .Single();

        return result!;
    }
}