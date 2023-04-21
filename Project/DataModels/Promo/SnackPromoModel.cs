using System.Text.Json.Serialization;


public class SnackPromoModel : PricePromoModel
{
    [JsonPropertyName("snackId")]
    public int SnackId { get; set; }

    [JsonPropertyName("snack")]
    public string Snack { get; set; }

    public SnackPromoModel(int snackId, string snack, double discount, bool flat = true)
        : base(discount, flat)
    {
        SnackId = snackId;
        Snack = snack;
    }
}