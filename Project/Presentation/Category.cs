public static class Category
{
    private static CategoryLogic CL = new CategoryLogic();
    public static void Start()
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>() { "Create new category", "Remove a category", "\nReturn" };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => NewCatMenu());
        Actions.Add(() => RemoveCatMenu());
        Actions.Add(() => Admin.Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void NewCatMenu()
    {
        string newCat = QuestionLogic.AskString("What name should the new category have?");
        CategoryModel newCatModel = new CategoryModel();
        CL.CreateNewCategory(newCatModel.NewCategoryModel(newCat));
    }
    public static void RemoveCatMenu()
    {
        string Question = "What category would you like to remove?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (CategoryModel cat in CL.GetAllCategories().Result)
        {
            Options.Add(cat.Name);
            Actions.Add(async () => await CL.DeleteCategory(cat.Id));
        }
        Options.Add("\nReturn");
        Actions.Add(() => Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void CategoryMenu(MovieModel movie)
    {
        bool finishCategory = false;

        Console.Clear();

        string Qeustion = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (!CL._swapCategory)
        {
            Options.Add("Swap Mode, Currently: Adding category");
        }
        else
        {
            Options.Add("Swap Mode, Currently: Removing category");
        }
        Actions.Add(() => CL.SwapMode());

        if (!CL._swapCategory)
        {
            foreach (CategoryModel cat in CL.GetAllCategories().Result.Where(c => !movie.Categories.Contains(c)))
            {
                Options.Add(cat.Name);
                Actions.Add(() => CL.AddCategoryToMovie(movie, cat));
            }
        }
        else
        {
            foreach (CategoryModel cat in movie.Categories)
            {
                Options.Add(cat.Name);
                Actions.Add(() => CL.RemoveCategoryFromMovie(movie, cat));
            }
        }

        Options.Add("\nFinish");
        Actions.Add(() => finishCategory = true);

        MenuLogic.Question(Qeustion, Options, Actions);

        if (!finishCategory) CategoryMenu(movie);
    }

    public static void AddCategory(MovieModel movie)
    {
        MoviesLogic MoviesLogic = new MoviesLogic();

        string Question = "Choose a category to add to the movie";
        while (true)
        {
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            // show all category's not selected yet
            foreach (var category in CL.GetAllCategories().Result.Where(c => !movie.Categories.Contains(c)))
            {
                Options.Add(category.Name);
                Actions.Add(() => movie.Categories.Add(category));
            }

            MenuLogic.Question(Question, Options, Actions);
            MoviesLogic.UpdateList(movie);

            Console.WriteLine("Would you like to add or remove a category? (a/r/n)");
            ConsoleKeyInfo inputKey = Console.ReadKey();
            if (inputKey.Key == ConsoleKey.A)
            {
                Question = "Choose a category to add to the movie";
            }
            else if (inputKey.Key == ConsoleKey.R)
            {
                Question = "Choose a category to remove from the movie";

            }
            else
            {
                break;
            }
        }
    }
}