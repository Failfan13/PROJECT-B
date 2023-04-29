using System.Text.Json.Serialization;

public class SeatModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("handicapped")]
    public bool Handicapped { get; set; }

    [JsonPropertyName("seatType")]
    public string SeatType { get; set; }

    [JsonConstructor]
    public SeatModel(int id, bool handicapped = false, string seatType = "basic")
    {
        Id = id;
        Handicapped = handicapped;
        SeatType = seatType;
    }
}