public static class Reservation
{
    public static void start()
    {
        bool CorrectInput = true;

        MovieModel choice = null;
        Console.Clear();
        Console.WriteLine("Which movie would you like to see?");
        var movies = new MoviesLogic();
        foreach (MovieModel movie in movies.AllMovies())
        {
            Console.WriteLine($"{movie.Id + 1}. {movie.Title}.");
        }

        int awnser = QuestionLogic.AskNumber("\nEnter number to continue:");
        while (CorrectInput)
        {
            Console.WriteLine("\nEnter number to continue:");
            try
            {
                choice = movies.GetById(awnser);
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Incorrect number");
            }
        }

        ShowMovieTimeSlots(choice);

    }

    public static void ShowMovieTimeSlots(MovieModel movie)
    {
        movie.Info();

    }
}
