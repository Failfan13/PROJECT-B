static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        string Question = "Make a choice from the menu by entering the number.\n";
        List<string> Options = new List<string>()
        {
            "Login","Make a Reservation","Contact"
        };
        List<Action> Actions = new List<Action>();
        Actions.Add(() => UserLogin.Start());
        Actions.Add(() => Reservation.NoFilterMenu());
        Actions.Add(() => Contact.start());

        // Only see if logged in
        if (AccountsLogic.CurrentAccount != null)
        {
            Console.WriteLine($"Name:{AccountsLogic.CurrentAccount.FullName}\n");
        }

        // Only see if logged in and admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            Console.WriteLine($"Admin {AccountsLogic.CurrentAccount.Admin}\n");
            Options.Add("Change data");
            Actions.Add(() => Admin.Start());
        }

        Options.Add("\nExit app");
        Actions.Add(() => Environment.Exit(1));

        MenuLogic.Question(Question, Options, Actions);
        Menu.Start();
    }
}