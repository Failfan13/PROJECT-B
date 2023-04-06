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
            Actions.Add(() => Reservation.FilteredMenu());
        }
        else
        {
            Options.Add("Return");
            Actions.Add(() => Reservation.NoFilterMenu());
        }

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void AddFilter(bool IsEdited = false)
    {
        string Question = "What Filter would you like to add?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        // Options.Add("Movie name");
        Options.Add("Movie time");
        Options.Add("Movie category");
        Options.Add("Movie price");
        Options.Add("Return");

        // Actions.Add(() => Console.WriteLine("sus1"));
        Actions.Add(() => FilterTimeSlot());
        Actions.Add(() => FilterCategory());
        Actions.Add(() => Console.WriteLine("sus4"));
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
            // if (filter.GetType() == typeof(MovieModel))
            // {
            //     Options.Add(((MovieModel)filter).Title);
            // }
            // else 
            if (filter.GetType() == typeof(CategoryModel))
            {
                Options.Add(((CategoryModel)filter).Name);
            }
            else if (filter.GetType() == typeof(TimeSlotModel))
            {
                DateTime timeSlotDT = ((TimeSlotModel)filter).Start;
                Options.Add($"Time for Date: {timeSlotDT.ToString("dd/MM/yy")}, Time: {timeSlotDT.ToString("hh:mm")}");
            }
            Actions.Add(() => Filter.AppliedFilters.Remove(filter));
        }
        Options.Add("Return");
        Actions.Add(() => Filter.Main(IsEdited));

        MenuLogic.Question(Question, Options, Actions);

        Filter.Main(IsEdited);
    }

    // private static void FilterMovie()
    // {
    //     CategoryLogic MovieLogic = new();
    //     string Question = "Enter the name of the movie";
    //     List<string> Options = new List<string>();
    //     List<Action> Actions = new List<Action>();

    //     foreach (CategoryModel category in CategoryLogic.AllCategories())
    //     {
    //         Options.Add(category.Name);
    //         Actions.Add(() => Filter.AppliedFilters.Add(category));
    //     }

    //     MenuLogic.Question(Question, Options, Actions);

    //     Filter.AddFilter();
    // }
    private static void FilterTimeSlot()
    {
        // https://stackoverflow.com/questions/22060758/how-to-convert-string-to-datetime-in-c
        // try parse excact 

        TimeSlotModel TimeSlotModel = new(0, 0, new DateTime(), null);
        bool Err = false;
        DateTime parsedDateTime = DateTime.MinValue;
        while (parsedDateTime == DateTime.MinValue)
        {
            string movieDate = QuestionLogic.AskString("Enter the date you would like to watch a movie on DD/MM/YY");
            string movieTime = QuestionLogic.AskString("Now enter at what time you would like to start watching 00:00");

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
}