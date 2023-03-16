static class Movies
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void ShowAllMovies()
    {
        foreach (MovieModel movie in MoviesLogic.AllMovies())
        {
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
            Console.WriteLine($"Director: {movie.Director}\n");
        }
    }


    public static void AddNewMovie()
    {
        bool CorrectTitle = true;
        bool CorrectDate = true;
        bool CorrectDirector = true;

        string Title = "";
        string Director = "";
        DateTime ReleaseDate = new DateTime();

        while (CorrectTitle)
        {
            Console.WriteLine("What is the title of the movie?: ");
            Title = Console.ReadLine();
            if (Title != null)
            {
                CorrectTitle = false;
            }
            else
            {
                Console.WriteLine("Can't be empty");
            }
        }

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

        while (CorrectDirector)
        {
            Console.WriteLine("Who is the director of the movie?: ");
            Director = Console.ReadLine();
            if (Director != null)
            {
                CorrectDirector = false;
            }
            else
            {
                Console.WriteLine("Can't be empty");
            }
        }

        MovieModel movie = MoviesLogic.NewMovie(Title, ReleaseDate, Director);

        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
        Console.WriteLine($"Director: {movie.Director}");

        Menu.Start();
    }
}