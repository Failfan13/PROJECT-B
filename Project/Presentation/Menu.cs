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

        string Question = "Make a choice from the menu by entering the number.\n";
        List<string> Options = new List<string>()
        {
            "Login","Make a Reservation","Contact","Exit app"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => UserLogin.Start());
        Actions.Add(() => Reservation.NoFilterMenu());
        Actions.Add(() => Contact.start());
        Actions.Add(() => Environment.Exit(1));

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void NormalAccount()
    {
        Console.Clear();

        Console.WriteLine($"WELCOME MESSAGE \nName:{AccountsLogic.CurrentAccount.FullName}\n");
        Console.WriteLine($"Admin {AccountsLogic.CurrentAccount.Admin}\n");
        string Question = "Make a choice from the menu by entering the number.\n";
        List<string> Options = new List<string>()
        {
            "Login","Make a Reservation","Contact","Exit app"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => UserLogin.Start());
        Actions.Add(() => Reservation.NoFilterMenu());
        Actions.Add(() => Contact.start());
        Actions.Add(() => Environment.Exit(1));

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void AdminAccount()
    {
        Console.Clear();

        Console.WriteLine($"WELCOME MESSAGE \nName:{AccountsLogic.CurrentAccount.FullName}\n");
        Console.WriteLine($"Admin {AccountsLogic.CurrentAccount.Admin}\n");
        string Question = "Make a choice from the menu by entering the number.\n";
        List<string> Options = new List<string>()
        {
            "Login","Make a Reservation","Change movie data","Exit app"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => UserLogin.Start());
        Actions.Add(() => Reservation.NoFilterMenu());
        Actions.Add(() => Movies.ChangeMoviesMenu());
        Actions.Add(() => Environment.Exit(1));

        MenuLogic.Question(Question, Options, Actions);
    }
}