static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login options");
        Console.WriteLine("Enter 2 add movie");
        Console.WriteLine("Enter 3 add snack");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Movies.ShowAllMovies();
        }
        else if (input == "3")
        {
            Snacks.Start();
            System.Environment.Exit(1);
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }
}