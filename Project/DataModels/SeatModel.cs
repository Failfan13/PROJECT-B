using System.Text.Json.Serialization;

public class SeatModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("reserved")]
    public bool Reserved { get; set; }

    [JsonPropertyName("handicapped")]
    public bool Handicapped { get; set; }

    [JsonPropertyName("luxury")]
    public bool Luxury { get; set; }

    [JsonPropertyName("visible")]
    public bool Visiable { get; set; } = true;

    [JsonConstructor]
    public SeatModel(int id, double price, bool reserved = false, bool handicapped = false, bool luxury = false)
    {
        Id = id;
        Price = price;
        Reserved = reserved;
        Handicapped = handicapped;
        Luxury = luxury;
    }
}