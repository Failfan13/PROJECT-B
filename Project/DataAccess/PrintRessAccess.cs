static class PrintRessAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/reservations.txt"));

    private static void FileExistance()
    {
        // Add new file if deleted
        if (!File.Exists(path))
        {
            File.Create(path);
            return;
        }
    }

    public async static Task WriteReservation(string text)
    {
        FileExistance();

        Task task = new Task(() =>
        {
            Thread.Sleep(1000);
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(text);
                // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                // {
                //     csv.WriteRecord(text);
                // }
            }
        });

        await task;
    }
}