public static class FormatsLogic
{
    private static MoviesLogic ML = new MoviesLogic();
    public static bool _swapFormats = false;
    private static Dictionary<string, FormatDetails> _formats = new() {
        {"imax", new FormatDetails { Item = "", Price = 10 } },
        {"imax 3d", new FormatDetails { Item = "3D Glasses", Price = 15 } },
        {"rpx", new FormatDetails { Item = "", Price = 8 } },
        {"rpx 3d", new FormatDetails { Item = "3D Glasses", Price = 12.50 } },
        {"3d", new FormatDetails { Item = "3D Glasses", Price = 5 } },
    };
    public static Dictionary<string, FormatDetails> AllFormatDetails()
    {
        return _formats;
    }

    public static FormatDetails? GetByFormat(string format)
    {
        try
        {
            return AllFormatDetails()[format];
        }
        catch (System.Exception)
        {
            return null;
        }

    }

    public static List<string> AllFormats()
    {
        return _formats.Keys.ToList();
    }

    public static void SwapMode() => _swapFormats = !_swapFormats;

    public static void AddFormatToMovie(MovieModel movie, string format)
    {
        movie.Formats.Add(format);
        ML.UpdateList(movie);
    }

    public static void RemoveFormatFromMovie(MovieModel movie, string format)
    {
        movie.Formats.Remove(format);
        ML.UpdateList(movie);
    }

    public static void AddFormatToTimeslot(MovieModel movie, TimeSlotModel tsm, string format)
    {
        tsm.Format = format;
        ML.UpdateList(movie);
    }

    public static void RemoveFormatFromTimeslot(MovieModel movie, TimeSlotModel tsm, string format)
    {
        tsm.Format = "standard";
        ML.UpdateList(movie);
    }

}

public class FormatDetails
{
    public string? Item { get; set; }
    public double Price { get; set; }
}