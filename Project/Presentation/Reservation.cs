public static class Reservation
{
    static private ReservationLogic ReservationLogic = new();
    static private MoviesLogic MoviesLogic = new();
    static private TimeSlotsLogic TimeSlotsLogic = new();
    static private TheatherLogic TheatherLogic = new();
    static public ReservationModel CurrReservation = null;

    public static void EditReservation()
    {
        int awnser;
        string reservationDate;
        MovieModel reservationMovie;

        string Question = "Which reservation would you like to edit?";
        List<string> Options = new List<string>();
        // List all reservations with date, time & movie name
        foreach (ReservationModel reservation in ReservationLogic.Reservations)
        {
            reservationDate = reservation.DateTime.ToString("dd/MM/yy HH:mm");
            reservationMovie = MoviesLogic.GetById(reservation.TimeSLotId);

            Options.Add($"{reservation.Id}. {reservationDate} - {reservationMovie.Title}");
        }

        awnser = MenuLogic.Question(Question,Options);
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
                "Choose time & seats",
                "Choose seats",
                "Change side snack",
                "Apply discount",
                "Return to previous menu"
            };
        // Actions reservations actions
        List<Action> actions = new();
        TimeSlotModel timeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSLotId);
        var movieid = timeSlot.MovieId;
        // choose all
        actions.Add(() => Reservation.NoFilterMenu(true));

        //choose time & seats
        actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movieid, true));

        // choose seats
        actions.Add(() => TheatherLogic.ShowSeats(TheatherLogic.GetById(timeSlot.Theater), timeSlot, true));

        MenuLogic.Question(question, options, actions);
    }


    public static void NoFilterMenu(bool IsEdited = false)
    {
        Filter.CatIds = new List<int>();
        var movies = new MoviesLogic().AllMovies();

        string Question = "which movie would you like to see?";
        List<string> Movies = new List<string>();
        List<Action> Actions = new List<Action>();

        Movies.Add("Use Filter");
        Actions.Add(() => Filter.Main());


        foreach (MovieModel movie in movies)
        {
            Movies.Add(movie.Title);
            Actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movie.Id, IsEdited));
        }
        Movies.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Movies, Actions);
    }

    public static void FilteredMenu(List<MovieModel> movies, bool IsEdited = false)
    {
        string Question = "which movie would you like to see?";
        List<string> Movies = new List<string>();
        List<Action> Actions = new List<Action>();
        Movies.Add("Use Filter");
        Actions.Add(() => Filter.Main());

        foreach (MovieModel movie in movies)
        {
            Actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movie.Id, IsEdited));
        }
        Movies.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Movies, Actions);
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