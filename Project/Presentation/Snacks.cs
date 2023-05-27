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
    public static void Start(TimeSlotModel timeSlot, List<SeatModel> seats, bool IsEdited = false)
    {

        Continue = true;

        // current ress snacks null make new list otherwise use existing
        if (IsEdited)
        {
            if (Reservation.CurrReservation.Snacks == null)
            {
                Reservation.CurrReservation.Snacks = new Dictionary<int, int>();
            }

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
                Options.Add($"{snack.Name}\t{new string('\t', Tabs)}\t{snack.Price}\t{SnacksLogic.CurrentResSnacks[snack.Id]}");
            }
            else
            {
                Options.Add($"{snack.Name}\t{new string('\t', Tabs)}\t{snack.Price}\t0");
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

        Options.Add($"Add to reservation");
        if (FormatsLogic.GetByFormat(timeSlot.Format) != null)
        {
            Actions.Add(() => Format.Start(timeSlot, seats, SnacksLogic.GetSelectedSnacks(), IsEdited));
        }
        else
        {
            Actions.Add(async () => await new ReservationLogic().MakeReservation(timeSlot, seats, SnacksLogic.GetSelectedSnacks(), "", IsEdited).ConfigureAwait(false));
        }


        double CurrentPrice = 0;
        foreach (KeyValuePair<int, int> KeyValue in SnacksLogic.CurrentResSnacks)
        {
            var Snack = SnacksLogic.GetById(KeyValue.Key);
            for (int i = 0; i < KeyValue.Value; i++)
            {
                CurrentPrice += Snack.Price;
            }
        }

        MenuLogic.Question(Question, Options, Actions, $"Total snack price: {CurrentPrice}");

        if (Continue)
        {
            Start(timeSlot, seats, IsEdited);
        }
    }
}
