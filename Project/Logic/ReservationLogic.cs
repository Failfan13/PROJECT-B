public class ReservationLogic
{
    public List<ReservationModel> Reservations;

    //Total reservation cost
    private double _totalOrder = 0.0;
    //Increases totalOrder
    public double TotalOrder
    {
        get => Math.Round(_totalOrder, 2);
        set => _totalOrder += value;
    }
    //Decreases totalOder
    public double TotalOrderDecr
    {
        set => _totalOrder -= value;
    }

    public ReservationLogic()
    {
        Reservations = ReservationAccess.LoadAll();
    }

    public void UpdateList(ReservationModel ress)
    {
        //Find if there is already an model with the same id
        int index = Reservations.FindIndex(s => s.Id == ress.Id);

        if (index != -1)
        {
            //update existing model
            Reservations[index] = ress;
            Logger.LogDataChange<ReservationModel>(ress.Id, "Updated");
        }
        else
        {
            //add new model
            Reservations.Add(ress);
            Logger.LogDataChange<ReservationModel>(ress.Id, "Added");
        }
        ReservationAccess.WriteAll(Reservations);

    }

    public ReservationModel? GetById(int id)
    {
        return Reservations.Find(i => i.Id == id);
    }
    public int GetNewestId()
    {
        try
        {
            return (Reservations.OrderByDescending(item => item.Id).First().Id) + 1;
        }
        catch (System.Exception)
        {
            return 1;
        }

    }
    private int AsAdminId()
    {
        AccountsLogic AL = new AccountsLogic();
        int returner = -1;
        string Question = "Do you want to use your Admin Id or a User Id?";
        List<string> Options = new List<string>() { "Admin", "User" };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => returner = AccountsLogic.CurrentAccount!.Id);
        Actions.Add(() => returner = AL.GetAccountIdFromList().Result);

        MenuLogic.Question(Question, Options, Actions);

        return returner;
    }

    public bool UpdateReservation(int ressId, ReservationModel newRess)
    {
        try
        {
            ReservationModel currRess = GetById(ressId)!;
            currRess.TimeSlotId = newRess.TimeSlotId;
            currRess.Seats = newRess.Seats;
            if (newRess.Snacks != null) currRess.Snacks = newRess.Snacks;
            if (newRess.Format != "") currRess.Format = newRess.Format;
            UpdateList(currRess);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public void MakeReservation(TimeSlotModel timeSlot, List<SeatModel> Seats, Dictionary<int, int> snacks = null!, string format = "", bool IsEdited = false)
    {
        Snacks.Continue = false;
        int AccountId = -1;
        ReservationModel ress = null!;
        DateTime currDate = DateTime.Now;
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

        // if this reservation is made by an edit, use the id of the current reservation
        if (IsEdited)
        {
            // Make the new Reservation and update the Theather timeslot for the seats
            ress = new ReservationModel(Reservation.CurrReservation.Id, timeSlot.Id, Seats, snacks, AccountId, currDate, format);
            UpdateReservation(Reservation.CurrReservation.Id, ress);

            Console.Clear();
            Console.WriteLine("Reservation edited");
            QuestionLogic.AskEnter();
            Menu.Start();
            //Reservation.TotalReservationCost(ress, AccountId, IsEdited);
        }
        else
        {
            ress = new ReservationModel(GetNewestId(), timeSlot.Id, Seats, snacks, AccountId, currDate, format);
            Reservation.TotalReservationCost(ress, AccountId);
            UpdateList(ress);
        }

        TimeSlotsLogic TL = new TimeSlotsLogic();
        TL.UpdateList(timeSlot);
    }

    public async void ChangeUserId(ReservationModel Ress)
    {
        AccountsLogic AL = new AccountsLogic();
        int newUserId = await AL.GetAccountIdFromList();

        Ress.AccountId = newUserId;
        UpdateList(Ress);
    }

    public TotalPriceModel ApplyDiscount(string DiscountCode, TotalPriceModel Ress)
    {

        PromoLogic PL = new PromoLogic();
        PromoModel PM = PL.GetById(PL.GetPromoId(DiscountCode))!;
        TimeSlotsLogic TL = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();

        List<PricePromoModel> PPM = PL.AllPrices(PM);
        PricePromoModel RPM = null!;
        List<MoviePromoModel> MPM = PL.AllMovies(PM);
        List<SeatPromoModel> SPM = PL.AllSeats(PM);
        SeatPromoModel RSPM = null!;
        List<SnackPromoModel> SP = PL.AllSnacks(PM);

        if (PM == null || !PM.Active) return Ress;
        if (SPM.Count != 0) RSPM = SPM[0];

        // Movie check
        if (MPM.Count() != 0)
        {
            MoviePromoModel movie = MPM.Find(m => m.MovieId == Ress.Movie.Id && m.Specific)!;

            //if (movie == null) movie = MPM.Find(m => m.Title == "all")!;

            if (movie != null)
            {
                double newPrice = PL.CalcAfterDiscount(Ress.Movie.Price, movie.Discount, movie.Flat);
                Ress.FinalPrice += newPrice - Ress.Movie.Price;
                Ress.Movie.Price = newPrice;
            }

            //if (movie == null) return Ress;
        }

        // Seat check
        if (SPM.Count() != 0 && Ress.Seats.Count() != 0)
        {
            TheatreLogic TL2 = new TheatreLogic();

            RSPM = SPM[0];
            bool allSeats = true;
            int loopFor = 0;

            if (RSPM.SeatAmount != "all") allSeats = false;

            if (allSeats) loopFor = Ress.Seats.Count();
            else loopFor = int.Parse(RSPM.SeatAmount);

            try // calculate price & set new price
            {
                for (int i = 0; i < loopFor; i++)
                {
                    double newPrice = PL.CalcAfterDiscount(Ress.Seats[i][1], RSPM.Discount, RSPM.Flat);
                    Ress.FinalPrice += newPrice - Ress.Seats[i][1];
                    Ress.Seats[i][1] = newPrice;
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
                    var snackie = Ress.Snacks.FirstOrDefault(s => s.Key.Id == snack.SnackId);
                    double newPrice = PL.CalcAfterDiscount(snackie.Key.Price, snack.Discount, snack.Flat);
                    Ress.FinalPrice += (newPrice - snackie.Key.Price) * snackie.Value;
                    snackie.Key.Price = newPrice;
                }
            }
            catch (System.Exception) { }
        }

        // Total check
        if (PPM.Count != 0)
        {
            RPM = PPM[0];
            Ress.FinalPrice = PL.CalcAfterDiscount(Ress.FinalPrice, RPM.Discount, RPM.Flat);
        }

        return Ress;
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
        movie = ML.GetById(TL.GetById(Ress.TimeSlotId)!.MovieId)!;
        finalPrice += movie.Price;

        // Seat verify
        int theatreId = TL.GetById(Ress.TimeSlotId)!.Theatre.TheatreId;

        for (int i = 0; i < Ress.Seats.Count(); i++)
        {
            double seatPrice = TL2.PriceOfSeatType(Ress.Seats[i].Type, theatreId);
            finalPrice += seatPrice;
            seats[i] = new double[] { Ress.Seats[i].Id, seatPrice };
        }

        // Snack verify
        if (Ress.Snacks != null)
        {
            snacks = Ress.Snacks.ToDictionary(x => SL.GetById(x.Key), x => x.Value)!;
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

        return ML.GetById(TL.GetById(ress.TimeSlotId)!.MovieId)!;
    }
}