static class LogAccess
{

    public static void WriteLine(string logline, string headers, string path)
    {
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            // if the file is new, write headers;
            if (writer.BaseStream.Position.Equals(0))
            {
                writer.WriteLine(headers);
            }
            writer.WriteLine(logline);
        }
    }
    public static List<Dictionary<string, string>> ReadCsv(string path)
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