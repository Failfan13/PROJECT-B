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
            Console.WriteLine($"{movie.Id + 1}. {movie.Title}");
        }

        while (CorrectInput)
        {
            int awnser = QuestionLogic.AskNumber("\nEnter number to continue:", true);
            try
            {
                choice = movies.GetById(awnser);
                ShowMovieTimeSlots(choice);
                break;
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Incorrect number");
            }
        }

        ReservationLogic.MakeReservation(2, new List<int>() { 1, 2, 3 });
        EditReservation();
    }

    public static void ShowMovieTimeSlots(MovieModel movie)
    {
        movie.Info();
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

    public static void EditReservation()
    {
        bool CorrectInput = true;
        int awnser;

        Console.Clear();
        Console.WriteLine("Choose a reservation you want to edit from the menu.");

        foreach (ReservationModel reservation in ReservationLogic.Reservations)
        {
            // 1. DD/MM/YY - HH:MM format (reservation.Id + 1 for indexing)
            Console.WriteLine(reservation.DateTime);
        }

        awnser = QuestionLogic.AskNumber("\nEnter number to continue:", true);
    }
}
