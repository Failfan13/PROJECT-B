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
        Actions.Add(() => returner = AccountsLogic.CurrentAccount.Id);
        Actions.Add(() => returner = AL.GetAccountIdFromList());

        MenuLogic.Question(Question, Options, Actions);

        return returner;
    }

    public void MakeReservation(TimeSlotModel timeSlot, List<SeatModel> Seats, Dictionary<int, int> snacks = null, bool IsEdited = false)
    {

        Snacks.Continue = false;
        int? AccountId = null;
        ReservationModel ress = null;
        DateTime currDate = DateTime.Now;
        try
        {
            AccountId = AccountsLogic.CurrentAccount.Id;
            if (AccountsLogic.CurrentAccount.Admin)
            {
                // asks if you want to use your own ID or that of an user
                AccountId = AsAdminId();
            }
        }
        catch (System.Exception)
        {   // not logged in
            AccountId = null;
        }

        // if this reservation is made by an edit, use the id of the current reservation
        if (IsEdited)
        {
            ress = new ReservationModel(Reservation.CurrReservation.Id, timeSlot.Id, Seats, snacks, AccountId, currDate);

        }
        else
        {
            ress = new ReservationModel(GetNewestId(), timeSlot.Id, Seats, snacks, AccountId, currDate);
        }

        // Make the new Reservation and update the Theather timeslot for the seats
        Reservation.TotalReservationCost(ress);
        UpdateList(ress);
        TimeSlotsLogic TL = new TimeSlotsLogic();
        TL.UpdateList(timeSlot);
    }

    public void ChangeUserId(ReservationModel Ress)
    {
        AccountsLogic AL = new AccountsLogic();
        int newUserId = AL.GetAccountIdFromList();

        Ress.AccountId = newUserId;
        UpdateList(Ress);
    }

    public ReservationModel ApplyDiscount(string DiscountCode, TotalPriceModel Ress)
    {
        PromoLogic PL = new PromoLogic();
        PromoModel PM = PL.GetById(PL.GetPromoId(DiscountCode))!;
        TimeSlotsLogic TL = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();

        List<PricePromoModel> PPM = PL.AllPrices(PM);
        List<MoviePromoModel> MPM = PL.AllMovies(PM);
        List<SeatPromoModel> SPM = PL.AllSeats(PM);
        List<SnackPromoModel> SP = PL.AllSnacks(PM);

        // if (PM == null || !PM.Active) return Ress;

        // Ress.DiscountCode = DiscountCode;
        // UpdateList(Ress);

        // // Movie check
        // MoviePromoModel mpmSpecific = MPM.Find(m => m.MovieId == TL.GetById(Ress.TimeSlotId).MovieId && m.Specific);
        // MoviePromoModel mpmAny = MPM.Find(m => m.Title == "all" && !m.Specific);

        // Seat check

        // Snack check

        // Total check
        return null;
    }

    public TotalPriceModel GetTotalRess(ReservationModel Ress)
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        SnacksLogic SL = new SnacksLogic();

        MovieModel movie = null;
        List<SeatModel> seat = null;
        Dictionary<int, SnackModel> snack = null;
        double finalPrice = 0.0;

        movie = ML.GetById(Ress.TimeSlotId);
        seat = Ress.Seats;
        snack = Ress.Snacks.ToDictionary(x => x.Value, x => SL.GetById(x.Key));

        finalPrice += movie.Price;
        foreach (var item in seat)
        {
            finalPrice += item.Price;
        }
        foreach (var item in snack)
        {
            finalPrice += item.Value.Price;
        }

        var total = new TotalPriceModel(movie, seat, snack);
        total.FinalPrice = finalPrice;
        return total;
    }

    public MovieModel GetMovieFromRess(ReservationModel ress)
    {
        TimeSlotsLogic TL = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();

        return ML.GetById(TL.GetById(ress.TimeSlotId).MovieId);
    }
}