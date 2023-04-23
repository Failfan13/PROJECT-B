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

    public void MakeReservation(TimeSlotModel timeSlot, List<SeatModel> Seats, Dictionary<int, int> snacks = null!, string format = "", bool IsEdited = false)
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
            ress = new ReservationModel(Reservation.CurrReservation.Id, timeSlot.Id, Seats, snacks, AccountId, currDate, format);

        }
        else
        {
            ress = new ReservationModel(GetNewestId(), timeSlot.Id, Seats, snacks, AccountId, currDate, format);
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
}