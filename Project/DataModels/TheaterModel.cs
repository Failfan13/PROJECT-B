using System.Text.Json.Serialization;

public class TheaterModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public List<SeatModel> Seats { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonConstructor]
    public TheaterModel(int id, List<SeatModel> seats, int width, int height)
    {
        Id = id;
        Seats = seats;
        Width = width;
        Height = height;
    }
}