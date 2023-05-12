using System.Text.Json;
public static class Promo
{
    public static PromoLogic PromoLogic = new PromoLogic();

    public static int Start()
    {
        Console.Clear();
        int codeId;

        Console.WriteLine("Do you have a Promo code?(Y/N)");
        string wantPromo = Console.ReadKey().Key.ToString();

        if (wantPromo != "Y") return -1;

        codeId = EnterPromoCode();

        if (codeId != 0)
        {
            return codeId;
        }

        return -1;
    }
    // if promo code is valid, return promo id or 0 if not
    private static int EnterPromoCode()
    {
        Console.Clear();
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
        Console.Clear();
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
        Console.Clear();
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

        if (promo.Condition == null)
            promo.Condition = new Dictionary<string, IEnumerable<object>>();

        string Question = "What are the conditions you want to apply";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Promo applied movie");
        Actions.Add(() => ChangeMovie(promo, true));
        Options.Add("Movie price");
        Actions.Add(() => ChangeMovie(promo));
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
        Console.Clear();
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
        Console.Clear();
        Console.WriteLine("Enter the Promo code to turn");
        string code = Console.ReadLine()!.ToUpper();

        if (PromoLogic.FindPromo(code))
        {
            PromoLogic.TurnPromo(code);
        }

        EditPromoMenu();
    }
    private static void EditPromo(PromoModel promo = null)
    {
        Console.Clear();
        string code = "";
        if (promo == null)
        {
            Console.WriteLine("Enter the Promo code to edit");
            code = Console.ReadLine()!.ToUpper();

            promo = PromoLogic.GetById(PromoLogic.GetPromoId(code) + 1);
        }
        else
        {
            code = promo.Code;
        }

        if (PromoLogic.FindPromo(code))
        {
            Console.Clear();
            string Question = "What would you like to change?";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            if (promo == null) return;

            if (promo.Condition == null)
                promo.Condition = new Dictionary<string, IEnumerable<object>>();

            Options.Add("Promo code");
            Actions.Add(() => ChangeCode(promo, true));
            Options.Add("Promo applied movie");
            Actions.Add(() => ChangeMovie(promo, true, true));
            Options.Add("Movie price");
            Actions.Add(() => ChangeMovie(promo, true));
            Options.Add("Driks & snacks price");
            Actions.Add(() => ChangeSnack(promo, true));
            Options.Add("Seat price");
            Actions.Add(() => ChangeSeat(promo, true));
            Options.Add("Total order price");
            Actions.Add(() => ChangeTotal(promo, true));
            Options.Add("Remove all previous conditions");
            Actions.Add(() => Parallel.Invoke(
                () => PromoLogic.GetById(promo.Id).Condition = null,
                () => PromoLogic.UpdateList(promo)));
            Options.Add("Return");
            Actions.Add(() => EditPromoMenu());

            MenuLogic.Question(Question, Options, Actions);
        }

        EditPromoMenu();
    }

