using System.Text.Json.Serialization;


public class SeatPromoModel : PricePromoModel
{

    [JsonPropertyName("seatType")]
    public string SeatType { get; set; }

    [JsonPropertyName("seatAmount")]
    public string SeatAmount { get; set; }

    public SeatPromoModel(string seatType, string seatAmount, double discount, bool flat = true)
        : base(discount, flat)
    {
        SeatType = seatType;
        SeatAmount = seatAmount;
    }
}