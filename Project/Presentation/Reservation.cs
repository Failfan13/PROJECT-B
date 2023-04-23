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
                "Choose format",
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

        actions.Add(() => Format.Start(timeSlot, CurrReservation.Seats));

        // Apply discount NEEDS CORRECT FUNTION
        actions.Add(() => Promo.Start());

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
        List<string> Movies = new List<string>();
        List<Action> Actions = new List<Action>();
        string Question = "which movie would you like to see?";

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
    // public static void TotalReservationCost(ReservationModel Ress)
    // {
    //     Console.Clear();

    //     PromoLogic PromoLogic = new();
    //     ReservationLogic ReservationLogic = new ReservationLogic();

    //     TimeSlotsLogic TimeSlotsLogic = new();

    //     TotalPriceModel TotalRess = ReservationLogic.GetTotalRess(Ress);

    //     double FinalPrice = 0.00;

    //     int promoId = Promo.Start();

    //     if (promoId != -1)
    //     {
    //         string DiscountCode = PromoLogic.GetById(promoId)!.Code;
    //         TotalRess = ReservationLogic.ApplyDiscount(DiscountCode, TotalRess);

    //         Ress.DiscountCode = DiscountCode;
    //         ReservationLogic.UpdateList(Ress);
    //     }

    //     Console.Clear();
    //     // Movie Data
    //     Console.WriteLine("\nMovie:");
    //     Console.WriteLine($"Title: {TotalRess.Movie.Title}\tPrice: €{TotalRess.Movie.Price}");

    //     // Seat Data
    //     Console.WriteLine("\nSeats:");
    //     foreach (SeatModel seat in TotalRess.Seats)
    //     {
    //         Console.WriteLine($"{seat.SeatRow(TimeSlotsLogic.GetById(Ress.TimeSlotId).Theater.Width)}\tPrice: €{seat.Price}");

    //         EmailLogic EmailLogic = new EmailLogic();
    //         double FinalPrice = 0.00;

    //         string subject = "Order summary";
    //         string body = "";
    //         string email = "";

    //         // Seat Data
    //         Console.WriteLine($"Order overview:");
    //         body += "Order overview:\n";
    //         Console.WriteLine("\nSeats:");
    //         body += $"\nChosen seats:\n\n";
    //         foreach (SeatModel seat in ress.Seats)
    //         {
    //             Console.WriteLine($"{seat.SeatRow(TimeSlotsLogic.GetById(ress.TimeSLotId).Theater.Width)}\tPrice: €{seat.Price}");
    //             body += $"Nr: {seat.SeatRow(TimeSlotsLogic.GetById(ress.TimeSLotId).Theater.Width)}\tPrice: €{seat.Price}\n";
    //             FinalPrice += seat.Price;
    //         }

    //         // Snack data
    //         if (Ress.Snacks != null)
    //         {
    //             Console.WriteLine("\nSnacks:");

    //             int MaxLength = Ress.GetSnacks().Max(snack => snack.Name.Length);
    //             foreach (KeyValuePair<SnackModel, int> keyValue in TotalRess.Snacks)
    //             {
    //                 int Tabs = (int)Math.Ceiling((MaxLength - keyValue.Key.Name.Length) / 8.0);
    //                 var price = (keyValue.Key.Price) * keyValue.Value;
    //                 Console.WriteLine($"{keyValue.Value}x{keyValue.Key.Name}\t{new string('\t', Tabs)}Price: €{price}");
    //             }
    //         }

    //         FinalPrice = TotalRess.FinalPrice;
    //         Console.Write("\nThe total cost of your order will be:");
    //         Console.Write($"€ " + FinalPrice + (FinalPrice.ToString().Contains(".") ? "" : ",-"));

    //         Console.WriteLine($"\n\nIMPORTANT\nYour order number is: {Ress.Id}\n");
    //         body += $"\nChosen snacks:\n\n";
    //         foreach (KeyValuePair<int, int> keyValue in ress.Snacks)
    //         {
    //             var Snack = new SnacksLogic().GetById(keyValue.Key);
    //             int Tabs = (int)Math.Ceiling((MaxLength - Snack.Name.Length) / 8.0);
    //             var price = (Snack.Price) * keyValue.Value;
    //             Console.WriteLine($"{keyValue.Value}x{Snack.Name}\t{new string('\t', Tabs)}Price: €{price}");
    //             body += $"{keyValue.Value}x {Snack.Name}\t{new string('\t', Tabs)}Price: €{price}\n";
    //             FinalPrice += price;
    //         }
    //     }

    //     email = UserLogin.AskEmail();

    //     EmailLogic.SendEmail(email, subject, body);

    //     UserLogin.SignUpMails();

    //     QuestionLogic.AskEnter();
    // }


    // Show total order amount
    public static void TotalReservationCost(ReservationModel Ress)
    {
        Console.Clear();

        PromoLogic PromoLogic = new();
        ReservationLogic ReservationLogic = new ReservationLogic();
        TimeSlotsLogic TimeSlotsLogic = new();

        TotalPriceModel TotalRess = ReservationLogic.GetTotalRess(Ress);

        double FinalPrice = 0.00;

        int promoId = Promo.Start();

        if (promoId != -1)
        {
            string DiscountCode = PromoLogic.GetById(promoId)!.Code;
            TotalRess = ReservationLogic.ApplyDiscount(DiscountCode, TotalRess);

            Ress.DiscountCode = DiscountCode;
            ReservationLogic.UpdateList(Ress);
        }

        Console.Clear();
        // Movie Data
        Console.WriteLine("\nMovie:");
        Console.WriteLine($"Title: {TotalRess.Movie.Title}\tPrice: €{TotalRess.Movie.Price}");

        // Seat Data
        Console.WriteLine("\nSeats:");
        foreach (SeatModel seat in TotalRess.Seats)
        {
            Console.WriteLine($"{seat.SeatRow(TimeSlotsLogic.GetById(Ress.TimeSlotId).Theater.Width)}\tPrice: €{seat.Price}");
        }

        // Snack data
        if (Ress.Snacks != null)
        {
            Console.WriteLine("\nSnacks:");

            int MaxLength = Ress.GetSnacks().Max(snack => snack.Name.Length);
            foreach (KeyValuePair<SnackModel, int> keyValue in TotalRess.Snacks)
            {
                int Tabs = (int)Math.Ceiling((MaxLength - keyValue.Key.Name.Length) / 8.0);
                var price = (keyValue.Key.Price) * keyValue.Value;
                Console.WriteLine($"{keyValue.Value}x{keyValue.Key.Name}\t{new string('\t', Tabs)}Price: €{price}");
            }
        }

        // Format data
        if (FormatsLogic.GetByFormat(Ress.Format) != null) // Same list in MovieLogic _formats
        {
            FormatDetails? formatDt = FormatsLogic.GetByFormat(Ress.Format);

            string required = formatDt.Item;
            double requiredPrice = formatDt.Price;

            Console.WriteLine($"\nThe ordered movie plays in {Ress.Format} format therefore there is an extra fee");
            if (required != "")
            {
                Console.Write("Requirements:");
                Console.WriteLine($"\n{required}x{Ress.Seats.Count}\tPrice: €{requiredPrice * Ress.Seats.Count}");

                FinalPrice += requiredPrice * Ress.Seats.Count;
            }
        }

        // Total price
        FinalPrice += TotalRess.FinalPrice;
        Console.Write("\nThe total cost of your order will be:");
        Console.Write($"€ " + FinalPrice + (FinalPrice.ToString().Contains(".") ? "" : ",-"));

        Console.WriteLine($"\n\nIMPORTANT\nYour order number is: {Ress.Id}\n");
        QuestionLogic.AskEnter();
    }
    public static void ClearReservation(Action returnTo)
    {
        Console.Clear();
        string Question = @"Are you sure you want to delete this reservation?
This will reset all your progress for this reservation";
        List<string> Options = new List<string>() { "Yes", "No" };
        List<Action> Actions = new List<Action>() { };

        Actions.Add(() => Reservation.NoFilterMenu(true));
        Actions.Add(() => returnTo());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void FormatPrompt(Action goTo)
    {
        string question = $@"This movie timeslot requires a special viewing method, 
Would you still like to order for this timeslot?";
        List<string> options = new List<string>() { "Yes", "No" };
        List<Action> actions = new List<Action>();

        actions.Add(() => goTo());
        actions.Add(() => NoFilterMenu());

        MenuLogic.Question(question, options, actions);
    }
}