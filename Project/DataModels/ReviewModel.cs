using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

public class ReviewModel
{
    [Name("movieId")]
    public int MovieId { get; set; }

    [Name("accountId")]
    public int AccountId { get; set; }

    [Name("rating")]
    public int Rating { get; set; }

    [Name("review")]
    public string Review { get; set; }

    public ReviewModel(int movieId, int accountId, int rating, string review)
    {
        MovieId = movieId;
        AccountId = accountId;
        Rating = rating;
        Review = CutReviewMessage(review);
    }

    private string CutReviewMessage(string reviewMsg)
    {
        if (reviewMsg.Length > 255)
        {
            return reviewMsg.Substring(0, 255);
        }
        return reviewMsg;
    }

}