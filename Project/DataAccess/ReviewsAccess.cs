using CsvHelper;
using System.Globalization;

static class ReviewsAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/reviews.csv"));

    private static void FileExistance()
    {
        if (!File.Exists(path))
        {
            File.Create(path);
        }
    }

    public static List<ReviewModel> LoadReviews()
    {
        FileExistance();
        using (var reader = new StreamReader(path))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<ReviewModel>().ToList();
            };
        }
    }

    public static void WriteReviews(List<ReviewModel> reviews)
    {
        FileExistance();
        using (var writer = new StreamWriter(path))
        {
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(reviews);
            }
        }
    }
}