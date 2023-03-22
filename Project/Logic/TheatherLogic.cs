public class TheatherLogic
{
    private List<TheaterModel> _theathers;

    public TheatherLogic()
    {
        _theathers = TheaterAccess.LoadAll();
    }

    public void UpdateList(TheaterModel theather)
    {
        //Find if there is already an model with the same id
        int index = _theathers.FindIndex(s => s.Id == theather.Id);

        if (index != -1)
        {
            //update existing model
            _theathers[index] = theather;
        }
        else
        {
            //add new model
            _theathers.Add(theather);
        }
        TheaterAccess.WriteAll(_theathers);

    }

    public TheaterModel? GetById(int id)
    {
        return _theathers.Find(i => i.Id == id);
    }
    public int GetNewestId()
    {
        return (_theathers.OrderByDescending(item => item.Id).First().Id) + 1;
    }
    public void MakeTheather(int rows, int seatAmount)
    {
        List<SeatModel> seats = new List<SeatModel>();

        for (int i = 0; i < rows; i++)
        {
            for (int x = 0; x < seatAmount; x++)
            {
                seats.Add(new SeatModel(x, 10, i, false, false));
            }
        }
        TheaterModel theater = new TheaterModel(GetNewestId(), seats, GetNewestId());

        UpdateList(theater);
    }
}