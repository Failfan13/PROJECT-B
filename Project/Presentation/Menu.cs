static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        if (AccountsLogic.CurrentAccount == null)
        {
            // show not logged in menu
            NoAccount();
        }
        else if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == false)
        {
            NormalAccount();
        }
        else if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            // show admin menu
            AdminAccount();
        }
    }

    public static void NoAccount()
    {
        Console.Clear();
        Console.WriteLine("WELCOME MESSAGE\n");
        Console.WriteLine("Make a choice from the menu by entering the number.\n");

        Console.WriteLine("Main Menu:");
        Console.WriteLine("1 Login");
        Console.WriteLine("2 Make a Reservation");
        Console.WriteLine("3 Contact");
        Console.WriteLine("4 Exit app");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Reservation.start();
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

    public static void NormalAccount()
    {
        Console.Clear();

        Console.WriteLine($"WELCOME MESSAGE \nName:{AccountsLogic.CurrentAccount.FullName}\n");
        Console.WriteLine($"Admin {AccountsLogic.CurrentAccount.Admin}\n");
        Console.WriteLine("Make a choice from the menu by entering the number.\n");

        Console.WriteLine("Main Menu:");
        Console.WriteLine("1 Login");
        Console.WriteLine("2 Make a Reservation");
        Console.WriteLine("3 Contact");
        Console.WriteLine("4 Exit app");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Reservation.start();
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

    public static void AdminAccount()
    {
        Console.Clear();

        Console.WriteLine($"WELCOME MESSAGE \nName:{AccountsLogic.CurrentAccount.FullName}\n");
        Console.WriteLine($"Admin {AccountsLogic.CurrentAccount.Admin}\n");
        Console.WriteLine("Make a choice from the menu by entering the number.\n");

        Console.WriteLine("Main Menu:");
        Console.WriteLine("1 Login");
        Console.WriteLine("2 Make a Reservation");
        Console.WriteLine("3 Contact");
        Console.WriteLine("4 Change movie");
        Console.WriteLine("5 Exit app");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Reservation.start();
        }
        else if (input == "3")
        {
            Contact.start();
        }       
         else if (input == "4")
        {
            Movies.ChangeCategory();
        }
        else if (input == "5")
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