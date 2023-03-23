using System.Text.Json.Serialization;

public class TheaterModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("rows")]
    public List<RowModel> Rows { get; set; }

    [JsonPropertyName("theaternumber")]
    public int TheaterNumber { get; set; }

    [JsonConstructor]
    public TheaterModel(int id, List<RowModel> rows, int theaterNumber)
    {
        Id = id;
        Rows = rows;
        TheaterNumber = theaterNumber;
    }
}