    private static void ChangeCode(PromoModel promo, bool isEdited = false)
    {
        Console.Clear();
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

        if (isEdited) EditPromo(promo);
        AddPromo(promo);
    }
    private static void ChangeMovie(PromoModel promo, bool specific = false, bool isEdited = false)
    {
        Console.Clear();
        MoviesLogic MoviesLogic = new MoviesLogic();
        // load all pre existing in movieDict
        List<MoviePromoModel> moviePromos = PromoLogic.AllMovies(promo);
        // set default moviePromo (all movies)
        MoviePromoModel moviePromo = new MoviePromoModel(0, "all", ChangeDiscount(), ChangeFlat());
        MovieModel movie = null;

        // specific movie for promo code
        if (specific)
        {
            moviePromo.Specific = true;

            Console.WriteLine("Enter the name of the movie");

            movie = MoviesLogic.FindTitle(Console.ReadLine()!);
            if (movie == null) return;

            moviePromo = new MoviePromoModel(movie.Id, movie.Title, ChangeDiscount(movie.Price), ChangeFlat());

            if (moviePromos.Any(m => m.MovieId == movie.Id))
            {
                moviePromos.RemoveAll(m => m.MovieId == movie.Id);
            }
        }

        moviePromos.Add(moviePromo);
        // if Condition has MovieDict
        try
        {
            promo.Condition.Add("movieDict", moviePromos);
        }
        catch
        {
            promo.Condition["movieDict"] = moviePromos;
        }

        PromoLogic.UpdateList(promo);

        if (isEdited) EditPromo(promo);
        AddPromo(promo);
    }
    private static void ChangeSnack(PromoModel promo, bool isEdited = false)
    {
        Console.Clear();
        SnacksLogic SnacksLogic = new SnacksLogic();
        List<SnackPromoModel> snackPromos = PromoLogic.AllSnacks(promo);// load all pre existing in snackDict
        SnackPromoModel? SnackModel = null;

        string Question = "Choose the snack that fits the promotion";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        int MaxLength = SnacksLogic.AllSnacks().Max(snack => snack.Name.Length);

        foreach (SnackModel snack in SnacksLogic.AllSnacks()) // copy in Snack.cs DRY
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

        if (SnackModel != null)
        {
            // if condition null or condition already contains same
            if (snackPromos.Any(s => s.SnackId == SnackModel.SnackId))
            {
                snackPromos.RemoveAll(s => s.SnackId == SnackModel.SnackId);
            }

            snackPromos.Add(SnackModel);

            // if Condition has snackDict
            try
            {
                promo.Condition.Add("snackDict", snackPromos);
            }
            catch
            {
                promo.Condition["snackDict"] = snackPromos;
            }
            PromoLogic.UpdateList(promo);
        }

        if (isEdited) EditPromo(promo);
        AddPromo(promo);
    }
    private static void ChangeSeat(PromoModel promo, bool isEdited = false)
    {
        Console.Clear();

        List<SeatPromoModel> SeatPromos = PromoLogic.AllSeats(promo);// load all pre existing in SeatDict

        string seatType;
        string amountSeats;
        double discountSeat;
        bool flatSeat;


        Console.WriteLine("Will the code work for all seats or luxury (N/L)");
        seatType = (Console.ReadKey().KeyChar.ToString().ToUpper()) switch
        {
            "N" => "normal",
            "L" => "luxury",
            _ => "normal"
        };

        Console.WriteLine("For how many seats does the promotion apply enter: 'all' or specific amount");
        amountSeats = Console.ReadLine()!.ToLower();
        if (amountSeats != "all" && !Int32.TryParse(amountSeats, out _))
        {
            Console.WriteLine("The input is not 'all' or a number, promotion set for all seats");
            amountSeats = "all";
        }

        discountSeat = ChangeDiscount();
        flatSeat = ChangeFlat();

        SeatPromoModel SeatModel = new SeatPromoModel(seatType, amountSeats, discountSeat, flatSeat);

        // if condition null or condition already contains same
        if (SeatPromos.Any(s => s.SeatType == SeatModel.SeatType))
        {
            SeatPromos.RemoveAll(s => s.SeatType == SeatModel.SeatType);
        }

        SeatPromos.Add(SeatModel);

        // if Condition has SeatDict
        try
        {
            promo.Condition.Add("seatDict", SeatPromos);
        }
        catch
        {
            promo.Condition["seatDict"] = SeatPromos;
        }
        PromoLogic.UpdateList(promo);

        if (isEdited) EditPromo(promo);
        AddPromo(promo);
    }
    private static void ChangeTotal(PromoModel promo, bool isEdited = false)
    {
        double discount = ChangeDiscount();
        bool flat = ChangeFlat();

        List<PricePromoModel> pricePromos = new List<PricePromoModel>() { new PricePromoModel(discount, flat) };

        // if Condition has priceDict
        try
        {
            promo.Condition.Add("priceDict", pricePromos);
        }
        catch
        {
            promo.Condition["priceDict"] = pricePromos;
        }
        PromoLogic.UpdateList(promo);

        if (isEdited) EditPromo(promo);
        AddPromo(promo);
    }
    private static double ChangeDiscount(double oldPrice = int.MaxValue)
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