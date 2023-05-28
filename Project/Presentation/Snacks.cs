static class Snacks
{
    static private SnacksLogic SnacksLogic = new();

    public static bool Continue = true;

    public static void ShowAll()
    {
        foreach (SnackModel snack in SnacksLogic.GetAllSnacks().Result)
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
        int MaxLength = SnacksLogic.GetAllSnacks().Result.Max(snack => snack.Name.Length);

        if (SnacksLogic._addRemove)
        {
            Options.Add("Swap Mode, Currently: Adding to list");
        }
        else
        {
            Options.Add("Swap Mode, Currently: Removing from list");
        }
        Actions.Add(() => SnacksLogic.SwapMode());

        foreach (SnackModel snack in SnacksLogic.GetAllSnacks().Result)
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
                CurrentPrice += Snack!.Result.Price;
            }
        }

        MenuLogic.Question(Question, Options, Actions, $"Total snack price: {CurrentPrice}");

        if (Continue)
        {
            Start(timeSlot, seats, IsEdited);
        }
    }

    public async static void NewSnackMenu()
    {
        Console.Clear();

        string name = "";
        string price = "";
        double newPrice = 0;

        Console.WriteLine("Welcome to the snack creator menu\n\nPlease follow the instructions below");

        Console.WriteLine("Enter the name of the new snack");
        name = Console.ReadLine()!;
        Console.WriteLine("\nEnter the price of the new snack");
        price = Console.ReadLine()!;

        // try parse price otherwise default
        if (double.TryParse(price, out double resultPrice))
        {
            newPrice = resultPrice;
        }

        // new snack model
        SnackModel newSnack = new SnackModel()
        {
            Name = name,
            Price = newPrice
        };

        // upload new snack
        await SnacksLogic.NewSnack(newSnack);
    }

    public static void ChangeSnackMenu()
    {
        Console.Clear();

        string Question = "What would you like to do?";
        List<string> Options = new List<string>() { "Change snack", "Remove snack" };
        List<Action> Actions = new List<Action>();

        Actions.Add(() => ChangeSnack());

        Actions.Add(() => RemoveSnack());

        Options.Add("Return");
        Actions.Add(() => Admin.ChangeData());

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void ChangeSnack()
    {
        // part 1 What snack
        List<SnackModel> snacks = SnacksLogic.GetAllSnacks().Result;

        Console.Clear();

        string Question = "What snack do you want to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (SnackModel snack in snacks)
        {
            Options.Add(snack.Name);
        }

        Options.Add("Return");
        Actions.Add(() => ChangeSnackMenu());

        int snackIndex = MenuLogic.Question(Question, Options, Actions);

        // part 2 What to change

        Question = "What would you like to change it to?";
        Options.Clear();
        Actions.Clear();

        Options.Add("Change name");
        Actions.Add(async () => await ChangeName(snacks[snackIndex]));

        Options.Add("Change price");
        Actions.Add(async () => await ChangePrice(snacks[snackIndex]));

        Options.Add("Return");
        Actions.Add(() => ChangeSnack());

        MenuLogic.Question(Question, Options, Actions);
    }

    private async static Task ChangeName(SnackModel snack)
    {
        Console.Clear();
        Console.WriteLine($"The old name: {snack.Name}\n\nEnter the new name: ");
        snack.Name = Console.ReadLine()!;

        await SnacksLogic.UpdateList(snack);
    }

    private async static Task ChangePrice(SnackModel snack)
    {
        Console.Clear();
        Console.WriteLine($"The old price: {snack.Price}\n\nEnter the new price: ");

        if (double.TryParse(Console.ReadLine()!, out double resultPrice))
        {
            snack.Price = resultPrice;
        }

        await SnacksLogic.UpdateList(snack);
    }

    private static void RemoveSnack()
    {
        List<SnackModel> snacks = SnacksLogic.GetAllSnacks().Result;

        Console.Clear();

        string Question = "What snack do you want to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (SnackModel snack in snacks)
        {
            Options.Add(snack.Name);
            Actions.Add(async () => await SnacksLogic.DeleteSnack(snack.Id));
        }

        Options.Add("Return");
        Actions.Add(() => ChangeSnackMenu());

        MenuLogic.Question(Question, Options, Actions);
    }
}
