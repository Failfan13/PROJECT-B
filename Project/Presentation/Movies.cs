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
        List<CategoryModel> Categories = new List<CategoryModel>{};

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
                List<CategoryModel> cats =CategoryLogic.AllCategories();
                Console.Clear();
                // stil working on it
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

    public static void ChangeCategory()
    {
        Console.Clear();
        string moviename = QuestionLogic.AskString("What movie do you want to change?");
        foreach (MovieModel movie in MoviesLogic.AllMovies())
        {
            if (moviename == movie.Title)
            {
                bool validinput = true;
                Console.Clear();
                do
                {
                    int addoremove = QuestionLogic.AskNumber("What do you want to do?\n1 Add a category\n2 Remove a category");
                    if (addoremove == 1)
                    {
                        MoviesLogic.AddCategory(movie);
                    }
                    else if (addoremove == 2)
                    {
                        MoviesLogic.RemoveCategory(movie);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input please enter 1 or 2");
                        Console.Clear();
                    }
                }
                while(validinput);
            }
        }
        Console.WriteLine("That movie does not exist in the database.\nPress enter to continue");
        Console.ReadLine();
        Menu.Start();
    }

}