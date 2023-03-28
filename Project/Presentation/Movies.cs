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


        MovieModel movie = MoviesLogic.NewMovie(Title, ReleaseDate, Director, Description, Duration);

        Console.WriteLine("New movie added!");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.Date}");
        Console.WriteLine($"Director: {movie.Director}");

        Menu.Start();
    }

    public static MovieModel AskMovie()
    {
        bool CorrectInput = true;
        MovieModel choice = null;

        Console.Clear();
        Console.WriteLine("Which movie would you like to see?");
        foreach (MovieModel movie in MoviesLogic.AllMovies())
        {
            Console.WriteLine($"{movie.Id + 1}. {movie.Title}");
        }

        while (CorrectInput)
        {
            int awnser = QuestionLogic.AskNumber("\nEnter number to continue:", MoviesLogic.AllMovies().Count);
            try
            {
                choice = MoviesLogic.GetById(awnser);
                ShowMovieTimeSlots(choice);
                return choice;
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Incorrect number");
            }
        }

        return null;
    }

    public static DateTime ShowMovieTimeSlots(MovieModel movie)
    {
        movie.Info();
        return DateTime.Now;
    }
}