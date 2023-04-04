static class LogAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/log.csv"));
    private static StreamWriter _streamWriter = new StreamWriter(path);

    public static void WriteLine(string logline)
    {
        _streamWriter.WriteLine(logline);
        _streamWriter.Close();
    }

}