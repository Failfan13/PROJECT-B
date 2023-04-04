static class LogAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/log.csv"));
    private static StreamWriter _streamWriter = new StreamWriter(path, true);


    public static void WriteLine(string logline, string headers)
    {
        if (_streamWriter.BaseStream.Position.Equals(0))
        {
            _streamWriter.WriteLine(headers);
        }
        _streamWriter.WriteLine(logline);
        _streamWriter.Close();
    }
    public static List<Dictionary<string, string>> ReadCsv()
    {
        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        using (StreamReader reader = new StreamReader(path))
        {
            // Read headers
            string headersLine = reader.ReadLine();
            string[] headers = headersLine.Split(',');

            // Read data rows
            while (!reader.EndOfStream)
            {
                string dataLine = reader.ReadLine();
                string[] dataFields = dataLine.Split(',');

                Dictionary<string, string> dataRow = new Dictionary<string, string>();

                for (int i = 0; i < dataFields.Length; i++)
                {
                    dataRow[headers[i]] = dataFields[i];
                }

                data.Add(dataRow);
            }
        }

        return data;
    }

}