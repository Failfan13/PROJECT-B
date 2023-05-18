public static class DbLogic
{
    public static int GetNewestId(string tableName = "movies")
    {
        return DbAccess.ExecuteQuery<int>("testje", new Dictionary<string, object>() { { "t_name", tableName } }).Result;
    }
}