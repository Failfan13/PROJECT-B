public class TotalPriceModel
{
    public MovieModel Movie { get; set; }
    public List<SeatModel> Seat { get; set; }
    public Dictionary<int, SnackModel> Snack { get; set; }
    public double FinalPrice { get; set; }

    public TotalPriceModel(MovieModel movie, List<SeatModel> seat, Dictionary<int, SnackModel> snack)
    {
        Movie = movie;
        Seat = seat;
        Snack = snack;
    }
}