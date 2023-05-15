using System.Text.Json.Serialization;


public class SnackPromoModel : PricePromoModel
{
    [JsonPropertyName("snackId")]
    public int SnackId { get; set; }

    [JsonPropertyName("snackName")]
    public string SnackName { get; set; }

    public SnackPromoModel(int snackId, string snackName, double discount, bool flat = true)
        : base(discount, flat)
    {
        SnackId = snackId;
        SnackName = snackName;
    }
}