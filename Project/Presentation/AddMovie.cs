static class AddMovie
{
 static private MoviesLogic MoviesLogic = new MoviesLogic();
 public static void Start()
 {
  Console.WriteLine("What is the title of the movie?: ");
  string Title = Console.ReadLine();
  Console.WriteLine("What is the release date of the movie? (dd/mm/yyyy): ");
  DateTime ReleaseDate = Convert.ToDateTime(Console.ReadLine());
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