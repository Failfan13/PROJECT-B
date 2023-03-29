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
        List<CategoryModel> Categories = new List<CategoryModel> { };

        Console.Clear();
        Title = QuestionLogic.AskString("What is the title of the movie?");

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
        Duration = QuestionLogic.AskNumber("What is the duration? (minutes)");
        bool nomorecategories = false;
        do
        {
            int anothercat = QuestionLogic.AskNumber("Add another category?");
            if (anothercat == 1)
            {
                List<CategoryModel> cats = CategoryLogic.AllCategories();
                Console.Clear();
                foreach (CategoryModel c in cats)
                {
                    Console.WriteLine($"{c.Id} {c.Name}");
                }
                int categorytoadd = QuestionLogic.AskNumber("What category do you want to add");
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


        MovieModel movie = MoviesLogic.NewMovie(Title, ReleaseDate, Director, Description, Duration, Categories);

        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
        Console.WriteLine($"Director: {movie.Director}");
        Console.WriteLine($"Categories: {string.Join(", ", movie.Categories)}");

        Menu.Start();
    }

    public static void ChangeMoviesMenu()
    {
        Console.Clear();
        List<MovieModel> movies = MoviesLogic.AllMovies();
        for (int i = 0; i < movies.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {movies[i].Title}");
        }
        Console.WriteLine($"{movies.Count + 1}. Return");
        int awnser = QuestionLogic.AskNumber("Enter the number to select the movie");

        if (awnser == movies.Count + 1)
        {
            Menu.Start();
        }
        ChangeMovieMenu(movies[awnser - 1]);

    }

    public static void ChangeMovieMenu(MovieModel movie)
    {
        Console.Clear();
        Console.WriteLine("What would you like to do?");
        List<string> options = new List<string>() { "Change title", "Change categories", "Change Director", "Change Date", "Change Description" };
        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }
        Console.WriteLine($"{options.Count + 1}. Return");

        int awnser = QuestionLogic.AskNumber("");

        if (awnser == 1)
        {
            // Change title
        }
        else if (awnser == 2)
        {
            ChangeCategory(movie);
        }
        else if (awnser == options.Count + 1)
        {
            ChangeMoviesMenu();
        }
    }
    public static void ChangeCategory(MovieModel movie)
    {
        Console.Clear();
        var validinput = true;
        do
        {
            int addoremove = QuestionLogic.AskNumber("What do you want to do?\n1 Add a category\n2 Remove a category");
            if (addoremove == 1)
            {
                MoviesLogic.AddCategory(movie);
                validinput = false;
                
            }
            else if (addoremove == 2)
            {
                MoviesLogic.RemoveCategory(movie);
                validinput = false;
            }
            else
            {
                Console.WriteLine("Invalid input please enter 1 or 2");
                Console.Clear();
            }
        }
        while (validinput);
    }
}

