using CsvHelper;
using System.Globalization;

static class LocationsAccess
{
    private static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/locations.csv"));

    private static void FileExistance()
    {
        if (!File.Exists(path))
        {
            File.Create(path);
        }
    }

    public static void Writer(List<LocationModel> data)
    {
        FileExistance();
        using (var writer = new StreamWriter(path))
        {
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
        }
    }


    public static List<LocationModel> ReadDataList()
    {
        FileExistance();
        using (var reader = new StreamReader(path))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<LocationModel>().ToList();
            };
        }
    }
}
