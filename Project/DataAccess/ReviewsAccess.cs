using CsvHelper;
using System.Globalization;

static class ReviewsAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/reviews.csv"));

    public static List<ReviewModel> LoadReviews()
    {
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
        using (var writer = new StreamWriter(path))
        {
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(reviews);
            }
        }
    }
}