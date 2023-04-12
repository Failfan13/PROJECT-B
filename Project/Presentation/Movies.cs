static class Movies
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();
    static private CategoryLogic CategoryLogic = new CategoryLogic();

    public static void ShowAllMovies()
    {
        foreach (MovieModel movie in MoviesLogic.AllMovies())
        {
            movie.Info();
        }
    }

    public static void AddNewMovie()
    {
        bool CorrectDate = true;

        int Duration = 0;
        string Title = "";
        string Director = "";
        string Description = "";
        DateTime ReleaseDate = new DateTime();
        double Price = 0;
        List<CategoryModel> Categories = new List<CategoryModel> { };
        List<string> Formats = new List<string> { };

        Console.Clear();
        Title = QuestionLogic.AskString("What is the title of the movie?");

        Console.Clear();
        while (CorrectDate)
        {
            Console.WriteLine("What is the release date of the movie? (dd/mm/yyyy): ");
            try
            {
                ReleaseDate = Convert.ToDateTime(Console.ReadLine());
                CorrectDate = false;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Wrong date format!");
            }
        }

        Description = QuestionLogic.AskString("What is the description of the movie? ");
        Director = QuestionLogic.AskString("Who is the director of the movie?: ");
        Duration = (int)QuestionLogic.AskNumber("What is the duration? (minutes)");
        Price = (int)QuestionLogic.AskNumber("How expensive is the movie?: ");

        bool nomorecategories = false;
        do
        {
            int anothercat = (int)QuestionLogic.AskNumber("Add another category?");
            if (anothercat == 1)
            {
                List<CategoryModel> cats = CategoryLogic.AllCategories();
                Console.Clear();
                foreach (CategoryModel c in cats)
                {
                    Console.WriteLine($"{c.Id} {c.Name}");
                }
                int categorytoadd = (int)QuestionLogic.AskNumber("What category do you want to add");
                CategoryModel category = CategoryLogic.GetById(categorytoadd);
                if (!Categories.Contains(category))
                {
                    Categories.Add(category);
                }
            }
            else if (anothercat == 2)
                nomorecategories = true;
        }
        while (nomorecategories != false);

        MovieModel movie = MoviesLogic.NewMovie(Title, ReleaseDate, Director, Description, Duration, Price, Categories, Formats);

        ChangeFormats(movie);


        Console.Clear();
        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
        Console.WriteLine($"Director: {movie.Director}");
        Console.WriteLine($"Price: {movie.Price}");
        Console.WriteLine($"Categories: {string.Join(", ", movie.Categories)}");
        Console.WriteLine($"Formats: {string.Join(", ", movie.Formats)}");

        //Menu.Start();
    }

    public static void ChangeMoviesMenu()
    {
        Console.Clear();
        List<MovieModel> movies = MoviesLogic.AllMovies();
        string Question = "What movie would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();
        foreach (var movie in movies)
        {
            Options.Add(movie.Title);
            Actions.Add(() => ChangeMovieMenu(movie));
        }

        Options.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);

    }

    public static void ChangeMovieMenu(MovieModel movie)
    {
        Console.Clear();
        //Console.WriteLine("What would you like to do?");
        string Question = "What would you like to change";

        List<string> Options = new List<string>() {
            "Change title", "Change Director",
            "Change Releasedate", "Change Description",
            "Change Duration", "Change Price",
            "Change Categories", "Change Formats"
        };

        List<Action> Actions = new List<Action>() { };
        Actions.Add(() => ChangeTitle(movie));
        Actions.Add(() => ChangeDirector(movie));
        Actions.Add(() => ChangeReleaseDate(movie));
        Actions.Add(() => ChangeDescription(movie));
        Actions.Add(() => ChangeDuration(movie));
        Actions.Add(() => ChangePrice(movie));
        Actions.Add(() => ChangeCategory(movie));
        Actions.Add(() => ChangeFormats(movie, () => ChangeMovieMenu(movie)));

        Options.Add("Return");
        Actions.Add(() => ChangeMoviesMenu());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ChangeCategory(MovieModel movie)
    {
        string Question = "What would you like to do?";
        List<string> Options = new List<string>() { "Add a category", "Remove a category" };
        List<Action> Actions = new List<Action>();

        Actions.Add(() => CategoryLogic.AddCategory(movie));
        Actions.Add(() => CategoryLogic.RemoveCategory(movie));

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ChangeFormats(MovieModel movie, Action Action = null)
    {
        string Question = "Would you like to change viewing methods?";
        List<string> Options = new List<string>() { "Add a viewing method", "Remove a viewing method" };
        List<Action> Actions = new List<Action>();

        Actions.Add(() => Movies.AddViewFormat(movie));
        Actions.Add(() => Movies.RemoveViewFormat(movie));

        Options.Add("Return");
        Actions.Add(() => Console.WriteLine(""));

        MenuLogic.Question(Question, Options, Actions);

        Action?.Invoke();
    }

    private static void ChangeTitle(MovieModel movie)
    {
        Console.Clear();
        string NewTitle = QuestionLogic.AskString("What do you want to change the title of this movie to?");
        MoviesLogic.ChangeTitle(movie, NewTitle);
        Console.WriteLine($"Title is now: {NewTitle}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangeDirector(MovieModel movie)
    {
        Console.Clear();
        string NewDirector = QuestionLogic.AskString("What do you want to change the director of this movie to?");
        MoviesLogic.ChangeDirector(movie, NewDirector);
        Console.WriteLine($"Director is now: {NewDirector}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangeDescription(MovieModel movie)
    {
        Console.Clear();
        string NewDescription = QuestionLogic.AskString("What do you want to change the description of this movie to?");
        MoviesLogic.ChangeDescription(movie, NewDescription);
        Console.WriteLine($"Description is now: {NewDescription}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangeDuration(MovieModel movie)
    {
        int NewDuration = (int)QuestionLogic.AskNumber("What do you want to change the duration of this movie to? (please enter the ammount of minutes)");
        MoviesLogic.ChangeDuration(movie, NewDuration);
        Console.WriteLine($"Duration is now: {NewDuration} minutes");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangePrice(MovieModel movie)
    {
        double NewPrice = QuestionLogic.AskNumber("What do you want to change the price of this movie to?");
        MoviesLogic.ChangePrice(movie, NewPrice);
        Console.WriteLine($"Price is now: {NewPrice}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    private static void ChangeReleaseDate(MovieModel movie)
    {
        Console.Clear();
        DateTime NewReleaseDate = new DateTime();
        bool CorrectDate = true;
        while (CorrectDate)
        {
            Console.WriteLine("What is the release date of the movie? (dd/mm/yyyy): ");
            try
            {
                NewReleaseDate = Convert.ToDateTime(Console.ReadLine());
                CorrectDate = false;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Wrong date format!");
            }
        }
        MoviesLogic.ChangeReleaseDate(movie, NewReleaseDate);
        Console.WriteLine($"Release date  is now: {NewReleaseDate.Date}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }

    public static void AddViewFormat(MovieModel movie)
    {
        string Question = "Select the format you want to add";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (MoviesLogic.AllFormats().Any())
        {
            foreach (var format in MoviesLogic.AllFormats())
            {
                Options.Add(format);
                Actions.Add(() => MoviesLogic.AddFormat(movie, format));
            }

            MenuLogic.Question(Question, Options, Actions);
        }

        MoviesLogic.UpdateList(movie);

        ChangeFormats(movie);
    }

    public static void RemoveViewFormat(MovieModel movie)
    {
        string Question = "Select the format you want to remove";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        if (movie.Formats.Any())
        {
            foreach (var format in movie.Formats)
            {
                Options.Add(format);
                Actions.Add(() => MoviesLogic.RemoveFormat(movie, format));
            }
            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Console.WriteLine("There are no formats to remove");
        }

        MoviesLogic.UpdateList(movie);

        ChangeFormats(movie);
    }
}

