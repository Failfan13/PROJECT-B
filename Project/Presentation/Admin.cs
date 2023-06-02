public static class Admin
{
    public static void Start()
    {
        string Question = "Make a choice from the menu\n";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>();

        Options.Add("Add new Movie");
        Actions.Add(() => Movies.AddNewMovie());

        Options.Add("Add new TimeSlot");
        Actions.Add(() => TimeSlots.WhatMovieTimeSlot());

        Options.Add("Add new Theatre");
        Actions.Add(() => Theatre.MakeNewTheatre());

        Options.Add("Add new Snack");
        Actions.Add(() => Snacks.NewSnackMenu());

        Options.Add("Add new Categories");
        Actions.Add(() => Category.NewCatMenu());

        Options.Add("Add new Reservation for user");
        Actions.Add(() => Reservation.FilterMenu());

        Options.Add("Add promotion");
        Actions.Add(() => Promo.AddPromo());

        Options.Add("\nReturn");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void ChangeData()
    {
        if (AccountsLogic.CurrentAccount == null || AccountsLogic.CurrentAccount.Admin == false)
        {
            Menu.Start();
        }

        string Question = "What would you like to do?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Change Theatre");
        Actions.Add(() => Theatre.WhatTheatre(true));

        Options.Add("Change Movies");
        Actions.Add(() => Movies.ChangeMoviesMenu());

        Options.Add("Change TimeSlots");
        Actions.Add(() => TimeSlots.WhatMovieTimeSlot(IsEdited: true));

        Options.Add("Change Snacks");
        Actions.Add(() => Snacks.ChangeSnackMenu());

        Options.Add("Change Categories");
        Actions.Add(() => Category.Start());

        Options.Add("Change Reservations");
        Actions.Add(() => Reservation.EditReservation(true));

        Options.Add("Change Promotions");
        Actions.Add(() => Promo.EditPromoMenu());

        Options.Add("Add Cinema Location");
        Actions.Add(() => LocationsLogic.NewLocation());

        Options.Add("Remove Cinema Location");
        Actions.Add(() => LocationsLogic.RemoveLocation());

        Options.Add("\nReturn");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);

    }
}