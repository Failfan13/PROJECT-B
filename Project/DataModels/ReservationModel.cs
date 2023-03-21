using System.Text.Json.Serialization;


public class ReservationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("movie_id")]
    public int MovieId { get; set; }

    [JsonPropertyName("seat_ids")]
    public List<int> SeatIds { get; set; }

    [JsonPropertyName("account_id")]
    public int? AccountId { get; set; }

    public ReservationModel(int id, int movieId, List<int> seatIds, int? accountId)
    {
        Id = id;
        MovieId = movieId;
        AccountId = accountId;
        SeatIds = seatIds;
    }

}




