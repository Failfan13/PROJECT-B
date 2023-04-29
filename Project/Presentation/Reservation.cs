public static class Reservation
{

    static private MoviesLogic MoviesLogic = new();
    static private TimeSlotsLogic TimeSlotsLogic = new();
    static private TheatreLogic TheatreLogic = new();
    static public ReservationModel CurrReservation = null;

    public static void EditReservation(bool AsAdmin = false)
    {
        ReservationLogic ReservationLogic = new ReservationLogic();
        AccountsLogic AccountsLogic = new AccountsLogic();
        int awnser;
        string reservationDate;
        MovieModel reservationMovie;
        int currAccId = AccountsLogic.CurrentAccount!.Id;

        // admin logged in ask account
        if (AsAdmin)
        {
            currAccId = AccountsLogic.GetAccountIdFromList();
        }

        if (!ReservationLogic.Reservations.Any(r => r.AccountId == currAccId))
        {
            Console.Clear();
            Console.WriteLine("No reservations found for this account\n");
            QuestionLogic.AskEnter();
            return;
        }

        string Question = "Which reservation would you like to edit?";
        List<string> Options = new List<string>();
        // List all reservations with date, time & movie name
        foreach (ReservationModel reservation in ReservationLogic.Reservations)
        {
            if (currAccId == reservation.AccountId || AsAdmin)
            {
                reservationDate = reservation.DateTime.ToString("dd/MM/yy HH:mm");
                var timeslotVar = TimeSlotsLogic.GetById(reservation.TimeSLotId);
                reservationMovie = MoviesLogic.GetById(timeslotVar.MovieId);

                Options.Add($"{reservationDate} - {reservationMovie.Title}");
            }
        }

        awnser = MenuLogic.Question(Question, Options);
        // Set current reservation field
        try
        {
            CurrReservation = ReservationLogic.Reservations.FindAll(r => r.AccountId == currAccId)[awnser];
        }
        catch (System.IndexOutOfRangeException)
        {
            Console.WriteLine("No existing reservation found");
            return;
        }

        // Make seats from reservation open again
        var CurrSeat = CurrReservation.Seats;
        var CurrTimeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSLotId);

        foreach (SeatModel seat in CurrSeat)
        {
            var TheatreSeat = CurrTimeSlot.Theatre.Seats.FirstOrDefault(s => s.Id == seat.Id);
            TheatreSeat.Reserved = false;
        }

        TimeSlotsLogic.UpdateList(CurrTimeSlot);


        // Edit reservations menu
        string question = "Choose a reservation you want to edit from the menu.";
        List<string> options = new List<string>()
            {
                "Choose movie, time & seats",
                "Choose time & seats",
                "Choose seats",
                "Choose side snack",
                "Choose format",
                "Apply discount"
            };
        // Actions reservations actions
        List<Action> actions = new();
        // TimeSlotModel timeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSLotId);
        var movieid = CurrTimeSlot.MovieId;
        // choose all
        actions.Add(() => Reservation.FilterMenu(true));

        //choose time & seats
        actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movieid, true));

        // choose seats
        actions.Add(() => Theatre.SelectSeats(CurrTimeSlot, true));

        // Change snack
        actions.Add(() => Snacks.Start(CurrTimeSlot, CurrReservation.Seats, true));

        actions.Add(() => Format.Start(CurrTimeSlot, CurrReservation.Seats));

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
        ReservationLogic.UpdateList(CurrReservation);
    }

    public static void FilterMenu(List<MovieModel> filteredList = null, bool IsEdited = false)
    //  public static void NoFilterMenu(bool IsEdited = false)
    {

        var movies = new MoviesLogic().AllMovies();

        string Question = "which movie would you like to see?";
        List<string> Movies = new List<string>();
        List<Action> Actions = new List<Action>();

        if (filteredList != null)
        {
            movies = filteredList;
        }

        //    MenuLogic.Question(Question, Movies, Actions);
        //  }
        //
        //public static void FilteredMenu(List<MovieModel> movies, bool IsEdited = false)
        //{
        //   List<string> Movies = new List<string>();
        //   List<Action> Actions = new List<Action>();
        //   string Question = "which movie would you like to see?";

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


    public static void FilterMenu(bool IsEdited) => FilterMenu(null, IsEdited);

    public static void TotalReservationCost(ReservationModel ress, int AccountId = -1, bool IsEdited = false)
    {
        Console.Clear();
        ReservationLogic ReservationLogic = new ReservationLogic();
        EmailLogic EmailLogic = new EmailLogic();
        double FinalPrice = 0.00;

        string subject = "Order summary";
        string body = "";
        string email = "";

        // Seat Data
        Console.WriteLine($"Order overview:");
        body += "Order overview:\n";
        Console.WriteLine("\nSeats:");
        body += $"\nChosen seats:\n\n";
        foreach (SeatModel seat in ress.Seats)
        {
            Console.WriteLine($"{seat.SeatRow(TimeSlotsLogic.GetById(ress.TimeSLotId).Theatre.Width)}\tPrice: €{seat.Price}");
            body += $"Nr: {seat.SeatRow(TimeSlotsLogic.GetById(ress.TimeSLotId).Theatre.Width)}\tPrice: €{seat.Price}\n";
            FinalPrice += seat.Price;
        }

        // Snack data
        if (ress.Snacks != null)
        {
            int MaxLength = ress.GetSnacks().Max(snack => snack.Name.Length);
            Console.WriteLine("\nSnacks:");
            body += $"\nChosen snacks:\n\n";
            foreach (KeyValuePair<int, int> keyValue in ress.Snacks)
            {
                var Snack = new SnacksLogic().GetById(keyValue.Key);
                int Tabs = (int)Math.Ceiling((MaxLength - Snack.Name.Length) / 8.0);
                var price = (Snack.Price) * keyValue.Value;
                Console.WriteLine($"{keyValue.Value}x{Snack.Name}\t{new string('\t', Tabs)}Price: €{price}");
                body += $"{keyValue.Value}x {Snack.Name}\t{new string('\t', Tabs)}Price: €{price}\n";
                FinalPrice += price;
            }
        }

        // Format data
        if (FormatsLogic.GetByFormat(ress.Format) != null) // Same list in MovieLogic _formats
        {
            FormatDetails? formatDt = FormatsLogic.GetByFormat(ress.Format);

            string required = formatDt.Item;
            double requiredPrice = formatDt.Price;

            Console.WriteLine($"\nThe ordered movie plays in {ress.Format} format therefore there is an extra fee");
            if (required != "")
            {
                Console.Write("Requirements:");
                Console.WriteLine($"\n{required}x{ress.Seats.Count}\tPrice: €{requiredPrice * ress.Seats.Count}");

                FinalPrice += requiredPrice * ress.Seats.Count;
            }
        }

        Console.Write("\nThe total cost of your order will be: ");
        var priceString = Convert.ToString(FinalPrice);

        //Show euro symbol
        //Print total cost + if not containing "." add ",-" at end
        Console.Write($"€" + FinalPrice + (priceString.Contains(".") ? "" : ",-"));
        body += $"€" + FinalPrice + (priceString.Contains(".") ? "" : ",-") + "\n";
        Console.WriteLine($"\n\nIMPORTANT\nYour order number is: {ress.Id}\n");
        body += $"\nIMPORTANT\nYour order number is: {ress.Id}";

        email = UserLogin.AskEmail();

        EmailLogic.SendEmail(email, subject, body);

        UserLogin.SignUpMails();
    }

    public static void ClearReservation(Action returnTo)
    {
        Console.Clear();
        string Question = @"Are you sure you want to delete this reservation?
This will reset all your progress for this reservation";
        List<string> Options = new List<string>() { "Yes", "No" };
        List<Action> Actions = new List<Action>() { };

        Actions.Add(() => Reservation.FilterMenu(true));
        Actions.Add(() => returnTo());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void FormatPrompt(Action returnTo)
    {
        string question = $@"This movie timeslot requires a special viewing method, 
Would you still like to order for this timeslot?";
        List<string> options = new List<string>() { "Yes", "No" };
        List<Action> actions = new List<Action>();

        actions.Add(() => returnTo());
        actions.Add(() => FilterMenu());

        MenuLogic.Question(question, options, actions);
    }
}