using System.Text.Json.Serialization;

public class SeatModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("Type")]
    public string Type { get; set; }

    [JsonConstructor]
    public SeatModel(int id, string Type = "basic")
    {
        Id = id;
        Type = checkSeatType(Type);
    }

    public string checkSeatType(string seatType)
    {
        if (AllSeatTypes().Contains(seatType)) return seatType;
        else return "basic";
    }

    public List<string> AllSeatTypes()
    {
        return new List<string> {
            "basic",
            "standard",
            "luxury",
            "handicap"
        };
    }
}