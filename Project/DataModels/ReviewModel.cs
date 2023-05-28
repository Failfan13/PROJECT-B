using Postgrest.Attributes;
using Postgrest.Models;

[Table("reviews")]
public class ReviewModel : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("movie_id")]
    public int MovieId { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }

    [Column("date_time")]
    public DateTime ReviewDate { get; set; }

    [Column("rating")]
    public double Rating { get; set; }

    [Column("review")]
    public string Review { get; set; }

    public ReviewModel NewReviewModel(int movieId, int accountId, double rating, string review, DateTime date = default)
    {
        MovieId = movieId;
        AccountId = accountId;
        Rating = rating;
        ReviewDate = (date != default) ? date : DateTime.Now;
        Review = ReviewLogic.CutReviewMessage(review);
        return this;
    }
}