public static class Filter
{
    public static List<int> CatIds = new List<int>();
    public static void Main(bool IsEdited = false)
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        Options.Add("Add a filter");
        Actions.Add(() => AddFilter(IsEdited));
        if (CatIds != null)
        {
            Options.Add("Remove a filter");
            Actions.Add(() => RemoveFilter(IsEdited));
        }
        ;
        MenuLogic.Question(Question, Options, Actions);
    }

    private static void AddFilter(bool IsEdited = false)
    {
        string Question = "What Filter would you like to add?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        CategoryLogic CatL = new CategoryLogic();
        foreach (CategoryModel cat in CatL.AllCategories())
        {
            Options.Add(cat.Name);
            Actions.Add(() => Filter.CatIds.Add(cat.Id));
        }

        MenuLogic.Question(Question, Options, Actions);

        var movies = new MoviesLogic().FilterOnCategories(CatIds);
        Reservation.FilteredMenu(movies, IsEdited);
    }
    private static void RemoveFilter(bool IsEdited = false)
    {
        string Question = "What Filter would you like to Remove?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        CategoryLogic CatL = new CategoryLogic();

        List<CategoryModel> CurrentCats = new List<CategoryModel>();

        foreach (int catId in Filter.CatIds)
        {
            CurrentCats.Add(CatL.GetById(catId));
        }
        foreach (CategoryModel cat in CurrentCats)
        {
            Options.Add(cat.Name);
            Actions.Add(() => Filter.CatIds.Remove(cat.Id));
        }

        MenuLogic.Question(Question, Options, Actions);

        if (Filter.CatIds.Count != 0)
        {
            var movies = new MoviesLogic().FilterOnCategories(CatIds);
            Reservation.FilteredMenu(movies, IsEdited);
        }
        else
        {
            Reservation.NoFilterMenu(IsEdited);
        }


    }
}