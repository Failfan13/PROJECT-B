using System.Text.Json.Serialization;

public class TheatreModel
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

    public TheatreModel() : this(0, new List<SeatModel>(), 0, 0) { }
    public TheatreModel(int id, List<SeatModel> seats, int width, int height)
    {
        Id = id;
        Seats = seats;
        Width = width;
        Height = height;
    }
}