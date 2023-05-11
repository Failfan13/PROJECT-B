using System.Text.Json.Serialization;

public class TheatreModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("layoutSpecs")]
    public SeatBuilderHelper LayoutSpecs { get; set; } = new SeatBuilderHelper();

    [JsonPropertyName("baseSeatPrice")]
    public double BasicSeatPrice { get; set; }

    [JsonPropertyName("stanSeatPrice")]
    public double StandardSeatPrice { get; set; }

    [JsonPropertyName("luxeSeatPrice")]
    public double LuxurySeatPrice { get; set; }

    [JsonPropertyName("roomWidth")]
    public int Width { get; set; }

    [JsonPropertyName("roomHeight")]
    public int Height { get; set; }

    [JsonConstructor]

    public TheatreModel() : this(0, 0, 0, 0) { }
    public TheatreModel(int id, double SeatPrice, int width, int height)
    {
        Id = id;
        BasicSeatPrice = SeatPrice;
        StandardSeatPrice = SeatPrice * 1.5;
        LuxurySeatPrice = SeatPrice * 2;
        Width = width;
        Height = height;
    }

    public class SeatBuilderHelper
    {
        [JsonPropertyName("pathwaysIndex")]
        public List<Tuple<string, int>> PathwayIndexes { get; set; } = new List<Tuple<string, int>>() { };

        [JsonPropertyName("blockedSeats")]
        public List<int> BlockedSeatIndexes { get; set; } = new List<int>() { };

        [JsonPropertyName("handicapSeats")]
        public List<int> HandiSeatIndexes { get; set; } = new List<int>() { };
    }
}