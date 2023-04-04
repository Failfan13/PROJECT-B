static class Logger
{
    private static string pathData = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/logdata.csv"));
    private static string pathSystem = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/logsystem.csv"));

    // call with a data change of a model
    public static void LogDataChange<T>(int modelId, string action)
    {
        string headers = "timestamp,type,id,action,user_id";

        var type = typeof(T);
        string typeString = type.ToString();

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string finalString = $"{timestamp},{typeString},{modelId},{action},{AccountsLogic.UserId()}";

        LogAccess.WriteLine(finalString, headers, pathData);
    }

    // call to get data based on a model
    public static List<Dictionary<string, string>> GetLogDataChange<T>()
    {
        var list = LogAccess.ReadCsv(pathData);
        List<Dictionary<string, string>> newList = new List<Dictionary<string, string>>();

        foreach (var item in list)
        {
            if (item["type"] == typeof(T).ToString())
            {
                newList.Add(item);
            }
        }

        return newList;
    }

    // system logs
    public static void SystemLog(string action)
    {
        string headers = "timestamp,action,user_id";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string finalString = $"{timestamp},{action},{AccountsLogic.UserId()}";

        LogAccess.WriteLine(finalString, headers, pathSystem);
    }

}
