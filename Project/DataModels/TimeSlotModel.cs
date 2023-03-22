using System.Text.Json.Serialization;

class TimeSlotModel
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

    public void Info()
    {
        MoviesLogic tempMLogic = new MoviesLogic();
        MovieModel movie = tempMLogic.GetById(MovieId);
        Console.WriteLine($"Start:\t\t{Start}");
        Console.WriteLine($"Theater:\t{Theater}");
        movie.Info();
    }
}