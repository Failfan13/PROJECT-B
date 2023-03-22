using System.Text.Json.Serialization;

public class SeatModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("row")]
    public int Row { get; set; }

    [JsonPropertyName("reserved")]
    public bool Reserved { get; set; }

    [JsonPropertyName("handicapped")]
    public bool Handicapped { get; set; }

    public SeatModel(int id, double price, int row, bool reserved, bool handicapped)
    {
        Id = id;
        Price = price;
        Row = row;
        Reserved = reserved;
        Handicapped = handicapped;
    }
}