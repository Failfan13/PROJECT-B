public class TotalPriceModel
{
    public MovieModel Movie { get; set; }
    public List<SeatModel> Seats { get; set; }
    public Dictionary<SnackModel, int> Snacks { get; set; }
    public double FinalPrice { get; set; }

    public TotalPriceModel(MovieModel movie, List<SeatModel> seats, Dictionary<SnackModel, int> snacks)
    {
        Movie = movie;
        Seats = seats;
        Snacks = snacks;
    }
}