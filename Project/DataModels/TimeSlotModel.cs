using System.Text.Json.Serialization;
using Postgrest.Attributes;
using Postgrest.Models;

[Table("time_slots")]
public class TimeSlotModel : BaseModel, IIdentity
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("movie_id")]
    public int MovieId { get; set; }

    [Column("date_time")]
    public DateTime Start { get; set; }

    [Column("theatre")]
    public Helper Theatre { get; set; } = null!;

    [Column("format")]
    public string Format { get; set; }

    public TimeSlotModel NewTimeSlotModel() => NewTimeSlotModel(0, new DateTime(), new Helper(), "");
    public TimeSlotModel NewTimeSlotModel(int movieid, DateTime start, TheatreModel theatre, string format) => NewTimeSlotModel(movieid, start, new Helper(theatre.Id), format);
    public TimeSlotModel NewTimeSlotModel(int movieid, DateTime start, Helper theatre, string format)
    {
        MovieId = movieid;
        Start = start;
        Theatre = theatre;
        Format = format;
        return this;
    }

    public void Info()
    {
        MoviesLogic tempMLogic = new MoviesLogic();
        Console.WriteLine($"Start:\t\t{Start}");
        Console.WriteLine($"Theatre:\t{Theatre}");
        Console.WriteLine($"Format:\t\t{Format}");
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