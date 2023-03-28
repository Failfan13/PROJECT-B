using System.Text.Json.Serialization;

public class RowModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public List<SeatModel> Seats { get; set; }

    [JsonConstructor]
    public RowModel(int row, List<SeatModel> seats)
    {
        Id = row;
        Seats = seats;
    }
}