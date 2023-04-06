using System.Text.Json.Serialization;


public class PricePromoModel : PromoModel, IPromo
{

    [JsonPropertyName("discount")]
    public double Discount { get; set; }

    // is the discount a flat number(true): 10 euros on total, or is it %(false): 10% on total;
    [JsonPropertyName("flat")]
    public bool Flat { get; set; }
    public PricePromoModel(int id, string code, double discount, bool flat = true) : base(id, code)
    {
        Discount = discount;
        Flat = flat;
    }

    public double ApplyDiscount(Dictionary<int, int> SnacksIdAmount)
    {
        return 10;
    }
}