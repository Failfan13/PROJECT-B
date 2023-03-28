using System.Text.Json.Serialization;


public class MovieModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("release_date")]
    public DateTime ReleaseDate { get; set; }

    [JsonPropertyName("director")]
    public string Director { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("categories")]
    public List<string> Categories{ get; set; }

    public MovieModel(int id, string title, DateTime releaseDate, string director, string description, int duration, List<string> categories)
    {
        Id = id;
        Title = title;
        ReleaseDate = releaseDate;
        Director = director;
        Description = description;
        Duration = duration;
        Categories = categories;
    }

    public void Info()
    {
        Console.WriteLine($"Title:      \t{Title}");
        Console.WriteLine($"Description:\t{Description}");
        Console.WriteLine($"Duration:   \t{Duration}");
        Console.WriteLine($"Director:   \t{Director}");
        Console.WriteLine($"ReleaseDate:\t{ReleaseDate.Day}-{ReleaseDate.Month}-{ReleaseDate.Year}");
        Console.WriteLine($"Categories: \t{string.Join(", ", Categories)}");
        Console.Write("\n");
    }

}




