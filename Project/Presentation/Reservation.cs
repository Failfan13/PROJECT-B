public static class Reservation
{

    static private MoviesLogic MoviesLogic = new();
    static private TimeSlotsLogic TimeSlotsLogic = new();
    static private TheatherLogic TheatherLogic = new();
    static public ReservationModel CurrReservation = null;

    public static void EditReservation(bool AsAdmim = false)
    {
        ReservationLogic ReservationLogic = new ReservationLogic();
        int awnser;
        string reservationDate;
        MovieModel reservationMovie;

        string Question = "Which reservation would you like to edit?";
        List<string> Options = new List<string>();
        // List all reservations with date, time & movie name
        foreach (ReservationModel reservation in ReservationLogic.Reservations)
        {
            if (AccountsLogic.CurrentAccount.Id == reservation.AccountId || AsAdmim)
            {
                reservationDate = reservation.DateTime.ToString("dd/MM/yy HH:mm");
                var timeslotVar = TimeSlotsLogic.GetById(reservation.TimeSlotId);
                reservationMovie = MoviesLogic.GetById(timeslotVar.MovieId);

                Options.Add($"{reservationDate} - {reservationMovie.Title}");
            }

        }

        awnser = MenuLogic.Question(Question, Options);
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

        // Make seats from reservation open again
        var CurrSeat = CurrReservation.Seats;
        var CurrTimeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSlotId);

        foreach (SeatModel seat in CurrSeat)
        {
            var TheatherSeat = CurrTimeSlot.Theater.Seats.FirstOrDefault(s => s.Id == seat.Id);
            TheatherSeat.Reserved = false;
        }

        TimeSlotsLogic.UpdateList(CurrTimeSlot);


        // Edit reservations menu
        string question = "Choose a reservation you want to edit from the menu.";
        List<string> options = new List<string>()
            {
                "Choose movie, time & seats",
                "Choose time & seats",
                "Choose seats",
                "Change side snack",
                "Change discount code"
            };
        // Actions reservations actions
        List<Action> actions = new();
        TimeSlotModel timeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSlotId);
        var movieid = timeSlot.MovieId;
        // choose all
        actions.Add(() => Reservation.NoFilterMenu(true));

        //choose time & seats
        actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movieid, true));

        // choose seats
        actions.Add(() => Theater.SelectSeats(timeSlot, true));

        // Change snack
        actions.Add(() => Snacks.Start(timeSlot, CurrReservation.Seats, true));

        // Apply discount NEEDS CORRECT FUNTION
        actions.Add(() => Menu.Start());

        if (AccountsLogic.CurrentAccount.Admin && CurrReservation.AccountId == null)
        {
            options.Add("Add user ID");
            actions.Add(() => ReservationLogic.ChangeUserId(CurrReservation));
        }

        // Return to Login menu
        options.Add("Return");
        actions.Add(() => UserLogin.Start());

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

    // Show total order amount
    public static void TotalReservationCost(ReservationModel ress)
    {
        Console.Clear();

        PromoLogic PromoLogic = new();
        ReservationLogic ReservationLogic = new ReservationLogic();
        TimeSlotsLogic TimeSlotsLogic = new();

        TotalPriceModel TotalRess = ReservationLogic.GetTotalRess(ress);

        double FinalPrice = 0.00;

        // Discount code
        Console.WriteLine("Do you have a discount code? (y/n)");
        if (Console.ReadKey().Key == ConsoleKey.Y)
        {
            string DiscountCode = "";

            while (PromoLogic.FindPromo(DiscountCode))
            {
                if (DiscountCode != "")
                {
                    Console.WriteLine("\nDiscount has not been found please try again.");
                }
                Console.WriteLine("\nDiscount code:");
                DiscountCode = Console.ReadLine();
            }

            ress = ReservationLogic.ApplyDiscount(DiscountCode, TotalRess);
        }

        // Movie Data
        Console.WriteLine($"Order overview:");
        MovieModel movie = ReservationLogic.GetMovieFromRess(ress);
        Console.WriteLine($"\nMovie: {movie.Title}\tPrice: €{movie.Price}");

        // Seat Data
        Console.WriteLine("\nSeats:");
        foreach (SeatModel seat in ress.Seats)
        {
            Console.WriteLine($"{seat.SeatRow(TimeSlotsLogic.GetById(ress.TimeSlotId).Theater.Width)}\tPrice: €{seat.Price}");
            FinalPrice += seat.Price;
        }

        // Snack data
        if (ress.Snacks != null)
        {
            int MaxLength = ress.GetSnacks().Max(snack => snack.Name.Length);
            Console.WriteLine("\nSnacks:");
            foreach (KeyValuePair<int, int> keyValue in ress.Snacks)
            {
                var Snack = new SnacksLogic().GetById(keyValue.Key);
                int Tabs = (int)Math.Ceiling((MaxLength - Snack.Name.Length) / 8.0);
                var price = (Snack.Price) * keyValue.Value;
                Console.WriteLine($"{keyValue.Value}x{Snack.Name}\t{new string('\t', Tabs)}Price: €{price}");
                FinalPrice += price;
            }
        }

        Console.Write("\nThe total cost of your order will be:");
        var priceString = Convert.ToString(FinalPrice);
        //Show euro symbol
        //Print total cost + if not containing "." add ",-" at end
        Console.Write($"€ " + FinalPrice + (priceString.Contains(".") ? "" : ",-"));
        Console.WriteLine($"\n\nIMPORTANT\nYour order number is: {ress.Id}\n");
        QuestionLogic.AskEnter();
    }
}