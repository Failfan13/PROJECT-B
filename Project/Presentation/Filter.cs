using System.Globalization;

public static class Filter
{
    public static MovieModel? AppliedFilters = null; // Selected movie date stored as AppliedFilters.ReleseDate
    public static void Main(bool IsEdited = false)
    {
        FilterLogic.CheckAppliedFilters();
        string Question = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        Options.Add("Add a filter");
        Actions.Add(() => Filter.AddFilter(IsEdited));

        if (FilterLogic.CheckAppliedFilters())
        {
            Options.Add("Remove a filter");
            Actions.Add(() => Filter.RemoveFilter(IsEdited));
            Options.Add("Apply filters");
            Actions.Add(() => Reservation.FilterMenu(FilterLogic.ApplyFilters()));
        }
        Options.Add("Return");
        Actions.Add(() => Reservation.FilterMenu());

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void AddFilter(bool IsEdited = false)
    {
        string Question = "What Filter would you like to add?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Movie title");
        Options.Add("Movie date");
        Options.Add("Movie category");
        Options.Add("Movie price");
        Options.Add("Return");

        Actions.Add(() => FilterTitle());
        Actions.Add(() => FilterTimeSlot());
        Actions.Add(() => FilterCategory());
        Actions.Add(() => FilterPrice());
        Actions.Add(() => Filter.Main(IsEdited));

        MenuLogic.Question(Question, Options, Actions);

        Filter.Main();
    }

    private static void RemoveFilter(bool IsEdited = false)
    {
        string Question = "What Filter would you like to Remove?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (var p in AppliedFilters.GetType().GetProperties())
        {
            switch (p.Name)
            {
                case var x when (x == "Title" && AppliedFilters.Title != ""):
                    Options.Add($"Selected movie title: {AppliedFilters.Title}");
                    Actions.Add(() => Filter.AppliedFilters.Title = "");
                    break;
                case var x when (x == "ReleaseDate" && AppliedFilters.ReleaseDate != DateTime.MinValue):
                    Options.Add($"Selected movie release date: {AppliedFilters.ReleaseDate}");
                    Actions.Add(() => Filter.AppliedFilters.ReleaseDate = DateTime.MinValue);
                    break;
                case var x when (x == "Categories" && AppliedFilters.Categories.Count > 0):
                    foreach (var category in AppliedFilters.Categories)
                    {
                        Options.Add($"Selected movie category: {category.Name}");
                        Actions.Add(() => Filter.AppliedFilters.Categories.Remove(category));
                    }
                    break;
                case var x when (x == "Price" && AppliedFilters.Price != 0.0):
                    Options.Add($"Selected movie price: {AppliedFilters.Price}");
                    Actions.Add(() => Filter.AppliedFilters.Price = 0.0);
                    break;
                default:
                    break;
            }
        }

        Options.Add("Return");
        Actions.Add(() => Filter.Main(IsEdited));

        MenuLogic.Question(Question, Options, Actions);

        Filter.Main(IsEdited);
    }

    private static void FilterTitle()
    {
        Console.Clear();
        Console.WriteLine("Enter the name of the movie you are trying to find ( leave empty to return )");
        string? ansTitle = Console.ReadLine();

        if (ansTitle == "")
        {
            Filter.AddFilter();
            return;
        }

        FilterLogic.CheckAppliedFilters();
        Filter.AppliedFilters.Title = ansTitle;

        Filter.AddFilter();
    }
    private static void FilterTimeSlot()
    {
        bool Err = false;
        DateTime parsedDateTime = DateTime.MinValue;

        Console.Clear();
        while (parsedDateTime == DateTime.MinValue)
        {

            Console.WriteLine("Enter the date you would like to watch a movie on ( DD/MM/YY or leave empty to return)");
            string? movieDate = Console.ReadLine();

            if (movieDate == "")
            {
                Filter.AddFilter();
                break;
            }

            if (!DateTime.TryParseExact(movieDate, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
            {
                Console.Clear();
                Console.Write("The Date or Time could not be converted, Please try again\n");
            }
        }

        FilterLogic.CheckAppliedFilters();
        Filter.AppliedFilters.ReleaseDate = parsedDateTime;

        Filter.AddFilter();
    }
    private static void FilterCategory()
    {
        CategoryLogic CategoryLogic = new CategoryLogic();
        string Question = "What category filter would you like to add?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (CategoryModel category in CategoryLogic.GetAllCategories().Result)
        {
            Options.Add(category.Name);
            Actions.Add(() => Filter.AppliedFilters.Categories.Add(category));
        }
        FilterLogic.CheckAppliedFilters();
        MenuLogic.Question(Question, Options, Actions);
        Filter.AddFilter();
    }
    private static void FilterPrice()
    {
        string? ansPrice = "";
        double toPrice = 0.0;
        bool Correct = false;

        Console.Clear();
        while (!Correct)
        {
            Console.WriteLine("Enter the price you want to spend at most ( 10,50 or leave empty to return)");
            ansPrice = Console.ReadLine().Replace(",", ".");

            if (ansPrice == "")
            {
                Filter.AddFilter();
                break;
            }

            try
            {
                toPrice = Math.Round(Convert.ToDouble(ansPrice), 2);

                Correct = true;
            }
            catch (System.FormatException)
            {
                Console.Clear();
                Console.WriteLine("Must be a number");
            }
        }

        FilterLogic.CheckAppliedFilters();
        Filter.AppliedFilters.Price = toPrice;

        Filter.AddFilter();
    }
}
