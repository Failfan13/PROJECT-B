public static class Filter
{
    public static MoviesLogic MoviesLogic = new();
    public static List<object> AppliedFilters = new();
    public static void Main(bool IsEdited = false)
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        Options.Add("Add a filter");
        Actions.Add(() => AddFilter(IsEdited));
        if (AppliedFilters.Any())
        {
            Options.Add("Remove a filter");
            Actions.Add(() => RemoveFilter(IsEdited));
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

        CategoryLogic CatL = new CategoryLogic();
        // Options.Add("Movie name");
        // Options.Add("Movie time");
        // Options.Add("Movie category");
        // Options.Add("Movie price");

        foreach (CategoryModel cat in CatL.AllCategories())
        {
            Options.Add(cat.Name);
            Actions.Add(() => Filter.AppliedFilters.Add(cat));
        }
        Options.Add("Return");
        Actions.Add(() => Main(IsEdited));

        MenuLogic.Question(Question, Options, Actions);

        //var movies = new MoviesLogic().FilterOnCategories(CatIds);
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
                Options.Add(((MovieModel)filter).Title);
            }
            else if (filter.GetType() == typeof(CategoryModel))
            {
                Options.Add(((CategoryModel)filter).Name);
            }
            else if (filter.GetType() == typeof(TimeSlotModel))
            {

            }
            Actions.Add(() => AppliedFilters.Remove(filter));
        }
        Options.Add("Return");
        Actions.Add(() => Main(IsEdited));

        MenuLogic.Question(Question, Options, Actions);

        Main(IsEdited);
        // CategoryLogic CatL = new CategoryLogic();

        // List<CategoryModel> CurrentCats = new List<CategoryModel>();

        // foreach (int catId in Filter.CatIds)
        // {
        //     CurrentCats.Add(CatL.GetById(catId));
        // }
        // foreach (CategoryModel cat in CurrentCats)
        // {
        //     Options.Add(cat.Name);
        //     Actions.Add(() => Filter.CatIds.Remove(cat.Id));
        // }

        // Options.Add("Return");
        // Actions.Add(() => Main(IsEdited));
        // MenuLogic.Question(Question, Options, Actions);

        // if (Filter.CatIds.Count != 0)
        // {
        //     var movies = new MoviesLogic().FilterOnCategories(CatIds);
        //     Reservation.FilteredMenu(movies, IsEdited);
        // }
        // else
        // {
        //     Reservation.NoFilterMenu(IsEdited);
        // }
    }
}