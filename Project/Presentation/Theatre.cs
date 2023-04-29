public static class Theatre
{
    private static TheatreLogic TL = new TheatreLogic();
    public static void SelectSeats(TimeSlotModel TimeSlot, bool IsEdited = false)
    {
        TimeSlotsLogic TS = new TimeSlotsLogic();
        var theatre = TimeSlot.Theatre;
        var size = 9;
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin)
        {
            size = 10000;
        }
        var help = TL.ShowSeats(theatre, size);
        ReservationLogic RL = new ReservationLogic();

        if (help != null)
        {
            var selectedSeats = help.Seats;

            string Question = "Would you like to order snacks?";
            List<string> Options = new List<string>() { "Yes", "No" };
            List<Action> Actions = new List<Action>();
            ReservationLogic RL = new ReservationLogic();

            if (FormatsLogic.GetByFormat(TimeSlot.Format) != null)
            {
                Actions.Add(() => Snacks.Start(TimeSlot, selectedSeats, IsEdited));
                Actions.Add(() => Format.Start(TimeSlot, selectedSeats));
            }
            else
            {
                Actions.Add(() => Snacks.Start(TimeSlot, selectedSeats, IsEdited));
                Actions.Add(() => RL.MakeReservation(TimeSlot, selectedSeats, IsEdited: IsEdited));
            }

            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Menu.Start();
        }
    }

    public static void WhatTheather()
    {
        string Question = "What theatre room would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (var item in TL.AllTheatres())
        {
            Options.Add($"Theatre room {item.Id}");
            Actions.Add(() => EditMenu(item));
        }

        Options.Add("\nReturn");
        Actions.Add(() => Admin.Start());

        MenuLogic.Question(Question, Options, Actions);

    }
    public static void EditMenu(TheatreModel theatre, Action returnTo = null!)
    {
        TheatreLogic TL = new TheatreLogic();

        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Change size");
        Actions.Add(() => TL.ChangeTheatreSize(theatre, () => Theatre.EditMenu(theatre, returnTo)));

        Options.Add("Block seats");
        Actions.Add(() => TL.BlockSeats(theatre, () => Theatre.EditMenu(theatre, returnTo)));

        Options.Add("Unblock seats");
        Actions.Add(() => TL.UnBlockSeats(theatre, () => Theatre.EditMenu(theatre, returnTo)));

        Options.Add("\nReturn");
        if (returnTo != null)
        {
            Actions.Add(returnTo.Invoke);
        }

        MenuLogic.Question(Question, Options, Actions);
    }
}