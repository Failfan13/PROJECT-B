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
        string Categories = "";

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
        Categories = QuestionLogic.AskString("What is/are the categorie('s) of the movie");


        MovieModel movie = MoviesLogic.NewMovie(Title, ReleaseDate, Director, Description, Duration, Categories);

        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
        Console.WriteLine($"Director: {movie.Director}");
        Console.WriteLine($"Categories: {movie.Categories}");

        Menu.Start();
    }

}