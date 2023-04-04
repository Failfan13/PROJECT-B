static class Logger
{
    public static void Log(string LogString)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        LogAccess.WriteLine($"{timestamp},{LogString}");
    }


}