using System.Text.Json.Serialization;
using System.Text.Json;

public class TheatreModel : ICloneable
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("copyRoomId")]
    public int CopyRoomId { get; set; } = -1;

    [JsonPropertyName("layoutSpecs")]
    public SeatBuilderHelper LayoutSpecs { get; set; } = new SeatBuilderHelper();

    [JsonPropertyName("seatPrices")]
    public SeatPriceHelper SeatPrices { get; set; }

    [JsonPropertyName("roomWidth")]
    public int Width { get; set; }

    [JsonPropertyName("roomHeight")]
    public int Height { get; set; }

    [JsonConstructor]

    public TheatreModel() : this(0, 0, 0, 0) { }
    public TheatreModel(int id, double SeatPrice, int width, int height)
    {
        Id = id;
        SeatPrices = new SeatPriceHelper();
        SeatPrices.Basic = SeatPrice;
        SeatPrices.Standard = SeatPrice * 1.5;
        SeatPrices.Luxury = SeatPrice * 2;
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

    public class SeatPriceHelper
    {
        [JsonPropertyName("baseSeatPrice")]
        public double Basic { get; set; }

        [JsonPropertyName("stanSeatPrice")]
        public double Standard { get; set; }

        [JsonPropertyName("luxeSeatPrice")]
        public double Luxury { get; set; }
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public object DeepClone()
    {
        return JsonSerializer.Deserialize<TheatreModel>(JsonSerializer.Serialize(this, this.GetType()))!;
    }
}