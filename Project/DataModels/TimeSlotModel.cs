using System.Text.Json.Serialization;

public class TimeSlotModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("movieid")]
    public int MovieId { get; set; }

    [JsonPropertyName("start")]
    public DateTime Start { get; set; }

    [JsonPropertyName("theater")]
    public int Theater { get; set; } 

    public TimeSlotModel(int id, int movieid, DateTime start, int theater)
    {
        Id = id;
        MovieId = movieid;
        Start = start;
        Theater =theater;
    }
}