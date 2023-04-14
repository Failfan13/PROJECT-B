public static class Theater
{
    private static TheatherLogic TL = new TheatherLogic();
    public static void SelectSeats(TimeSlotModel timeSLot, bool IsEdited = false)
    {

        var theater = timeSLot.Theater;
        var size = 9;
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin)
        {
            size = 10000;
        }
        var help = TL.ShowSeats(theater, size);
        if (help != null)
        {
            var selectedSeats = help.Seats;

            string Question = "Would you like to order snacks?";
            List<string> Options = new List<string>() { "Yes", "No" };
            List<Action> Actions = new List<Action>();
            ReservationLogic RL = new ReservationLogic();


            Actions.Add(() => Snacks.Start(timeSLot, selectedSeats, IsEdited));
            Actions.Add(() => RL.MakeReservation(timeSLot, selectedSeats, IsEdited: IsEdited));

            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Menu.Start();
        }
    }

    public static void WhatTheather()
    {
        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (var item in TL.AllTheaters())
        {
            Options.Add($"{item.Id}");
            Actions.Add(() => EditMenu(item));
        }

        Options.Add("\nReturn");
        Actions.Add(() => Admin.Start());

        MenuLogic.Question(Question, Options, Actions);

    }
    public static void EditMenu(TheaterModel theater, Action returnTo = null!)
    {
        TheatherLogic TL = new TheatherLogic();

        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Change size");
        Actions.Add(() => TL.ChangeTheaterSize(theater, () => Theater.EditMenu(theater, returnTo)));

        Options.Add("Block seats");
        Actions.Add(() => TL.BlockSeats(theater, () => Theater.EditMenu(theater, returnTo)));

        Options.Add("Unblock seats");
        Actions.Add(() => TL.UnBlockSeats(theater, () => Theater.EditMenu(theater, returnTo)));

        Options.Add("\nReturn");
        if (returnTo != null)
        {
            Actions.Add(returnTo.Invoke);
        }

        MenuLogic.Question(Question, Options, Actions);
    }
}