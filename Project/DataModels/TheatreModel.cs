using System.Text.Json.Serialization;

public class TheatreModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("predefinedSeats")]
    public List<SeatModel> PreDefSeats { get; set; } = null!;

    [JsonPropertyName("outerSeatPrice")]
    public double OuterSeatPrice { get; set; }

    [JsonPropertyName("middSeatPrice")]
    public double MiddleSeatPrice { get; set; }

    [JsonPropertyName("innerSeatPrice")]
    public double InnerSeatprice { get; set; }

    [JsonPropertyName("roomWidth")]
    public int Width { get; set; }

    [JsonPropertyName("roomHeight")]
    public int Height { get; set; }

    [JsonConstructor]

    public TheatreModel() : this(0, 0, 0, 0) { }
    public TheatreModel(int id, double SeatPrice, int width, int height)
    {
        Id = id;
        OuterSeatPrice = SeatPrice;
        MiddleSeatPrice = SeatPrice * 1.5;
        OuterSeatPrice = SeatPrice * 2;
        Width = width;
        Height = height;
    }
}