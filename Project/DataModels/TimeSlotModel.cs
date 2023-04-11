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
    public TheaterModel Theater { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    public TimeSlotModel(int id, int movieid, DateTime start, TheaterModel theater, string format)
    {
        Id = id;
        MovieId = movieid;
        Start = start;
        Theater = theater;
        Format = Format;
    }

    public void Info()
    {
        MoviesLogic tempMLogic = new MoviesLogic();
        MovieModel movie = tempMLogic.GetById(MovieId);
        Console.WriteLine($"Start:\t\t{Start}");
        Console.WriteLine($"Theater:\t{Theater}");
        Console.WriteLine($"Format:\t\t{Format}");
        movie.Info();
    }
}