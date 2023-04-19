using System.Text.Json.Serialization;


public class SeatPromoModel : PricePromoModel
{

    [JsonPropertyName("seat")]
    public string Seat { get; set; }

    public SeatPromoModel(string seatNum, double discount, bool flat = true)
        : base(discount, flat)
    {
        Seat = seatNum;
    }
}