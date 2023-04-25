using System.Text.Json.Serialization;

public class TimeSlotModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("movieid")]
    public int MovieId { get; set; }

    [JsonPropertyName("start")]
    public DateTime Start { get; set; }

    [JsonPropertyName("theatre")]
    public TheatreModel Theatre { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonConstructor]
    public TimeSlotModel() : this(0, 0, new DateTime(), new TheatreModel(), "") { }
    public TimeSlotModel(int id, int movieid, DateTime start, TheatreModel Theatre, string format)
    {
        Id = id;
        MovieId = movieid;
        Start = start;
        Theatre = Theatre;
        Format = format;
    }

    public void Info()
    {
        MoviesLogic tempMLogic = new MoviesLogic();
        MovieModel movie = tempMLogic.GetById(MovieId);
        Console.WriteLine($"Start:\t\t{Start}");
        Console.WriteLine($"Theatre:\t{Theatre}");
        Console.WriteLine($"Format:\t\t{Format}");
        movie.Info();
    }
}