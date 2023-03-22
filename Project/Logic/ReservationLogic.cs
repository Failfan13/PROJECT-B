public class ReservationLogic
{
    private List<ReservationModel> _reservations;

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
        _reservations = ReservationAccess.LoadAll();
    }


    public void UpdateList(ReservationModel ress)
    {
        //Find if there is already an model with the same id
        int index = _reservations.FindIndex(s => s.Id == ress.Id);

        if (index != -1)
        {
            //update existing model
            _reservations[index] = ress;
        }
        else
        {
            //add new model
            _reservations.Add(ress);
        }
        ReservationAccess.WriteAll(_reservations);

    }

    public ReservationModel? GetById(int id)
    {
        return _reservations.Find(i => i.Id == id);
    }
    public int GetNewestId()
    {
        try
        {
            return (_reservations.OrderByDescending(item => item.Id).First().Id) + 1;
        }
        catch (System.Exception)
        {
            return 1;
        }

    }

    public void MakeReservation(int movieId, List<int> seatIds)
    {
        int? AccountId = null;
        try
        {
            AccountId = AccountsLogic.CurrentAccount.Id;
        }
        catch (System.Exception)
        {   // not logged in
            AccountId = null;
        }
        ReservationModel ress = new ReservationModel(GetNewestId(), movieId, seatIds, AccountId);
        UpdateList(ress);
    }
}