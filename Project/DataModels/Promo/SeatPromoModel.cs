using System.Text.Json.Serialization;


public class SeatPromoModel : PricePromoModel
{

    [JsonPropertyName("seat")]
    public string Seat { get; set; }

    public SeatPromoModel(string seat, double discount, bool flat = true)
        : base(discount, flat)
    {
        Seat = seat;
    }
}