public static class Admin
{
    public static void Start()
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

        // Options.Add("Change Tileslots");
        // Actions.Add(() => TimeSlots.ChangeTimeSlotsMenu());

        Options.Add("Change Categories");
        Actions.Add(() => Category.Start());

        Options.Add("Change Reservations");
        Actions.Add(() => Reservation.EditReservation(true));

        Options.Add("Change TimeSlots");
        Actions.Add(() => TimeSlots.WhatMovieEditTimeSlot());

        Options.Add("\nReturn");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);
    }

    // public static void View()
    // {
    //     if (AccountsLogic.CurrentAccount == null || AccountsLogic.CurrentAccount.Admin == false)
    //     {
    //         Menu.Start();
    //     }

    //     string Question = "What would you like to do?";
    //     List<string> Options = new List<string>();
    //     List<Action> Actions = new List<Action>();

    //     Options.Add("View users");
    //     Actions.Add(() => User.SelectUser());


    //     MenuLogic.Question(Question, Options, Actions);

    // }


}