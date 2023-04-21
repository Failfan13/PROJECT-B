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
    public static void tester(PromoModel promo)
    {
        // foreach (var type in promo.Condition)
        // {
        //     // switch (type.GetType().Name)
        //     // {
        //     //     case "MoviePromoModel":
        //     //         Console.WriteLine("sussy");
        //     //         break;

        //     // }
        //     Console.WriteLine(type.GetType().Name);
        // }
        // Console.ReadLine();
        // PromoAccess.LoadAllCondition.LoadAll(promo);



        // Deserialize the Condition from the json file to right object (MoviePromoModel, SnackPromoModel, etc)
        Console.ReadLine();
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
            PromoModel? promo = PromoLogic.GetById(PromoLogic.GetPromoId(code) + 1);

            if (promo == null) return;

            Options.Add("Promo code");
            Actions.Add(() => ChangeCode(promo));
            Options.Add("Promo applied movie");
            Actions.Add(() => ChangeMovie(promo, true));
            Options.Add("Movie price");
            Actions.Add(() => ChangeMovie(promo));
            Options.Add("Driks & snacks price");
            Actions.Add(() => ChangeSnack(promo));
            Options.Add("Seat price");
            Actions.Add(() => ChangeSeat(promo));
            Options.Add("Total order price");
            Actions.Add(() => ChangeTotal(promo));
            Options.Add("Remove all previous conditions");
            Actions.Add(() => Parallel.Invoke(
                () => PromoLogic.GetById(promo.Id).Condition = null,
                () => PromoLogic.UpdateList(promo)));
            Options.Add("Log promo");
            Actions.Add(() => tester(promo));
            Options.Add("Return");
            Actions.Add(() => EditPromoMenu());

            MenuLogic.Question(Question, Options, Actions);
        }

        EditPromoMenu();
    }

    private static void ChangeMovie(PromoModel promo, bool specific = false)
    {
        MoviesLogic MoviesLogic = new MoviesLogic();
        List<MoviePromoModel> moviePromos = new List<MoviePromoModel>();

        Console.WriteLine("Enter the name of the movie");
        MovieModel? movie = MoviesLogic.FindTitle(Console.ReadLine()!);

        if (movie != null)
        {
            MoviePromoModel moviePromo = new MoviePromoModel(movie.Id, movie.Title, ChangeDiscount(movie.Price), ChangeFlat());

            // specific movie for promo code
            if (specific) moviePromo.Specific = true;

            if (promo.Condition == null) promo.Condition = new List<object>();

            moviePromos.Add(moviePromo);
            promo.Condition.Add(moviePromos);
            PromoLogic.UpdateList(promo);
        }

        EditPromoMenu();
    }
    private static void ChangeCode(PromoModel promo)
    {
        string code;
        bool CorrectCode = false;

        Console.WriteLine("Enter the new Promo code");

        code = Console.ReadLine()!.ToUpper();
        CorrectCode = PromoLogic.VerifyCode(code);

        if (CorrectCode)
        {
            promo.Code = code;
        }

        PromoLogic.UpdateList(promo);

        EditPromoMenu();
    }
    private static void ChangeSnack(PromoModel promo)
    {
        SnacksLogic SnacksLogic = new SnacksLogic();
        List<SnackPromoModel> snackPromos = new List<SnackPromoModel>();
        SnackPromoModel? SnackModel = null;

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
            Actions.Add(() => SnackModel = new SnackPromoModel(snack.Id, snack.Name, ChangeDiscount(snack.Price), ChangeFlat()));
        }

        Options.Add("Return");
        Actions.Add(() => EditPromoMenu());

        MenuLogic.Question(Question, Options, Actions);

        if (promo.Condition == null) promo.Condition = new List<object>();

        if (SnackModel != null) snackPromos.Add(SnackModel);

        promo.Condition.Add(snackPromos);
        PromoLogic.UpdateList(promo);

        AddPromo(promo);
    }
    private static void ChangeSeat(PromoModel promo)
    {

    }
    private static void ChangeTotal(PromoModel promo) { Console.WriteLine("sus3"); }
    private static double ChangeDiscount(double oldPrice)
    {
        Console.WriteLine($"Enter the new discount price");
        try
        {
            double newPrice = double.Parse(Console.ReadLine()!);

            if (newPrice < oldPrice) return newPrice;
            else return oldPrice;
        }
        catch (Exception)
        {
            return oldPrice;
        }
    }
    private static bool ChangeFlat()
    {
        Console.WriteLine("Is the discount in percentaile or euros? (P/E)");
        return (Console.ReadKey().Key.ToString()) switch
        {
            "P" => true,
            "E" => false,
            _ => false
        };

    }
}