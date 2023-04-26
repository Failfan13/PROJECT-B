using System.Text.Json.Serialization;

public class TheatreModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("predefinedSeats")]
    public List<SeatModel> PreDefSeats { get; set; } = null!;

    [JsonPropertyName("roomWidth")]
    public int Width { get; set; }

    [JsonPropertyName("roomHeight")]
    public int Height { get; set; }

    [JsonConstructor]

    public TheatreModel() : this(0, 0, 0) { }
    public TheatreModel(int id, int width, int height)
    {
        Id = id;
        Width = width;
        Height = height;
    }
}