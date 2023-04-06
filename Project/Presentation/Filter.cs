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
            Console.ReadKey();
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
        Options.Add("Movie date/time");
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


        foreach (object filter in AppliedFilters)
        {
            if (filter.GetType() == typeof(MovieModel))
            {
                MovieModel movie = ((MovieModel)filter);
                // MovieModel movieApply = ((MovieModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(MovieModel)));

                if (movie.Title != null)
                {
                    Options.Add($"Selected movie title: {movie.Title}");
                    ((MovieModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(MovieModel))).Title = null
                    Actions.Add(() => );
                }
                if (movie.Price != 0)
                {
                    Options.Add($"Selected movie price: {movie.Price}");
                    Actions.Add(() => ((MovieModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(MovieModel))).Price = 0);
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
                Options.Add($"Selected Date/time Date: {timeSlotDT.ToString("dd/MM/yy")}, Time: {timeSlotDT.ToString("hh:mm")}");
                Actions.Add(() => Filter.AppliedFilters.Remove(filter));
            }
        }
        Options.Add("Return");
        Actions.Add(() => Filter.Main(IsEdited));

        MenuLogic.Question(Question, Options, Actions);

        Filter.Main(IsEdited);
    }

    private static void FilterTitle()
    {
        MovieModel MovieModel = (MovieModel)Filter.AppliedFilters.Find(x => x.GetType() == typeof(MovieModel));

        string ansTitle = QuestionLogic.AskString("Enter the name of the movie you are trying to find");

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
        // https://stackoverflow.com/questions/22060758/how-to-convert-string-to-datetime-in-c
        // try parse excact 

        TimeSlotModel TimeSlotModel = new(0, 0, new DateTime(), null);
        bool Err = false;
        DateTime parsedDateTime = DateTime.MinValue;
        while (parsedDateTime == DateTime.MinValue)
        {
            string movieDate = QuestionLogic.AskString("Enter the date you would like to watch a movie on ( DD/MM/YY )");
            string movieTime = QuestionLogic.AskString("Now enter at what time you would like to start watching ( 00:00 )");

            movieDate.Replace(" ", "");
            movieTime.Replace(" ", "");

            string toConvert = $"{movieDate} {movieTime}";

            if (!DateTime.TryParseExact(toConvert, "dd/MM/yy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
            {
                Console.Write("The Date or Time could not be converted, Please try again\n");
                System.Threading.Thread.Sleep(2000);
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

        double ansPrice = QuestionLogic.AskNumber("Enter the price you want to spend at most ( 10,50 )");

        if (MovieModel == null)
        {
            MovieModel = new(0, null, new DateTime(), null, null, 0, 0, null);
            MovieModel.Price = ansPrice;
            AppliedFilters.Add(MovieModel);
            Filter.AddFilter();
        }

        MovieModel.Price = ansPrice;
        Filter.AddFilter();
    }

    private static List<MovieModel> ApplyFilters()
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