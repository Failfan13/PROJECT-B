using System.Text.Json.Serialization;
using System.Text.Json;
using Postgrest.Attributes;
using Postgrest.Models;

[Table("theatres")]
public class TheatreModel : BaseModel, ICloneable, IIdentity
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("copy_room_id")]
    public int CopyRoomId { get; set; } = -1;

    [Column("layout_specs")]
    public SeatBuilderHelper LayoutSpecs { get; set; }

    [Column("seat_prices")]
    public SeatPriceHelper SeatPrices { get; set; }

    [Column("room_width")]
    public int Width { get; set; }

    [Column("room_height")]
    public int Height { get; set; }


    public TheatreModel NewTheatreModel(double SeatPrice, int width, int height)
    {
        LayoutSpecs = new SeatBuilderHelper();
        SeatPrices = new SeatPriceHelper();
        SeatPrices.Basic = SeatPrice;
        SeatPrices.Standard = SeatPrice * 1.5;
        SeatPrices.Luxury = SeatPrice * 2;
        Width = width;
        Height = height;
        return this;
    }

    public class SeatBuilderHelper
    {
        [JsonPropertyName("pathwaysIndex")]
        public Tuple<string, int>[] PathwayIndexes { get; set; } = new Tuple<string, int>[] { };

        [JsonPropertyName("blockedSeats")]
        public int[] BlockedSeatIndexes { get; set; } = new int[] { };

        [JsonPropertyName("handicapSeats")]
        public int[] HandiSeatIndexes { get; set; } = new int[] { };
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