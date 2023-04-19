using System.Text.Json.Serialization;


public class PromoModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; } = true;

    [JsonPropertyName("Condition")]
    public List<object>? Condition { get; set; }

    public PromoModel(int id, string code)
    {
        Id = id;
        Code = code;
    }
}