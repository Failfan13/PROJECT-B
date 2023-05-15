using System.Text.Json.Serialization;


public class ReservationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("timeslot_id")]
    public int TimeSlotId { get; set; }

    [JsonPropertyName("seats")]
    public List<SeatModel> Seats { get; set; }

    [JsonPropertyName("snacks")]
    public Dictionary<int, int>? Snacks { get; set; }

    [JsonPropertyName("account_id")]
    public int? AccountId { get; set; }

    [JsonPropertyName("date_time")]
    public DateTime DateTime { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("discountCode")]
    public string DiscountCode { get; set; } = "";

    public ReservationModel(int id, int timeSlotId, List<SeatModel> seats, Dictionary<int, int> snacks, int? accountId, DateTime dateTime, string format)
    {
        Id = id;
        TimeSlotId = timeSlotId;
        AccountId = accountId;
        DateTime = dateTime;
        Seats = seats;
        Snacks = snacks;
        Format = format;
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



