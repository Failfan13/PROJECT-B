using System.Text.Json.Serialization;


public class MoviePromoModel : PricePromoModel
{
    [JsonPropertyName("movieId")]
    public int MovieId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    // specific true: promo is for a specific movie
    [JsonPropertyName("specific")]
    public bool Specific { get; set; } = false;

    public MoviePromoModel(int movieId, string title, double discount, bool flat = true)
        : base(discount, flat)
    {
        MovieId = movieId;
        Title = title;
    }
}