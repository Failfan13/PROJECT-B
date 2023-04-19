using System.Text.Json.Serialization;


public class PricePromoModel
{

    [JsonPropertyName("discount")]
    public double Discount { get; set; }

    // is the discount a flat number(true): 10 euros on total, or is it %(false): 10% on total;
    [JsonPropertyName("flat")]
    public bool Flat { get; set; }
    public PricePromoModel(double discount, bool flat = true)
    {
        Discount = discount;
        Flat = flat;
    }
}