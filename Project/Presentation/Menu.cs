static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        string Question = "Make a choice from the menu\n";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>();

        // Login
        if (AccountsLogic.CurrentAccount == null)
        {
            Options.Add("Login / new account");
            Actions.Add(() => UserLogin.Start());
        }
        else // show account name
        {
            Question = "Welcome back " + AccountsLogic.CurrentAccount.FullName + "\n" + Question;
        }

        // Make reservation
        if (AccountsLogic.CurrentAccount == null || !AccountsLogic.CurrentAccount.Admin)
        {
            Options.Add("Make a Reservation");
            Actions.Add(() => Reservation.FilterMenu());
        }

        // Reservation options
        if (AccountsLogic.CurrentAccount != null && !AccountsLogic.CurrentAccount.Admin)
        {
            Options.Add("See all reservations");
            //Actions.Add(() => Reservation.AllReservations());
            Actions.Add(() => Console.WriteLine("No Work bruh"));

            Options.Add("Change a reservation");
            Actions.Add(() => Reservation.EditReservation());

            Options.Add("Review past reservation");
            Actions.Add(() => Movies.AddReviewMenu());
        }

        // Contact
        if (AccountsLogic.CurrentAccount == null || !AccountsLogic.CurrentAccount.Admin)
        {
            Options.Add("Movie Releases");
            Actions.Add(() => Movies.UpAndComingReleases());
            Options.Add("\nContact");
            Actions.Add(() => Contact.start());
        }

        // Admin data
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            Console.WriteLine($"Admin {AccountsLogic.CurrentAccount.Admin}\n");
            Options.Add("Add data");
            Actions.Add(() => Admin.Start());

            Options.Add("Change data");
            Actions.Add(() => Admin.ChangeData());

            Options.Add("Change user data");
            Actions.Add(() => User.SelectUser());

            Options.Add("View ratings");
            Actions.Add(() => Movies.EditReviewsMenu());
        }

        // Logout & account settings
        if (AccountsLogic.CurrentAccount != null)
        {
            Options.Add("\nLogout");
            Actions.Add(() => UserLogin.Logout());

            Options.Add("Account settings");
            Actions.Add(() => UserLogin.Start());
        }

        Options.Add("\nExit app");
        Actions.Add(() => Environment.Exit(1));

        MenuLogic.Question(Question, Options, Actions);
        Menu.Start();
    }
}