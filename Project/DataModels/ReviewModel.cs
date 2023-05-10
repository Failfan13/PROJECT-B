using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

public class ReviewModel
{
    [Name("movieId")]
    public int MovieId { get; set; }

    [Name("accountId")]
    public int AccountId { get; set; }

    [Name("reviewDate")]
    public DateTime ReviewDate { get; set; }

    [Name("rating")]
    public double Rating { get; set; }

    [Name("review")]
    public string Review { get; set; }

    public ReviewModel(int movieId, int accountId, double rating, string review, DateTime reviewDate = default)
    {
        MovieId = movieId;
        AccountId = accountId;
        Rating = rating;
        ReviewDate = (reviewDate != default) ? reviewDate : DateTime.Now;
        Review = ReviewLogic.CutReviewMessage(review);
    }
}