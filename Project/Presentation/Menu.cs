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

        if (AccountsLogic.CurrentAccount == null)
        {
            Options.Add("Login");
            Actions.Add(() => UserLogin.Login());
        }

        Options.Add("Make a Reservation");
        Actions.Add(() => Reservation.FilterMenu());

        if (AccountsLogic.CurrentAccount != null)
        {
            Options.Add("Change a reservation");
            Actions.Add(() => Reservation.EditReservation());

            if (AccountsLogic.CurrentAccount.Admin == false)
            {
                Options.Add("Add review for past reservation");
                Actions.Add(() => Movies.AddReviewMenu());
            }

            // Options.Add("See all reservations");
            // Actions.Add(() => Reservation.AllReservations());

            Options.Add("Account settings");
            Actions.Add(() => UserLogin.Start());
        }

        Options.Add("Contact");
        Actions.Add(() => Contact.start());

        if (AccountsLogic.CurrentAccount != null)
        {
            Options.Add("\nLogout");
            Actions.Add(() => UserLogin.Logout());
        }

        // Movies.AddNewMovie();



        // // adding movies to check new functions
        // List<CategoryModel> genres = new List<CategoryModel>();
        // DateTime start = new DateTime(2000, 1, 1);
        // MovieModel movie = new MovieModel(0, "Batman", start, "geronimo", "Batman and the joker", 69, 20, genres);


        // foreach (MovieModel film in movie.AllMovies()



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

            Options.Add("Change user data");
            Actions.Add(() => User.SelectUser());

            Options.Add("View ratings");
            Actions.Add(() => Movies.EditReviewsMenu());
        }

        Options.Add("\nExit app");
        Actions.Add(() => Environment.Exit(1));

        MenuLogic.Question(Question, Options, Actions);
        Menu.Start();
    }
}