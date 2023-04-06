using System.Text.Json.Serialization;


public class AmountForAmountModel : PromoModel, IPromo
{

    [JsonPropertyName("items_id")]
    public List<int> SnacksID { get; set; }

    [JsonPropertyName("amount1")]
    public int Amount1 { get; set; }
    [JsonPropertyName("amount2")]
    public int Amount2 { get; set; }

    public AmountForAmountModel(int id, string code, List<int> snacksId, int amount1, int amount2) : base(id, code)
    {
        SnacksID = snacksId;
        Amount1 = amount1;
        Amount2 = Amount2;
    }

    public string Info()
    {
        return $"Promo: {Amount1} for {Amount2}";
    }

    public double? ApplyDiscount(Dictionary<int, int> SnacksIdAmount)
    {
        SnacksLogic SL = new SnacksLogic();

        // minimum amount of snacks needed to apply discount
        int minNeeded = Amount1 + Amount2;

        List<SnackModel> SnacksOnPromo = new List<SnackModel>();

        foreach (KeyValuePair<int, int> item in SnacksIdAmount)
        {
            if (SnacksID.Contains(item.Key))
            {
                SnacksOnPromo.Add(SL.GetById(item.Key));
            }
        }

        // check if contains enough snacks that are in this promo
        if (SnacksOnPromo.Count < minNeeded)
        {
            return null;
        }

        



        return 0;
    }
}