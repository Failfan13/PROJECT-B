using System.Globalization;
static class Logger
{
    private static string pathData = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/logdata.csv"));
    private static string pathSystem = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/logsystem.csv"));

    // call with a data change of a model
    public static void LogDataChange<T>(int modelId, string action)
    {
        string headers = "timestamp,type,id,action,user_id";

        // Make a string of model type
        var type = typeof(T);
        string typeString = type.ToString();

        // time is now
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
    public static void SystemLog(string action, int id)
    {
        string headers = "timestamp,action,user_id";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string finalString = $"{timestamp},{action},{AccountsLogic.UserId()}";

        LogAccess.WriteLine(finalString, headers, pathSystem);
    }

    public static List<int[]> ReportList(DateTime sDate, DateTime eDate)
    {
        MoviesLogic ML = new();
        var MVS = ML.AllMovies(true);
        List<int[]> LogData = new();
        var Logs = GetLogDataChange<ReservationModel>();
        foreach (MovieModel M in MVS)
        {
            LogData.Add(new int[4]{M.Id, 0, 0, 0});
        }
        foreach (var l in Logs)
        {
            if (Convert.ToDateTime(l["timestamp"]) > sDate && Convert.ToDateTime(l["timestamp"]) < eDate)
            {
                foreach(var I in LogData)
                {
                    if (I[0].ToString() == l["id"])
                    {
                        switch (l["action"])
                        {
                            case "Added":
                                I[1] += 1;
                                break;
                            case "Updated":
                                I[2] += 1;
                                break;
                            case "removed":
                                I[3] += 1;
                                break;
                            default:
                                break;
                        }
                
                    }
                }

            }
        }
        return LogData;
    }
}
