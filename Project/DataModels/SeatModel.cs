using System.Text.Json.Serialization;

public class SeatModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("handicapped")]
    public bool Handicapped { get; set; }

    [JsonPropertyName("luxury")]
    public bool Luxury { get; set; }

    [JsonConstructor]
    public SeatModel(int id, bool handicapped = false, bool luxury = false)
    {
        Id = id;
        Handicapped = handicapped;
        Luxury = luxury;
    }
}