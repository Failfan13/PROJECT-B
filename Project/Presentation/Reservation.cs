public static class Reservation
{
    static private ReservationLogic ReservationLogic = new();
    static private MoviesLogic MoviesLogic = new();
    public static void start()
    {
        bool CorrectInput = true;

        MovieModel choice = null;
        Console.Clear();
        Console.WriteLine("Which movie would you like to see?");
        foreach (MovieModel movie in MoviesLogic.AllMovies())
        {
            Console.WriteLine($"{movie.Id + 1}. {movie.Title}");
        }

        while (CorrectInput)
        {
            int awnser = QuestionLogic.AskNumber("\nEnter number to continue:", true);
            try
            {
                choice = MoviesLogic.GetById(awnser);
                ShowMovieTimeSlots(choice);
                break;
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Incorrect number");
            }
        }

        ReservationLogic.MakeReservation(1, new List<int>() { 1, 2, 3 });
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
        string reservationDate;
        string reservationMovie;

        Console.Clear();
        Console.WriteLine("Choose a reservation you want to edit from the menu.");

        foreach (ReservationModel reservation in ReservationLogic.Reservations)
        {
            reservationDate = reservation.DateTime.ToString("dd/MM/yy_HH:mm");
            reservationMovie = MoviesLogic.GetById(reservation.MovieId - 1).Title;
            Console.WriteLine($"{reservation.Id}. {reservationDate} - {reservationMovie}");
        }

        awnser = QuestionLogic.AskNumber("\nEnter number to continue:", true);
    }
}
