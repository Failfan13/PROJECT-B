public static class Admin
{
    public static void Start()
    {
        string Question = "Make a choice from the menu\n";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>();

        Options.Add("Add new Theatre");
        Actions.Add(() => Theatre.MakeNewTheatre());

        Options.Add("Add new Movie");
        Actions.Add(() => Movies.AddNewMovie());

        Options.Add("Add new Tileslot");
        //Actions.Add(() => TimeSlots.ChangeTimeSlotsMenu());

        Options.Add("Add new Categories");
        //Actions.Add(() => Category.AddNewCategory());

        Options.Add("Add new Reservation for user");
        //Actions.Add(() => Reservation.AddNewReservation());

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
        Actions.Add(() => Theatre.WhatTheatre());

        Options.Add("Change Movies");
        Actions.Add(() => Movies.ChangeMoviesMenu());

        Options.Add("Change Tileslots");
        //Actions.Add(() => TimeSlots.ChangeTimeSlotsMenu());

        Options.Add("Change Categories");
        Actions.Add(() => Category.Start());

        Options.Add("Change Reservations");
        Actions.Add(() => Reservation.EditReservation(true));

        Options.Add("\nReturn");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }
}