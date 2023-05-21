using Postgrest.Attributes;
using Postgrest.Models;

public class DbLogic
{
    public List<T> GetAll<T>() where T : BaseModel, new()
    {
        return (List<T>)Convert.ChangeType(DbAccess.LoadAll<T>(), typeof(List<T>));
    }

    public void UpdateItem<T>(T model) where T : BaseModel2, new()
    {
        DbAccess.UpdateItem<T>(model);
    }

    public void RemoveItem<T>(T model) where T : BaseModel2, new()
    {
        DbAccess.RemoveItem<T>(model);
    }

    public void RemoveItemById<T>(int id) where T : BaseModel2, new()
    {
        DbAccess.RemoveItemById<T>(id);
    }

    public T GetById<T>(int id) where T : BaseModel2, new()
    {
        return (T)Convert.ChangeType(DbAccess.GetById<T>(id), typeof(T))!;
    }

    //--------Accounts Logic--------

    public T LoginAs<T>(Dictionary<string, string> loginDetails) where T : BaseModel, new()
    {
        return (T)Convert.ChangeType(DbAccess.Login<T>(loginDetails), typeof(T))!;
    }

}