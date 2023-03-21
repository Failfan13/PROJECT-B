static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.Clear();
        Console.WriteLine("WELCOME MESSAGE\n");
        Console.WriteLine("Make a choice from the menu by entering the number.\n");

        Console.WriteLine("Main Menu:");
        Console.WriteLine("1\tLogin");
        Console.WriteLine("2\tMake a Reservation");
        Console.WriteLine("3\tContact");
        Console.WriteLine("4\tExit app");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Movies.AddNewMovie();
        }
        else if (input == "3")
        {
            Contact.start();
        }
        else if (input == "4")
        {
            Environment.Exit(1);
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }
}