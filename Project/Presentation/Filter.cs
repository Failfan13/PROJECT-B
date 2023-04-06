using System.Globalization;

public static class Filter
{
    public static List<object> AppliedFilters = new();
    public static void Main(bool IsEdited = false)
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        Options.Add("Add a filter");
        Actions.Add(() => Filter.AddFilter(IsEdited));

        if (AppliedFilters.Any())
        {
            Options.Add("Remove a filter");
            Actions.Add(() => Filter.RemoveFilter(IsEdited));
            Options.Add("Apply filters");
            Actions.Add(() => Reservation.FilterMenu(Filter.ApplyFilters()));
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
        MovieModel movieApply = ((MovieModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(MovieModel)));

        foreach (object filter in AppliedFilters)
        {
            if (filter.GetType() == typeof(MovieModel))
            {
                MovieModel movie = ((MovieModel)filter);
                if (movie.Title != null)
                {
                    Options.Add($"Selected movie title: {movie.Title}");
                    Actions.Add(() => movieApply.Title = null);
                }

                if (movie.Price != 0)
                {
                    Options.Add($"Selected movie price: {movie.Price}");
                    Actions.Add(() => movieApply.Price = 0);
                }
            }
            else if (filter.GetType() == typeof(CategoryModel))
            {
                Options.Add($"Selected category: {((CategoryModel)filter).Name}");
                Actions.Add(() => Filter.AppliedFilters.Remove(filter));
            }
            else if (filter.GetType() == typeof(TimeSlotModel))
            {
                DateTime timeSlotDT = ((TimeSlotModel)filter).Start;
                Options.Add($"Selected Date: {timeSlotDT.ToString("dd/MM/yy")}");
                Actions.Add(() => Filter.AppliedFilters.Remove(filter));
            }
        }
        Options.Add("Return");
        Actions.Add(() => Filter.Main(IsEdited));

        MenuLogic.Question(Question, Options, Actions);

        if ((movieApply != null) && movieApply.Title == null && movieApply.Price == 0)
        {
            Filter.AppliedFilters.Remove(movieApply);
        }

        Filter.Main(IsEdited);
    }

    private static void FilterTitle()
    {
        MovieModel MovieModel = (MovieModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(MovieModel));

        Console.Clear();
        Console.WriteLine("Enter the name of the movie you are trying to find ( leave empty to return )");
        string ansTitle = Console.ReadLine();

        if (ansTitle == "")
        {
            Filter.AddFilter();
            return;
        }

        if (MovieModel == null)
        {
            MovieModel = new(0, null, new DateTime(), null, null, 0, 0, null);
            MovieModel.Title = ansTitle;
            AppliedFilters.Add(MovieModel);
            Filter.AddFilter();
        }

        MovieModel.Title = ansTitle;
        Filter.AddFilter();
    }
    private static void FilterTimeSlot()
    {
        TimeSlotModel TimeSlotModel = new(0, 0, new DateTime(), null);
        bool Err = false;
        DateTime parsedDateTime = DateTime.MinValue;

        Console.Clear();
        while (parsedDateTime == DateTime.MinValue)
        {

            Console.WriteLine("Enter the date you would like to watch a movie on ( DD/MM/YY or leave empty to return)");
            string movieDate = Console.ReadLine();

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

        TimeSlotModel.Start = parsedDateTime;
        AppliedFilters.Add(TimeSlotModel);
        Filter.AddFilter();
    }
    private static void FilterCategory()
    {
        CategoryLogic CategoryLogic = new();
        string Question = "What category filter would you like to add?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (CategoryModel category in CategoryLogic.AllCategories())
        {
            Options.Add(category.Name);
            Actions.Add(() => Filter.AppliedFilters.Add(category));
        }

        MenuLogic.Question(Question, Options, Actions);

        Filter.AddFilter();
    }
    private static void FilterPrice()
    {
        MovieModel MovieModel = (MovieModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(MovieModel));
        string ansPrice = "";
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

        if (MovieModel == null)
        {
            MovieModel = new(0, null, new DateTime(), null, null, 0, 0, null);
            MovieModel.Price = toPrice;
            AppliedFilters.Add(MovieModel);
            Filter.AddFilter();
        }

        MovieModel.Price = toPrice;
        Filter.AddFilter();
    }

    public /*private*/ static List<MovieModel> ApplyFilters()
    {
        MovieModel movieFilter = (MovieModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(MovieModel));
        CategoryModel categoryFilter = (CategoryModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(CategoryModel));
        TimeSlotModel timeslotFilter = (TimeSlotModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(TimeSlotModel));

        //List<MovieModel> catFilter = MoviesLogic.FilterOnCategories(categoryFilter);
        Console.WriteLine(movieFilter);
        Console.WriteLine(categoryFilter);
        Console.WriteLine(timeslotFilter);

        Console.ReadKey();
        return null;
    }
}