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

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("categories")]
    public List<CategoryModel> Categories { get; set; }

    [JsonPropertyName("formats")]
    private List<string>? _formats;
    public List<string> Formats
    {
        get => _formats!;
        set
        {
            if (!value.Contains("standard")) value.Add("standard");
            _formats = value;
        }
    }

    [JsonPropertyName("followers")]
    public List<int> Followers { get; set; }

    [JsonPropertyName("reviews")]
    public ReviewHelper Reviews { get; set; }

    [JsonPropertyName("ads")]
    public bool Ads { get; set; } = false;

    [JsonConstructor]
    public MovieModel(int id, string title, DateTime releaseDate, string director, string description,
        int duration, double price, List<CategoryModel> categories, List<string> formats)
    {
        Id = id;
        Title = title;
        ReleaseDate = releaseDate;
        Director = director;
        Description = description;
        Duration = duration;
        Price = price;
        Categories = categories;
        Formats = formats;
        Followers = new List<int>();
        Reviews = new ReviewHelper();
    }

    public class ReviewHelper
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("stars")]
        public double Stars { get; set; }

        [JsonConstructor]
        public ReviewHelper() // default constructor
        {
            Amount = 0;
            Stars = 0;
        }
    }

    public void Info()
    {
        string cats = "";
        Console.WriteLine($"Title:      \t{Title}");
        Console.WriteLine($"Description:\t{Description}");
        Console.WriteLine($"Duration:   \t{Duration}");
        Console.WriteLine($"Director:   \t{Director}");
        Console.WriteLine($"ReleaseDate:\t{ReleaseDate.Day}-{ReleaseDate.Month}-{ReleaseDate.Year}");
        Console.WriteLine($"Price:   \t{Price}");
        if (Categories.Count != 0)
        {
            foreach (CategoryModel c in Categories)
            {
                cats += $"{c.Name}, ";
            }
            cats = cats.Remove(cats.Length - 1);
            cats = cats.Remove(cats.Length - 1);
            Console.WriteLine($"Categories: \t{cats}");
            Console.Write("\n");
        }
        else
        {
            Console.WriteLine($"No categories are assinged to this movie");
        }
        Console.WriteLine($"Formats:   \t{string.Join(", ", Formats)}");
    }
}




