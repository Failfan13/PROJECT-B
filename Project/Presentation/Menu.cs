static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 add movie");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Movies.AddNewMovie();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }

    static public void LoggedIn()
    {
        Console.WriteLine("Enter 1 to view all movies");
        Console.WriteLine("Enter 2 to Add a new movie");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            Console.Clear();
            Movies.ShowAllMovies();
        }
        else if (input == "2")
        {
            Movies.AddNewMovie();
        }
        else
        {
            Console.WriteLine("Invalid input");
            LoggedIn();
        }
    }
}