public class ReservationLogic
{
    private List<ReservationModel> _reservations;

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

    public void MakeReservation(int movieId, int seatId)
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
        ReservationModel ress = new ReservationModel(GetNewestId(), movieId, seatId, AccountId);
        UpdateList(ress);
    }
}