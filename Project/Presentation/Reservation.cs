using System.Globalization;
public static class Reservation
{

    static private MoviesLogic MoviesLogic = new();
    static private TimeSlotsLogic TimeSlotsLogic = new();
    static private TheatreLogic TheatreLogic = new();
    static public ReservationModel CurrReservation = null!;

    public async static void EditReservation(bool AsAdmin = false)
    {
        ReservationLogic ReservationLogic = new ReservationLogic();
        AccountsLogic AccountsLogic = new AccountsLogic();
        List<ReservationModel> allReservations = ReservationLogic.GetAllReservations().Result;
        List<ReservationModel> userFilteredReservations = new List<ReservationModel>();
        string reservationDate;
        MovieModel reservationMovie;
        int currAccId = AccountsLogic.CurrentAccount!.Id;

        // admin logged in ask account
        if (AsAdmin)
        {
            currAccId = AccountsLogic.GetAccountIdFromList();
        }

        // filter for user
        userFilteredReservations = allReservations.FindAll(r => r.AccountId == currAccId);

        string Question = "Which reservation would you like to edit?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        int errRess = 0;

        // List all reservations with date, time & movie name
        foreach (ReservationModel reservation in userFilteredReservations)
        {
            if (currAccId == reservation.AccountId || AsAdmin)
            {
                try
                {
                    reservationDate = reservation.DateTime.ToString("dd/MM/yy HH:mm");
                    var timeslotVar = TimeSlotsLogic.GetById(reservation.TimeSlotId);
                    reservationMovie = MoviesLogic.GetById(timeslotVar!.Result.MovieId)!.Result;

                    if (reservationMovie != null && reservationDate != null)
                    {
                        Options.Add($"{reservationMovie.Title} - {reservationDate}");
                        Actions.Add(() => CurrReservation = reservation);
                    }
                }
                catch
                {
                    errRess++;
                }
            }
        }

        // no reservations found
        if (userFilteredReservations.Count == 0 || userFilteredReservations.Count == errRess)
        {
            Console.Clear();
            Console.WriteLine("No reservations found for this account\n");
            QuestionLogic.AskEnter();
            return;
        }

        MenuLogic.Question(Question, Options, Actions);

        // set current seats & reservation timeslot
        var CurrSeat = CurrReservation.Seats;
        var CurrTimeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSlotId)!.Result;

        try
        {
            foreach (SeatModel seat in CurrSeat)
            {
                var TheatreSeat = CurrTimeSlot!.Theatre.Seats.FirstOrDefault(s => s.Id == seat.Id);
                //TheatreSeat.Reserved = false;
            }
        }
        catch { }

        int I = CurrTimeSlot.MovieId;
        Logger.LogDataChange<ReservationModel>(I, "Updated");
        TimeSlotsLogic.UpdateList(CurrTimeSlot).ConfigureAwait(false);

        // Edit reservations menu
        string question = "Choose a reservation you want to edit from the menu.";
        List<string> options = new List<string>()
            {
                "Choose movie, time & seats",
                "Choose time & seats",
                "Choose seats",
                "Change side snack",
                "Remove TimeSlot"
                //"Choose format", // will not be interchangable
                //"Change discount code" // will not be interchangable
            };
        List<Action> actions = new();

        TimeSlotModel timeSlot = TimeSlotsLogic.GetById(CurrReservation.TimeSlotId)!.Result;
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

        // // Change format  // will not be interchangeable
        // actions.Add(() => Format.Start(CurrTimeSlot, CurrReservation.Seats, true));

        // // Apply discount // will not be interchangable
        // actions.Add(() => Promo.Start());

        actions.Add(() => TimeSlotsLogic.DeleteTimeSlot(CurrTimeSlot.Id));

        if (AccountsLogic.CurrentAccount.Admin && CurrReservation.AccountId == null)
        {
            options.Add("Add user ID");
            actions.Add(() => ReservationLogic.ChangeUserId(CurrReservation));
        }

        // Return to Login menu
        options.Add("Return");
        actions.Add(() => Menu.Start());

