public static class Reservation
{
    static private ReservationLogic ReservationLogic = new();
    static private MoviesLogic MoviesLogic = new();
    static public ReservationModel CurrReservation = null;

    public static void Start()
    {
        bool CorrectInput = true;
        MovieModel movieChoice = null;
        DateTime movieTime = DateTime.MinValue;

        Console.Clear();
        Console.WriteLine("Which movie would you like to see?");
        foreach (MovieModel movie in MoviesLogic.AllMovies())
        {
            Console.WriteLine($"{movie.Id + 1}. {movie.Title}");
        }

        while (CorrectInput)
        {
            int awnser = QuestionLogic.AskNumber("\nEnter number to continue:", MoviesLogic.AllMovies().Count);
            try
            {
                movieChoice = MoviesLogic.GetById(awnser);
                movieTime = Movies.ShowMovieTimeSlots(movieChoice);
                break;
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Incorrect number");
            }
        }

        if (CurrReservation == null)
        {
            // Make reservation in here 💀

            // Call seats here
            // Call snacks here
            // Snacks.Start()
            // Call discount here

            // Make reservation
        }
        else
        {
            // Edit the chosen reservation
            CurrReservation.TimeSLotId = movieChoice.Id;
            CurrReservation.DateTime = movieTime;
            ReservationLogic.UpdateList(CurrReservation);
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

        string orderCost = ReservationLogic.TotalOrder.ToString();
        //Show euro symbol
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        //Print total cost + if not containing "." add ",-" at end
        Console.WriteLine($"€ " + orderCost + (orderCost.Contains(".") ? "" : ",-"));
    }

    public static void EditReservation()
    {
        int awnser;
        string reservationDate;
        string reservationMovie;

        Console.Clear();
        Console.WriteLine("Choose a reservation you want to edit from the menu.");

        // List all reservations with date, time & movie name
        foreach (ReservationModel reservation in ReservationLogic.Reservations)
        {
            reservationDate = reservation.DateTime.ToString("dd/MM/yy HH:mm");
            reservationMovie = MoviesLogic.GetById(reservation.TimeSLotId - 1).Title;

            Console.WriteLine($"{reservation.Id}. {reservationDate} - {reservationMovie}");
        }

        awnser = QuestionLogic.AskNumber("\nEnter number to continue:", ReservationLogic.Reservations.Count());
        // Set current reservation field
        try
        {
            CurrReservation = ReservationLogic.Reservations[awnser];
        }
        catch (System.IndexOutOfRangeException)
        {
            Console.WriteLine("No existing reservation found");
            return;
        }

        // Call changeables
        Console.Write("1. Choose movie & time & seats\n");
        Console.Write("2. Choose seats\n");
        Console.Write("3. Change side snack\n");
        Console.Write("4. Apply discount\n");
        Console.WriteLine("5. Return to previous menu");

        awnser = QuestionLogic.AskNumber("\nEnter number to continue:", 5);

        // Enter option for changes
        switch (awnser)
        {
            case 0:
                // to default reservation screen
                Reservation.Start();
                break;
            case 1:
                // to seats menu
                break;
            case 2:
                //Snacks.Start();
                break;
            case 3:
                // to dicount apply menu
                break;
            case 4:
                return;
            default:
                return;
        }
    }
}