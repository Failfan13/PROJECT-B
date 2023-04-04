static class Logger
{
    private static string _pathReservation = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/logreservations.csv"));
    private static string _pathData = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/logdata.csv"));

    public static List<Dictionary<string, string>> ReadLog(string path, string type = null)
    {
        List<Dictionary<string, string>> oldList = LogAccess.ReadCsv(path);
        List<Dictionary<string, string>> newList = new List<Dictionary<string, string>>();
        if (type != null)
        {
            foreach (var item in oldList)
            {
                if (item["type"] == type)
                {
                    newList.Add(item);
                }
            }
            return newList;
        }
        else
        {
            return oldList;
        }
    }
    // call with a data change of a model
    public static void LogDataChange<T>(int modelId, string action)
    {
        string headers = "timestamp,type,id,action,user_id";

        var type = typeof(T);
        string typeString = type.ToString();

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string finalString = $"{timestamp},{typeString},{modelId},{action},{AccountsLogic.UserId()}";

        LogAccess.WriteLine(finalString, headers, _pathData);
    }

    // call to get data based on a model
    public static List<Dictionary<string, string>> GetLogDataChange<T>()
    {
        var list = LogAccess.ReadCsv(_pathData);
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

    public static void LogReservation(ReservationModel ress)
    {
        string headers = "timestamp,id,user_id";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string finalString = $"{timestamp},{ress.Id},{ress.AccountId}";

        LogAccess.WriteLine(finalString, headers, _pathReservation);
    }

    public static List<Dictionary<string, string>> GetLogReservation()
    {
        var list = LogAccess.ReadCsv(_pathReservation);
        return list;
    }
}
