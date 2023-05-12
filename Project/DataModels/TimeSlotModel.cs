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
    public Helper Theatre { get; set; } = null!;

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonConstructor]
    public TimeSlotModel(int id) : this(id, 0, new DateTime(), new Helper(), "") { }
    public TimeSlotModel(int id, int movieid, DateTime start, TheatreModel theatre, string format) : this(id, movieid, start, new Helper(theatre.Id), format) { }
    public TimeSlotModel(int id, int movieid, DateTime start, Helper theatre, string format)
    {
        Id = id;
        MovieId = movieid;
        Start = start;
        Theatre = theatre;
        Format = format;
    }

    public void Info()
    {
        MoviesLogic tempMLogic = new MoviesLogic();
        MovieModel movie = tempMLogic.GetById(MovieId)!;
        Console.WriteLine($"Start:\t\t{Start}");
        Console.WriteLine($"Theatre:\t{Theatre}");
        Console.WriteLine($"Format:\t\t{Format}");
        movie.Info();
    }
    public int NewId()
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        return TL.GetNewestId();
    }

    // Helps to deserialize theatre info
    public class Helper
    {
        [JsonPropertyName("id")]
        public int TheatreId { get; set; }

        [JsonPropertyName("reservedSeats")]
        public List<SeatModel> Seats { get; set; }

        [JsonConstructor]
        public Helper() : this(0, new List<SeatModel>()) { }
        public Helper(int theatreId) : this(theatreId, new List<SeatModel>()) { }
        public Helper(int theatreId, List<SeatModel> seats)
        {
            TheatreId = theatreId;
            Seats = seats;
        }
    }
}