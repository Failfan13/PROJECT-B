using Postgrest.Attributes;
using Postgrest.Models;

[Table("reservations")]
public class ReservationModel : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("account_id")]
    public int? AccountId { get; set; }

    [Column("timeslot_id")]
    public int TimeSlotId { get; set; }

    [Column("seats")]
    public List<SeatModel> Seats { get; set; }

    [Column("snacks")]
    public Dictionary<int, int>? Snacks { get; set; }

    [Column("date_time")]
    public DateTime DateTime { get; set; }

    [Column("format")]
    public string Format { get; set; }

    [Column("discount_code")]
    public string DiscountCode { get; set; }

    public ReservationModel NewReservationModel(int timeSlotId, List<SeatModel> seats, Dictionary<int, int> snacks, int? accountId, DateTime dateTime, string format)
    {
        TimeSlotId = timeSlotId;
        AccountId = accountId;
        DateTime = dateTime;
        Seats = seats;
        Snacks = snacks;
        Format = format;
        return this;
    }

    public List<SnackModel> GetSnacks()
    {
        List<SnackModel> snacks = new List<SnackModel>();
        SnacksLogic SL = new SnacksLogic();

        foreach (var item in Snacks.Keys)
        {
            snacks.Add(SL.GetById(item));
        }
        return snacks;
    }

}

public class TotalPriceModel
{
    public MovieModel Movie { get; set; }
    public int TheatreId { get; set; }
    public double[][] Seats { get; set; }
    public Dictionary<SnackModel, int> Snacks { get; set; }
    public double FinalPrice { get; set; }

    public TotalPriceModel(MovieModel movie, int theatreId, double[][] seats, Dictionary<SnackModel, int> snacks)
    {
        Movie = movie;
        TheatreId = theatreId;
        Seats = seats;
        Snacks = snacks;
    }
}



