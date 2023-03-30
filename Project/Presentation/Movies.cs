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
        List<string> options = new List<string>() { "Change title", "Change Director", "Change Releasedate", "Change Description", "Change Categories", "Change Duration" };
        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }
        Console.WriteLine($"{options.Count + 1}. Return");

        int awnser = QuestionLogic.AskNumber("");

        if (awnser == 1)
        {
            ChangeTitle(movie);
        }
        else if (awnser == 2)
        {
            ChangeDirector(movie);
        }
        else if (awnser == 3)
        {
            ChangeReleaseDate(movie);
        }
        else if (awnser == 4)
        {
            ChangeDescription(movie);
        }
        else if (awnser == 5)
        {
            ChangeCategory(movie);
        }
        else if (awnser == 6)
        {
            ChangeDuration(movie);
        }
        else if (awnser == options.Count + 1)
        {
            ChangeMoviesMenu();
        }
    }
    public static void ChangeCategory(MovieModel movie)
    {
        bool validinput = true;
        Console.Clear();
        do
        {
            int addoremove = QuestionLogic.AskNumber("What do you want to do?\n1 Add a category\n2 Remove a category");
            if (addoremove == 1)
            {
                CategoryLogic.AddCategory(movie);
            }
            else if (addoremove == 2)
            {
                CategoryLogic.RemoveCategory(movie);
            }
            else
            {
                Console.WriteLine("Invalid input please enter 1 or 2");
                Console.Clear();
            }
        }
        while (validinput);
    }


    public static void ChangeTitle(MovieModel movie)
    {
        string NewTitle = QuestionLogic.AskString("What do you want to change the title of this movie to?");
        MoviesLogic.ChangeTitle(movie, NewTitle);
        Console.WriteLine($"Title is now: {NewTitle}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    public static void ChangeDirector(MovieModel movie)
    {
        string NewDirector = QuestionLogic.AskString("What do you want to change the director of this movie to?");
        MoviesLogic.ChangeDirector(movie, NewDirector);
        Console.WriteLine($"Director is now: {NewDirector}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    public static void ChangeDescription(MovieModel movie)
    {
        string NewDescription = QuestionLogic.AskString("What do you want to change the description of this movie to?");
        MoviesLogic.ChangeDescription(movie, NewDescription);
        Console.WriteLine($"Description is now: {NewDescription}");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    public static void ChangeDuration(MovieModel movie)
    {
        int NewDuration = QuestionLogic.AskNumber("What do you want to change the duration of this movie to? (please enter the ammount of minutes)");
        MoviesLogic.ChangeDuration(movie, NewDuration);
        Console.WriteLine($"Duration is now: {NewDuration} minutes");
        QuestionLogic.AskEnter();
        ChangeMovieMenu(movie);
    }
    public static void ChangeReleaseDate(MovieModel movie)
    {
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
}

