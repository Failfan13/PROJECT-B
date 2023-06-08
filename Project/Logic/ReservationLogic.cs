public class ReservationLogic
{
    public async Task<List<ReservationModel>> GetAllReservations()
    {
        return await DbLogic.GetAll<ReservationModel>();
    }
    // pass model to update
    public async Task UpdateList(ReservationModel ress)
    {
        await DbLogic.UpdateItem(ress);
    }

    public async Task UpsertList(ReservationModel ress)
    {
        await DbLogic.UpsertItem(ress);
    }

    public async Task<ReservationModel>? GetById(int id)
    {
        return await DbLogic.GetById<ReservationModel>(id);
    }

    public async Task<ReservationModel> NewReservation(int timeSlotId, List<SeatModel> seats, Dictionary<int, int> snacks, int? accountId, DateTime dateTime, string format)
    {
        ReservationModel ress = new ReservationModel();
        ress = ress.NewReservationModel(timeSlotId, seats, snacks, accountId, dateTime, format);
        await DbLogic.UpsertItem<ReservationModel>(ress);
        return ress;
    }

    public async Task UpdateReservation(ReservationModel ress) //Adds or changes category to list of categories
    {
        await UpdateList(ress);
    }

    public void DeleteReservation(int ressInt) // Deletes category from list of categories
    {
        // account exists and is admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            DbLogic.RemoveItemById<MovieModel>(ressInt);
        }
    }

    private int AsAdminId()
    {
        AccountsLogic AL = new AccountsLogic();
        int returner = -1;
        string Question = "Do you want to use your Admin Id or a User Id?";
        List<string> Options = new List<string>() { "User", "Admin" };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => returner = AL.GetAccountIdFromList());
        Actions.Add(() => returner = AccountsLogic.CurrentAccount!.Id);

        MenuLogic.Question(Question, Options, Actions);

        return returner;
    }

    public async Task MakeReservation(TimeSlotModel timeSlot, List<SeatModel> Seats, Dictionary<int, int> snacks = null!, string format = "standard", bool IsEdited = false)
    {
        Snacks.Continue = false;
        int AccountId = -1;
        ReservationModel ress = new ReservationModel();
        DateTime currDate = DateTime.Now;

        // if this reservation is made by an edit, use the id of the current reservation
        if (IsEdited)
        {
            // update curr reservation with old + new data
            if (Reservation.CurrReservation.AccountId == null) return;
            ress = Reservation.CurrReservation;
            ress.TimeSlotId = timeSlot.Id;
            ress.Seats = Seats;
            ress.Snacks = snacks;
            ress.Format = format;

            Console.Clear();
            Console.WriteLine("Reservation edited");
            QuestionLogic.AskEnter();

            await UpdateList(Reservation.CurrReservation);
        }
        else
        {
            try
            {
                AccountId = AccountsLogic.CurrentAccount!.Id;
                if (AccountsLogic.CurrentAccount.Admin)
                {
                    // asks if you want to use your own ID or that of an user
                    AccountId = AsAdminId();
                }
            }
            catch (System.Exception)
            {   // not logged in
                AccountId = -1;
            }

            // new reservation model
            ress = ress.NewReservationModel(timeSlot.Id, Seats, snacks, AccountId, currDate, format);

            // insert new reservation
            ress = DbLogic.InsertItem<ReservationModel>(ress).Result;

            // update total
            Reservation.TotalReservationCost(ress, AccountId);
        }

        // TimeSlotsLogic TL = new TimeSlotsLogic();
        // TL.UpdateList(timeSlot);
    }

    public async void ChangeUserId(ReservationModel Ress)
    {
        AccountsLogic AL = new AccountsLogic();
        int newUserId = AL.GetAccountIdFromList();

        Ress.AccountId = newUserId;
        await UpdateList(Ress);
    }

    public async Task<TotalPriceModel> ApplyDiscount(int discountId, TotalPriceModel ress)
    {

        PromoLogic PL = new PromoLogic();
        PromoModel PM = await PL.GetById(discountId)!;
        TimeSlotsLogic TL = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();

        List<PricePromoModel> PPM = PL.AllPrices(PM);
        PricePromoModel RPM = null!;
        List<MoviePromoModel> MPM = PL.AllMovies(PM);
        List<SeatPromoModel> SPM = PL.AllSeats(PM);
        SeatPromoModel RSPM = null!;
        List<SnackPromoModel> SP = PL.AllSnacks(PM);

        if (PM == null || !PM.Active) return ress;
        if (SPM.Count != 0) RSPM = SPM[0];

        // Movie check
        if (MPM.Count() != 0)
        {
            MoviePromoModel movie = MPM.Find(m => m.MovieId == ress.Movie.Id && m.Specific)!;

            //if (movie == null) movie = MPM.Find(m => m.Title == "all")!;

            if (movie != null)
            {
                double newPrice = PL.CalcAfterDiscount(ress.Movie.Price, movie.Discount, movie.Flat);
                ress.FinalPrice += newPrice - ress.Movie.Price;
                ress.Movie.Price = newPrice;
            }

            //if (movie == null) return ress;
        }

        // Seat check
        if (SPM.Count() != 0 && ress.Seats.Count() != 0)
        {
            TheatreLogic TL2 = new TheatreLogic();

            RSPM = SPM[0];
            bool allSeats = true;
            int loopFor = 0;

            if (RSPM.SeatAmount != "all") allSeats = false;

            if (allSeats) loopFor = ress.Seats.Count();
            else loopFor = int.Parse(RSPM.SeatAmount);

            try // calculate price & set new price
            {
                for (int i = 0; i < loopFor; i++)
                {
                    double newPrice = PL.CalcAfterDiscount(ress.Seats[i][1], RSPM.Discount, RSPM.Flat);
                    ress.FinalPrice += newPrice - ress.Seats[i][1];
                    ress.Seats[i][1] = newPrice;
                }
            }
            catch (System.Exception) { } // to little seats for loopFor
        }

        // Snack check
        if (SP.Count != 0)
        {
            // No Snacks
            try
            {
                foreach (var snack in SP)
                {
                    var snackie = ress.Snacks.FirstOrDefault(s => s.Key.Id == snack.SnackId);
                    double newPrice = PL.CalcAfterDiscount(snackie.Key.Price, snack.Discount, snack.Flat);
                    ress.FinalPrice += (newPrice - snackie.Key.Price) * snackie.Value;
                    snackie.Key.Price = newPrice;
                }
            }
            catch (System.Exception) { }
        }

        // Total check
        if (PPM.Count != 0)
        {
            RPM = PPM[0];
            ress.FinalPrice = PL.CalcAfterDiscount(ress.FinalPrice, RPM.Discount, RPM.Flat);
        }

        return ress;
    }

    public TotalPriceModel GetTotalRess(ReservationModel Ress)
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        TheatreLogic TL2 = new TheatreLogic();
        MoviesLogic ML = new MoviesLogic();
        SnacksLogic SL = new SnacksLogic();

        MovieModel movie = null!;
        double[][] seats = new double[Ress.Seats.Count()][];
        Dictionary<SnackModel, int> snacks = null!;
        double finalPrice = 0.0;

        // Movie verify
        movie = GetMovieFromRess(Ress);
        finalPrice += movie.Price;

        // Seat verify
        int theatreId = TL.GetById(Ress.TimeSlotId)!.Result.Theatre.TheatreId;

        for (int i = 0; i < Ress.Seats.Count(); i++)
        {
            double seatPrice = TL2.PriceOfSeatType(Ress.Seats[i].Type, theatreId);
            finalPrice += seatPrice;
            seats[i] = new double[] { Ress.Seats[i].Id, seatPrice };
        }

        // Snack verify
        if (Ress.Snacks != null)
        {
            try
            {
                snacks = Ress.Snacks.ToDictionary(x => SL.GetById(x.Key)!.Result, x => x.Value)!;
            }
            catch (System.Exception)
            {
                snacks = new Dictionary<SnackModel, int>();
            }

            foreach (var snack in snacks)
            {
                finalPrice += snack.Key.Price * snack.Value;
            }
        }

        var total = new TotalPriceModel(movie, theatreId, seats, snacks);
        total.FinalPrice = finalPrice;
        return total;
    }

    public MovieModel GetMovieFromRess(ReservationModel ress)
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();

        return ML.GetById(TL.GetById(ress.TimeSlotId)!.Result.MovieId)!.Result;
    }

    public static void MenuReservation()
    {
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

    public static void PreviousReservations(int user_id)
    {
        Console.Clear();
        TimeSlotsLogic TL = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        // iterate through all data in
        List<ReservationModel> reservations = ReservationAccess.LoadAll();
        foreach (ReservationModel reservation in reservations)
        {
            if (TL.GetById(reservation.TimeSlotId).Start  < DateTime.Now)
            {
                try{
                    var resMovieId = TL.GetById(reservation.TimeSlotId).MovieId;

                    var resMovie = ML.GetById(resMovieId);
      
                    Console.WriteLine($"{resMovie.Title}, at {TL.GetById(reservation.TimeSlotId).Start}.\nOrdered on: {reservation.DateTime}\n");  
                }
                catch (System.Exception){
                    continue;
                }
            }
        }
        Console.WriteLine("\nPress any key to return");
        Console.ReadKey();
    }

    public static void CurrentReservations(int user_id)
    {
        Console.Clear();
        TimeSlotsLogic TL = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        // iterate through all data in
        List<ReservationModel> reservations = ReservationAccess.LoadAll();
        foreach (ReservationModel reservation in reservations)
        {
            if (TL.GetById(reservation.TimeSlotId).Start >= DateTime.Now)
            {
                try{
                    var resMovieId = TL.GetById(reservation.TimeSlotId).MovieId;

                    var resMovie = ML.GetById(resMovieId);
    
                    Console.WriteLine($"{resMovie.Title}, at {TL.GetById(reservation.TimeSlotId).Start}.\nOrdered on: {reservation.DateTime}\n");  
                }
                catch (System.Exception){
                    continue;
                }
            }
        }
        Console.WriteLine("\nPress any key to return");
        Console.ReadKey();      
    }
}