        MenuLogic.Question(question, options, actions);
        await ReservationLogic.UpdateList(CurrReservation);
    }

    public static void FilterMenu(List<MovieModel> filteredList = null!, bool IsEdited = false)
    {
        MoviesLogic ML = new();
        bool ofAge = AccountsLogic.CheckOfAge();

        var movies = new MoviesLogic().AllMovies();

        string Question = "which movie would you like to see?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (filteredList != null)
        {
            movies = filteredList;
        }

        Options.Add("Use Filter");
        Actions.Add(() => Filter.Main());

        if (ofAge)
        {
            foreach (MovieModel movie in movies)
            {
                Options.Add(movie.Title);
                Actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movie.Id, IsEdited));
                Options.Add("~details");
                Actions.Add(() => ML.ShowMovieDetails(movie));
            }
        }
        else // not of age
        {
            foreach (MovieModel movie in movies.FindAll(m => !m.Categories.Any(c => c.Name == "18+")))
            {
                Options.Add(movie.Title);
                Actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movie.Id, IsEdited));
                Options.Add("~details");
                Actions.Add(() => ML.ShowMovieDetails(movie));
            }
        }

        Options.Add("Return");
        Actions.Add(() => Menu.Start());
        MenuLogic.Question(Question, Options, Actions);
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
        PromoModel DiscountModel = null!;
        int theatreId = TimeSlotsLogic.GetById(ress.TimeSlotId)!.Result.Theatre.TheatreId;

        if (promoId != -1)
        {
            DiscountModel = PromoLogic.GetById(promoId).Result;
            if (DiscountModel != null)
            {
                TotalRess = ReservationLogic.ApplyDiscount(DiscountModel.Id, TotalRess).Result;

                ress.DiscountCode = DiscountModel.Code;
                ReservationLogic.UpdateList(ress);
            }
        }

        Console.Clear();
        // Movie Data
        Console.WriteLine("\nMovie:");
        Console.WriteLine($"Title: {TotalRess.Movie.Title}\tPrice: €{TotalRess.Movie.Price}");

        // Seat Data
        Console.WriteLine("\nSeats:");
        for (int i = 0; i < TotalRess.Seats.Count(); i++)
        {
            Console.Write($"{TheatreLogic.SeatNumber(TheatreLogic.GetById(theatreId)!.Result.Width, (int)TotalRess.Seats[i][0])}");

            SeatModel currSeat = ress.Seats.Find(seat => seat.Id == (int)TotalRess.Seats[i][0])!;
            Console.Write($"\tType: {currSeat!.Type}");
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

        //------ Build Email ------ //

        string subject = "Confirmation order Cinema";
        string body = "";
        string email = "";

        body += @$"Thank you for your order!
Below you will find your order details

Order details:

Movie: {TotalRess.Movie.Title}

Movie time: {TimeSlotsLogic.GetById(ress.TimeSlotId)!.Result.Start}

Seats: 
";

        foreach (var seat in TotalRess.Seats)
        {
            body += $"{TheatreLogic.SeatNumber(TheatreLogic.GetById(theatreId)!.Result.Width, (int)seat[0])} \n";
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

        QuestionLogic.AskEnter();

        AskPrintOrEmail(AccountId, email, subject, body);

        AskMailSignup(AccountId);
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


    private static void AskPrintOrEmail(int AccountId, string email, string subject, string body)
    {
        AccountsLogic AccountsLogic = new AccountsLogic();
        EmailLogic EmailLogic = new EmailLogic();

        string Question = "Would you like to print your order or receive an email?";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>() { };

        Options.Add("E-mail");
        Actions.Add(() =>
        {
            if (AccountId != -1)
            {
                email = AccountsLogic.GetById(AccountId).Result.EmailAddress;
            }
            else
            {
                email = UserLogin.AskEmail();
            }

            EmailLogic.SendEmail(email, subject, body);
        });
        Options.Add("Print");
        Actions.Add(async () => await ReservationLogic.PrintRes(body));

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void AskMailSignup(int AccountId = -1)
    {
        EmailLogic EmailLogic = new EmailLogic();
        AccountsLogic AccountsLogic = new AccountsLogic();

        AccountModel account = null!;

        if (AccountId != -1)
            try
            {
                account = AccountsLogic.GetById(AccountId)!.Result;
                if (!account.AdMails)
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }


        string Question = "Would you like to sign up for ad-mails?";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>() { };

        Options.Add("Yes");
        Options.Add("No");
        Actions.Add(() =>
        {
            EmailLogic.SubscribeAds(account);
            Console.Clear();
            Console.WriteLine("You have subscribed to the ad emails.");
            QuestionLogic.AskEnter();
        });
        Actions.Add(() => QuestionLogic.AskEnter());
    }

    public static void MenuReservation()
    {
        ReservationLogic ReservationLogic = new ReservationLogic();

        Console.Clear();
        string Question = "Select an option\n";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>();

        // previous reservations
        Options.Add("Previous reservations");
        Actions.Add(() => ReservationLogic.PreviousReservations(AccountsLogic.CurrentAccount.Id));
        // future reservations
        Options.Add("Future reservations");
        Actions.Add(() => ReservationLogic.CurrentReservations(AccountsLogic.CurrentAccount.Id));
        // return
        Options.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }
}