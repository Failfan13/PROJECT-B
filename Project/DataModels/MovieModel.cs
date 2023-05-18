using Postgrest.Attributes;
using Postgrest.Models;

[Table("Movies")]
public class MovieModel : BaseModel
{
    [PrimaryKey("id, false")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = "";

    [Column("releaseDate")]
    public DateTime ReleaseDate { get; set; }

    [Column("director")]
    public string Director { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("duration")]
    public int Duration { get; set; }

    [Column("price")]
    public double Price { get; set; }

    [Column("categories")]
    public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();

    [Column("formats")]
    public List<string> Formats { get; set; } = new List<string>();

    [Column("followers")]
    public List<int> Followers { get; set; } = new List<int>();

    [Column("ads")]
    public bool Ads { get; set; } = false;

    [Column("reviews")]
    public ReviewHelper Reviews { get; set; } = new ReviewHelper();


    public MovieModel NewMovieModel(string title, DateTime releaseDate, string director, string description,
        int duration, double price)
    {
        Title = title;
        ReleaseDate = releaseDate;
        Director = director;
        Description = description;
        Duration = duration;
        Price = price;
        Ads = false;
        return this;
    }

    public class ReviewHelper
    {
        [Column("reviewAmount")]
        public int ReviewAmount { get; set; }

        [Column("reviewStars")]
        public double ReviewStars { get; set; }

        public ReviewHelper() // default constructor
        {
            ReviewAmount = 0;
            ReviewStars = 0;
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

// public class NewMovieModel
// {
//     public static MovieModel NewModel(string title, DateTime releaseDate, string director, string description, int duration, double price)
//     {
//         MovieModel m = new MovieModel();
//         m.Title = title;
//         m.ReleaseDate = releaseDate;
//         m.Director = director;
//         m.Description = description;
//         m.Duration = duration;
//         m.Price = price;
//         return m;
//     }
// }





