static class Logger
{

    private static void Log(string LogString, string type)
    {

        string[] headersL = { "Timestamp", "type", "log", "user" };
        string headers = string.Join(",", headersL);

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        LogAccess.WriteLine($"{timestamp},{type},{LogString},{AccountsLogic.UserName()}", headers);
    }

    public static List<Dictionary<string, string>> ReadLog(string type = null)
    {
        List<Dictionary<string, string>> oldList = LogAccess.ReadCsv();
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

    public static void LogDataChange(string title, string action)
    {

    }

    public static void LogReservation()
    {

    }


}