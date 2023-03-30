public static class Reservation
{
    static private ReservationLogic ReservationLogic = new();
    static private MoviesLogic MoviesLogic = new();
    static public ReservationModel CurrReservation = null;

    public static void Start()
    {
        bool CorrectInput = true;
        int movieInput;
        MovieModel movieChoice = null;
        List<object> timeSeats = new();

        Console.Clear();
        string Question = "Which movie would you like to see?";
        List<string> Options = new();

        foreach (MovieModel movie in MoviesLogic.AllMovies())
        {
            Options.Add(movie.Title);
            if (movie.Id == 9)
            {
                break;
            }
        }

        while (CorrectInput)
        {
            movieInput = MenuLogic.Question(Question, Options);

            try
            {
                movieChoice = MoviesLogic.GetById(movieInput - 1);
                timeSeats = TimeSlots.ShowAllTimeSlotsForMovie(movieChoice.Id, movieChoice.Title);

                if (timeSeats.Count == 0)
                {
                    CorrectInput = true;
                }
                else
                {
                    CorrectInput = false;
                    break;
                }

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
            CurrReservation.DateTime = (DateTime)timeSeats[0];
            // Add seats mf (SeatModel)timeSeats[1]
            ReservationLogic.UpdateList(CurrReservation);
        }
    }

    public static void EditReservation()
    {
        int awnser;
        string reservationDate;
        string reservationMovie;

        Console.Clear();

        // List all reservations with date, time & movie name
        foreach (ReservationModel reservation in ReservationLogic.Reservations)
        {
            reservationDate = reservation.DateTime.ToString("dd/MM/yy HH:mm");
            reservationMovie = MoviesLogic.GetById(reservation.TimeSLotId).Title;

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

        // Edit reservations menu
        string question = "Choose a reservation you want to edit from the menu.";
        List<string> options = new List<string>()
        {
            "Choose movie, time & seats",
            "Choose seats",
            "Change side snack",
            "Apply discount",
            "Return to previous menu"
        };
        // Actions reservations actions
        List<Action> actions = new();
        actions.Add(() => Reservation.Start());

        MenuLogic.Question(question, options, actions);
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
}