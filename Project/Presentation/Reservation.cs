public static class Reservation
{
    static private ReservationLogic ReservationLogic = new();
    static private CategoryLogic CategoryLogic = new();
    public static void start()
    {
        NoFilterMenu();
    }

    public static void NoFilterMenu()
    {
        Filter.CatIds = new List<int>();
        var movies = new MoviesLogic().AllMovies();

        string Question = "which movie would you like to see?";
        List<string> Movies = new List<string>();
        List<Action> Actions = new List<Action>();

        Movies.Add("Use Filter");
        Actions.Add(() => Filter.Main());


   
        foreach (MovieModel movie in movies)
        {
  
            //checks if the movie category is 18+ and if the user is not an adult so adult movies are not shown to underaged users
            if (AccountsLogic.CurrentAccount.Adult == false && movie.Categories[0].Name == "18+")
                {
                    continue;
                }
            Movies.Add(movie.Title);
            Actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movie.Id, movie.Title));            
                   
            
        
        }
        Movies.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Movies, Actions);
    }

    public static void FilteredMenu(List<MovieModel> movies)
    {
        string Question = "which movie would you like to see?";
        List<string> Movies = new List<string>();
        List<Action> Actions = new List<Action>();
        Movies.Add("Use Filter");
        Actions.Add(() => Filter.Main());

        foreach (MovieModel movie in movies)
        {
            Movies.Add(movie.Title);
            Actions.Add(() => TimeSlots.ShowAllTimeSlotsForMovie(movie.Id, movie.Title));
        }
        Movies.Add("Return");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Movies, Actions);
    }
    // Increases total order amount
    //ReservationLogic.TotalOrder = 5.5;

    // Decreases total order amount
    //ReservationLogic.TotalOrderDecr = 2.3;

    // Show total order amount
    public static void TotalReservationCost()
    {
        Console.Write("The total cost of your order will be:\n");
        //Order as string
        string orderCost = ReservationLogic.TotalOrder.ToString();
        //Show euro symbol
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        //Print total cost + if not containing "." add ",-" at end
        Console.WriteLine($"€ " + orderCost + (orderCost.Contains(".") ? "" : ",-"));
    }
}
