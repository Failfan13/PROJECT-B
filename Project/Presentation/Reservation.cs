public static class Reservation
{
    static private ReservationLogic ReservationLogic = new();
    public static void start()
    {
        bool CorrectInput = true;

        MovieModel choice = null;
        Console.Clear();
        Console.WriteLine("Which movie would you like to see?");
        var movies = new MoviesLogic();
        foreach (MovieModel movie in movies.AllMovies())
        {
            Console.WriteLine($"{movie.Id + 1}. {movie.Title}.");
        }
        Console.WriteLine($"{movies.AllMovies().Count + 1}. Return");

        while (CorrectInput)
        {
            int awnser = QuestionLogic.AskNumber("\nEnter number to continue:");
            try
            {
                if (awnser == movies.AllMovies().Count + 1)
                {
                   Menu.Start();
                   break;
                }
                choice = movies.GetById(awnser - 1);
                CorrectInput = false;
                TimeSlots.ShowAllTimeSlotsForMovie(choice.Id, choice.Title);
                break;
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Incorrect number");
            }
        }
    }

    // Increases total order amount
    //ReservationLogic.TotalOrder = 5.5;

    // Decreases total order amount
    //ReservationLogic.TotalOrderDecr = 2.3;

    // Show total order amount
    public static void TotalReservationCost()
    {
        Console.Write("The total cost of your order will be:\n");
        //Order as string
        string orderCost = ReservationLogic.TotalOrder.ToString();
        //Show euro symbol
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        //Print total cost + if not containing "." add ",-" at end
        Console.WriteLine($"€ " + orderCost + (orderCost.Contains(".") ? "" : ",-"));
    }
}
