using System.Text.Json.Serialization;

public class SeatModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seatType")]
    public string SeatType { get; set; }

    [JsonConstructor]
    public SeatModel(int id, string seatType = "basic")
    {
        Id = id;
        SeatType = checkSeatType(seatType);
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