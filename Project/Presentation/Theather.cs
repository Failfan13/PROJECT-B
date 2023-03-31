public static class Theater
{
    private static TheatherLogic TL = new TheatherLogic();
    public static void SelectSeats(TimeSlotModel timeSLot, bool IsEdited = false)
    {

        var theater = timeSLot.Theater;
        var help = TL.ShowSeats(theater, 9);
        var selectedSeats = help.Seats;

        new TimeSlotsLogic().UpdateList(timeSLot);
        string Question = "Would you like to order snacks?";
        List<string> Options = new List<string>() { "Yes", "No" };
        List<Action> Actions = new List<Action>();
        ReservationLogic RL = new ReservationLogic();


        Actions.Add(() => Snacks.Start(timeSLot.Id, selectedSeats, IsEdited));
        Actions.Add(() => RL.MakeReservation(timeSLot.Id, selectedSeats, IsEdited: IsEdited));

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void EditMenu(TheaterModel theater)
    {
        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Change size");
        Actions.Add(() => TL.ChangeTheaterSize(theater));

        Options.Add("BlockSeats");
        Actions.Add(() => TL.BlockSeats(theater));

        Options.Add("Return");
        Actions.Add(() => Menu.Start());
        MenuLogic.Question(Question, Options, Actions);
    }
}