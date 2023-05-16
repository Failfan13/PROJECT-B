using System.Globalization;
public static class Reservation
{

    static private MoviesLogic MoviesLogic = new();
    static private TimeSlotsLogic TimeSlotsLogic = new();
    static private TheatreLogic TheatreLogic = new();
    static public ReservationModel CurrReservation = null!;

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
                try
                {
                    reservationDate = reservation.DateTime.ToString("dd/MM/yy HH:mm");
                    var timeslotVar = TimeSlotsLogic.GetById(reservation.TimeSlotId);
                    reservationMovie = MoviesLogic.GetById(timeslotVar.MovieId);

                    if (reservationMovie != null && reservationDate != null)
                        Options.Add($"{reservationDate} - {reservationMovie.Title}");
                }
                catch { }
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

        // set current seats & reservation timeslot
        var CurrSeat = CurrReservation.Seats;
        var CurrTimeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSlotId);

        foreach (SeatModel seat in CurrSeat)
        {
            var TheatreSeat = CurrTimeSlot.Theatre.Seats.FirstOrDefault(s => s.Id == seat.Id);
            //TheatreSeat.Reserved = false;
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
        List<Action> actions = new();

        TimeSlotModel timeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSlotId);
        var movieId = timeSlot.MovieId;

        // choose all
        actions.Add(() => Reservation.FilterMenu(true));

        // change time & seats
        actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movieId, true));

        // change seats
        actions.Add(() => Parallel.Invoke(
            () => Theatre.DeselectCurrentSeats(CurrTimeSlot, CurrReservation),
            () => Theatre.SelectSeats(CurrTimeSlot, true))
        );

        // Change snack
        actions.Add(() => Snacks.Start(CurrTimeSlot, CurrReservation.Seats, true));

        actions.Add(() => Format.Start(CurrTimeSlot, CurrReservation.Seats));

        // Apply discount NEEDS CORRECT FUNTION
        actions.Add(() => Promo.Start());

        if (AccountsLogic.CurrentAccount.Admin && CurrReservation.AccountId == null)
        {
            options.Add("Add user ID");
            actions.Add(() => ReservationLogic.ChangeUserId(CurrReservation));
        }

        // Return to Login menu
        options.Add("Return");
        actions.Add(() => Menu.Start());

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

        Movies.Add("Use Filter");
        Actions.Add(() => Filter.Main());

        foreach (MovieModel movie in movies)
        {
            int age;
            //checks if the movie category is 18+ and if the user is not an adult so adult movies are not shown to underaged users
            if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.DateOfBirth != null)
            {
                DateTime dateOfBirth = DateTime.ParseExact( AccountsLogic.CurrentAccount.DateOfBirth, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                age = DateTime.Today.Year - dateOfBirth.Year;
            }
            else 
            {age = 1;}
            if(movie.Categories.Any(i => i.Name == "18+" && age < 18 ))
                {
                    continue;
                }
            Movies.Add(movie.Title);
            Actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movie.Id, IsEdited));            
        }
        Movies.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Movies, Actions);
    }


    public static void FilterMenu(bool IsEdited) => FilterMenu(null!, IsEdited);

    // Show total order amount
    public static void TotalReservationCost(ReservationModel ress, int AccountId = -1)
    {
        Console.Clear();

        PromoLogic PromoLogic = new();
        ReservationLogic ReservationLogic = new ReservationLogic();
        TimeSlotsLogic TimeSlotsLogic = new();
        TheatreLogic TheatreLogic = new();

        TotalPriceModel TotalRess = ReservationLogic.GetTotalRess(ress);
        AccountsLogic AccountsLogic = new();

        double FinalPrice = 0.00;
        int promoId = Promo.Start();
        string DiscountCode = "";
        int theatreId = TimeSlotsLogic.GetById(ress.TimeSlotId)!.Theatre.TheatreId;

        if (promoId != -1)
        {
            DiscountCode = PromoLogic.GetById(promoId)!.Code;
            TotalRess = ReservationLogic.ApplyDiscount(DiscountCode, TotalRess);

            ress.DiscountCode = DiscountCode;
            ReservationLogic.UpdateList(ress);
        }

        Console.Clear();
        // Movie Data
        Console.WriteLine("\nMovie:");
        Console.WriteLine($"Title: {TotalRess.Movie.Title}\tPrice: €{TotalRess.Movie.Price}");

        // Seat Data
        Console.WriteLine("\nSeats:");
        for (int i = 0; i < TotalRess.Seats.Count(); i++)
        {
            Console.Write($"{TheatreLogic.SeatNumber(TheatreLogic.GetById(theatreId)!.Width, (int)TotalRess.Seats[i][0])}");

            SeatModel currSeat = ress.Seats.Find(seat => seat.Id == (int)TotalRess.Seats[i][0])!;
            Console.Write($"\tType: {currSeat!.SeatType}");
            Console.WriteLine($"\tPrice: €{TotalRess.Seats[i][1]}");
        }

        // Snack data
        if (ress.Snacks != null && ress.Snacks.Count > 0)
        {
            Console.WriteLine("\nSnacks:");

            int MaxLength = ress.GetSnacks().Max(snack => snack.Name.Length);
            foreach (KeyValuePair<SnackModel, int> keyValue in TotalRess.Snacks)
            {
                int Tabs = (int)Math.Ceiling((MaxLength - keyValue.Key.Name.Length) / 8.0);
                var price = (keyValue.Key.Price) * keyValue.Value;
                Console.WriteLine($"{keyValue.Value}x{keyValue.Key.Name}\t{new string('\t', Tabs)}Price: €{price}");
            }
        }

        // Format data
        if (FormatsLogic.GetByFormat(ress.Format) != null) // Same list in MovieLogic _formats
        {
            FormatDetails formatDt = FormatsLogic.GetByFormat(ress.Format)!;

            string required = formatDt.Item!;
            double requiredPrice = formatDt.Price;

            Console.WriteLine($"\nThe ordered movie plays in {ress.Format} format therefore there is an extra fee");
            if (required != "")
            {
                Console.Write("Requirements:");
                Console.WriteLine($"\n{required}x{ress.Seats.Count}\tPrice: €{requiredPrice * ress.Seats.Count}");

                FinalPrice += requiredPrice * ress.Seats.Count;
            }
        }

        // Total price
        FinalPrice += TotalRess.FinalPrice;
        Console.Write("\nThe total cost of your order will be:");
        Console.Write($"€ " + FinalPrice + (FinalPrice.ToString().Contains(".") ? "" : ",-"));
        Console.WriteLine($"\n\nIMPORTANT\nYour order number is: {ress.Id}\n");

        // Send email
        EmailLogic EmailLogic = new EmailLogic();

        string subject = "Confirmation order Cinema";
        string body = "";
        string email = "";

        body += @$"Thank you for your order!
Below you will find your order details

Order details:

Movie: {TotalRess.Movie.Title}

Movie time: {TimeSlotsLogic.GetById(ress.TimeSlotId)!.Start}

Seats: 
";

        foreach (var seat in TotalRess.Seats)
        {
            body += $"{TheatreLogic.SeatNumber(TheatreLogic.GetById(theatreId)!.Width, (int)seat[0])} \n";
        }

        if (ress.Snacks != null && ress.Snacks.Count > 0)
        {
            body += $"\nSnacks:\n";
            foreach (var snack in TotalRess.Snacks)
            {
                body += $"{snack.Value}x{snack.Key.Name} \n";
            }
        }

        if (FormatsLogic.GetByFormat(ress.Format) != null)
        {
            FormatDetails formatDt = FormatsLogic.GetByFormat(ress.Format)!;

            string required = formatDt.Item!;
            double requiredPrice = formatDt.Price;

            body += @$"The ordered movie plays in {ress.Format} format therefore there is an extra fee";

            if (required != "")
            {
                body += $"\nRequirements:";
                body += $"\n{required}x{ress.Seats.Count} \tPrice: €{requiredPrice * ress.Seats.Count}";
            }
        }

        body += @$"
The total cost of your order is: €{FinalPrice.ToString() + (FinalPrice.ToString().Contains(".") ? "" : ",-")}

IMPORTANT
Your order number is: {ress.Id}
";

        if (AccountId != -1)
        {
            email = AccountsLogic.GetById(AccountId).EmailAddress;
        }
        else
        {
            email = UserLogin.AskEmail();
        }

        EmailLogic.SendEmail(email, subject, body);

        UserLogin.SignUpMails(email);

        QuestionLogic.AskEnter();
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