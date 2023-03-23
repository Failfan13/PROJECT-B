using System.Text.Json.Serialization;

public class SeatModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("row")]
    public char Row { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("reserved")]
    public bool Reserved { get; set; }

    [JsonPropertyName("handicapped")]
    public bool Handicapped { get; set; }


    [JsonConstructor]
    public SeatModel(int id, char row, double price, bool reserved, bool handicapped)
    {
        Id = id;
        Price = price;
        Reserved = reserved;
        Row = row;
        Handicapped = handicapped;
    }
}