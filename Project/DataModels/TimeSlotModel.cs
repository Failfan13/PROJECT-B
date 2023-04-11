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

    [JsonPropertyName("extra")]
    public TheaterModel Extra { get; set; }

    public TimeSlotModel(int id, int movieid, DateTime start, TheaterModel theater, string extra)
    {
        Id = id;
        MovieId = movieid;
        Start = start;
        Theater = theater;
        Extra = extra;
    }

    public void Info()
    {
        MoviesLogic tempMLogic = new MoviesLogic();
        MovieModel movie = tempMLogic.GetById(MovieId);
        Console.WriteLine($"Start:\t\t{Start}");
        Console.WriteLine($"Theater:\t{Theater}");
        Console.WriteLine($"Extras:\t{Extra}");
        movie.Info();
    }
}