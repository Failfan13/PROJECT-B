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
        bool CorrectDate = true;
        DateTime ReleaseDate = new DateTime();
        Console.WriteLine("What is the title of the movie?: ");
        string Title = Console.ReadLine();
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

        Console.WriteLine("Who is the director of the movie?: ");
        string Director = Console.ReadLine();
        MovieModel movie = MoviesLogic.NewMovie(Title, ReleaseDate, Director);

        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
        Console.WriteLine($"Director: {movie.Director}");

        Menu.Start();
    }
}