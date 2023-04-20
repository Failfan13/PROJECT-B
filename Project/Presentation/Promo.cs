public static class Promo
{
    public static PromoLogic PromoLogic = new PromoLogic();

    public static int? Start()
    {
        string wantPromo;
        int codeId;

        Console.WriteLine("Do you have a Promo code?(Y/N)");
        wantPromo = Console.ReadKey().Key.ToString();

        if (wantPromo != "Y") return null;

        codeId = EnterPromoCode();
        Console.WriteLine(codeId);
        if (codeId != 0)
        {
            return codeId;
        }

        return null;
    }

    // if promo code is valid, return promo id or 0 if not
    private static int EnterPromoCode()
    {
        string code;
        Console.WriteLine("Enter your promo code");
        code = Console.ReadLine()!.ToUpper();

        if (PromoLogic.FindPromo(code))
        {
            return PromoLogic.GetPromoId(code);
        }
        return 0;
    }

    public static void EditPromoMenu()
    {
        string Question = "what would you like to do?";
        List<string> Options = new List<string>() { "Add Promo", "Remove Promo", "Turn Promo on/off", "Edit Promo" };
        List<Action> Actions = new List<Action>();

        Actions.Add(() => AddPromo());
        Actions.Add(() => RemovePromo());
        Actions.Add(() => TurnPromo());
        Actions.Add(() => EditPromo());

        Options.Add("Return");
        Actions.Add(() => EditPromoMenu()/* where to return to*/);

        MenuLogic.Question(Question, Options, Actions);

        // where to return default
    }

    private static void AddPromo(PromoModel promo = null)
    {
        string code;
        bool corrCode = false;

        if (promo == null)
        {
            Console.WriteLine("Enter the new Promo code");
            code = Console.ReadLine()!.ToUpper();

            corrCode = PromoLogic.VerifyCode(code);
            if (!corrCode) AddPromo(); // if code is not valid, ask again

            promo = PromoLogic.NewPromo(code);
        }

        string Question = "What are the conditions you want to apply";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Driks & snacks prices");
        Actions.Add(() => ChangeSnack(promo));
        Options.Add("Seat prices");
        Actions.Add(() => ChangeSeat(promo));
        Options.Add("Total order price");
        Actions.Add(() => ChangeTotal(promo));
        Options.Add("Return");
        Actions.Add(() => EditPromoMenu());

        MenuLogic.Question(Question, Options, Actions);

        EditPromoMenu();
    }
    private static void RemovePromo()
    {
        Console.WriteLine("Enter the Promo code to remove");
        string code = Console.ReadLine()!.ToUpper();

        if (PromoLogic.FindPromo(code))
        {
            PromoLogic.RemovePromo(code);
        }

        EditPromoMenu();
    }
    private static void TurnPromo()
    {
        Console.WriteLine("Enter the Promo code to turn");
        string code = Console.ReadLine()!.ToUpper();

        if (PromoLogic.FindPromo(code))
        {
            PromoLogic.TurnPromo(code);
        }

        EditPromoMenu();
    }
    private static void EditPromo()
    {
        Console.WriteLine("Enter the Promo code to edit");
        string code = Console.ReadLine()!.ToUpper();

        if (PromoLogic.FindPromo(code))
        {
            string Question = "What would you like to change?";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();
            PromoModel? promo = PromoLogic.GetById(PromoLogic.GetPromoId(code));

            Options.Add("Promo code");
            Actions.Add(() => ChangeCode(promo));
            Options.Add("Driks & snacks prices");
            Actions.Add(() => ChangeSnack(promo));
            Options.Add("Seat prices");
            Actions.Add(() => ChangeSeat(promo));
            Options.Add("Total order price");
            Actions.Add(() => ChangeTotal(promo));
            Options.Add("Return");
            Actions.Add(() => EditPromoMenu());

            MenuLogic.Question(Question, Options, Actions);
        }

        EditPromoMenu();
    }

    private static void ChangeCode(PromoModel promo)
    {
        string code;
        bool CorrectCode = false;

        code = Console.ReadLine()!.ToUpper();
        CorrectCode = PromoLogic.VerifyCode(code);

        if (CorrectCode)
        {
            promo.Code = code;
        }

        PromoLogic.UpdateList(promo);

        EditPromo();
    }
    private static void ChangeSnack(PromoModel promo)
    {
        SnacksLogic SnacksLogic = new SnacksLogic();
        List<SnackPromoModel> snackPromos = new List<SnackPromoModel>();
        int MaxLength = SnacksLogic.AllSnacks().Max(snack => snack.Name.Length);

        string Question = "Choose the snack that fits the promotion";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (SnackModel snack in SnacksLogic.AllSnacks()) // copied in Promo DRY
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
            //Actions.Add(() => )// needs to create SnackPromoModel, And Ask for discount & flat);
        }

        Options.Add("Return");
        Actions.Add(() => EditPromoMenu());

        MenuLogic.Question(Question, Options, Actions);

        if (promo.Condition == null)
        {
            promo.Condition = new List<object>();
        }
        //snackPromos.Add( HERE ADD NEW SNACKPROMOMODEl);
        promo.Condition.Add(snackPromos);
        PromoLogic.UpdateList(promo);

        AddPromo(promo);
    }
    private static void ChangeSeat(PromoModel promo) { Console.WriteLine("sus2"); }
    private static void ChangeTotal(PromoModel promo) { Console.WriteLine("sus3"); }
    private static void ChangeDiscount(double discount, bool flat)
    {
        Console.WriteLine($"Enter the new discount price");
        discount = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Is the discount in percentaile or euros? (P/E)");
        switch (Console.ReadKey().Key.ToString())
        {
            case "P":
                flat = false;
                break;
            case "E":
                flat = true;
                break;
            default:
                break;
        }

    }
}