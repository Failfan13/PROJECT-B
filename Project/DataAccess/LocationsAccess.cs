using System.IO;

static class LocationAccess
{
    private static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/locations.csv"));
    public static void Writer(string data)
    {
        // Overwrite the file with the new data
        File.WriteAllText(path, data);
    }


    public static List<string> ReadDataList()
    {
        List<string> lines = new List<string>();

        // Read the data from the CSV file
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }
}
