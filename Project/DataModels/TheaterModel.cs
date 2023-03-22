using System.Text.Json.Serialization;

public class TheaterModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public List<SeatModel?> Seats { get; set; }

    [JsonPropertyName("theaternumber")]
    public int TheaterNumber { get; set; }


    public TheaterModel(int id, List<SeatModel> seats, int theaterNumber)
    {
        Id = id;
        Seats = seats;
        TheaterNumber = theaterNumber;
    }
}