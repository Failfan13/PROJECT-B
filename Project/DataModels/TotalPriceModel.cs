public class TotalPriceModel
{
    public MovieModel Movie { get; set; }
    public List<SeatModel> Seats { get; set; }
    public Dictionary<int, SnackModel> Snacks { get; set; }
    public double FinalPrice { get; set; }

    public TotalPriceModel(MovieModel movie, List<SeatModel> seats, Dictionary<int, SnackModel> snacks)
    {
        Movie = movie;
        Seats = seats;
        Snacks = snacks;
    }
}