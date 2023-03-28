static class Movies
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

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


        MovieModel movie = MoviesLogic.NewMovie(Title, ReleaseDate, Director, Description, Duration);

        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
        Console.WriteLine($"Director: {movie.Director}");

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
        List<string> options = new List<string>() { "Change title", "Change Director", "Change Date", "Change Description" };
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
            // change director
        }
        else if (awnser == options.Count + 1)
        {
            ChangeMoviesMenu();
        }
    }
}