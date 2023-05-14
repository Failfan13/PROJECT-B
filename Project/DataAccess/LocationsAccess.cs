using System.IO;

static class LocationAccess
{
    public static void Writer(string data, string path)
    {
    // Overwrite the file with the new data
    File.WriteAllText(path, data);
    }
    

    public static List<string> ReadDataList(string path)
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
