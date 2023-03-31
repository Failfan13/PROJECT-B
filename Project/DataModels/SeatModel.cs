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


    [JsonConstructor]
    public SeatModel(int id, double price, bool reserved = false, bool handicapped = false)
    {
        Id = id;
        Price = price;
        Reserved = reserved;
        Handicapped = handicapped;
    }
    public string SeatRow(int width)
    {
        List<char> letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        var Seat = (Id % width) + 1;
        var Row = (Math.Floor((double)(Id / width)));

        return $"{Seat}{letters[(int)Row]}";
    }
}