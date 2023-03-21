using System.Text.Json.Serialization;
public class ScreeningModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("movieid")]
    public int MovieId { get; set; }
    [JsonPropertyName("screeningtime")]
    public DateTime ScreeningTime { get; set; }
    [JsonPropertyName("screenid")]
    public int ScreenId { get; set; }
    [JsonPropertyName("chairs")]
    public Dictionary<string, bool> Chairs{get; set;}
    
    public ScreeningModel(int id, int movieid, DateTime screeningTime, int screenId, Dictionary<string, bool> chairs)
    {
        Id = id;
        MovieId = movieid;
        ScreeningTime = screeningTime;
        ScreenId = screenId;
        Chairs = chairs;
    }
}