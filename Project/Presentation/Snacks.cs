static class Snacks
{
    static private SnacksLogic SnacksLogic = new();

    public static bool Continue = true;

    public static void ShowAll()
    {
        foreach (SnackModel snack in SnacksLogic.AllSnacks())
        {
            Console.WriteLine($"{snack.Id} {snack.Name}");
        }
    }

    // Start request to user
    public static void Start(int TimeSlotId, List<SeatModel> Seats, bool IsEdited = false)
    {
        Continue = true;
        if (IsEdited)
        {
            SnacksLogic.CurrentResSnacks = Reservation.CurrReservation.Snacks;
        }
        string Question = "Make a Snack choice:";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        // Get length of longest Snack string
        int MaxLength = SnacksLogic.AllSnacks().Max(snack => snack.Name.Length);

        if (SnacksLogic._addRemove)
        {
            Options.Add("Swap Mode, Currently: Adding to list");
        }
        else
        {
            Options.Add("Swap Mode, Currently: Removing from list");
        }
        Actions.Add(() => SnacksLogic.SwapMode());

        foreach (SnackModel snack in SnacksLogic.AllSnacks())
        {
            int Tabs = (int)Math.Ceiling((MaxLength - snack.Name.Length) / 8.0);
            if (SnacksLogic.CurrentResSnacks.ContainsKey(snack.Id))
            {
                Options.Add($"{snack.Name}\t{new string('\t', Tabs)}{SnacksLogic.CurrentResSnacks[snack.Id]}");
            }
            else
            {
                Options.Add($"{snack.Name}\t{new string('\t', Tabs)}0");
            }

            // Add or Remove snacks from list
            if (SnacksLogic._addRemove)
            {
                Actions.Add(() => SnacksLogic.AddSnack(snack));
            }
            else
            {
                Actions.Add(() => SnacksLogic.RemoveSnack(snack));
            }
        }
        
        Options.Add("Add to reservation");
        Actions.Add(() => new ReservationLogic().MakeReservation(TimeSlotId, Seats, SnacksLogic.GetSelectedSnacks(), IsEdited));

        MenuLogic.Question(Question, Options, Actions);

        if (Continue)
        {
            Start(TimeSlotId, Seats, IsEdited);
        }

    }
}