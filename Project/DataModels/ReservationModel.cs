using System.Text.Json.Serialization;


public class ReservationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("timeslot_id")]
    public int TimeSLotId { get; set; }

    [JsonPropertyName("seats")]
    public List<SeatModel> Seats { get; set; }

    [JsonPropertyName("account_id")]
    public int? AccountId { get; set; }

    [JsonPropertyName("date_time")]
    public DateTime DateTime { get; set; }

    public ReservationModel(int id, int timeSlotId, List<SeatModel> seats, int? accountId, DateTime dateTime)
    {
        Id = id;
        TimeSLotId = timeSlotId;
        AccountId = accountId;
        DateTime = dateTime;
        Seats = seats;
    }

}




