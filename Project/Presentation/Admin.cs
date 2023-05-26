using System.Globalization;
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

        Options.Add("Add new Categories");
        Actions.Add(() => Category.NewCatMenu());

        Options.Add("Add new Reservation for user");
        Actions.Add(() => Reservation.FilterMenu());

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
        Actions.Add(() => TimeSlots.WhatMovieTimeSlot(isEdited: true));

        Options.Add("Change Categories");
        Actions.Add(() => Category.Start());

        Options.Add("Change Reservations");
        Actions.Add(() => Reservation.EditReservation(true));

        Options.Add("Change TimeSlots");
        Actions.Add(() => TimeSlots.WhatMovieTimeSlot(isEdited: true));

        Options.Add("Add Cinema Location");
        Actions.Add(() => LocationsLogic.NewLocation());

        Options.Add("Remove Cinema Location");
        Actions.Add(() => LocationsLogic.RemoveLocation());

        Options.Add("\nReturn");
        Actions.Add(() => Menu.Start());

        MenuLogic.Question(Question, Options, Actions);

    }
    public static void LogReportByDate()
    {
        MoviesLogic ML = new();

        DateTime StartDate = new();
        DateTime EndDate = new();

        bool CorrectsDate = false;
        Console.Clear();
        while (!CorrectsDate)
        {
            Console.WriteLine("What is the start date of the period you want a report for? (dd/mm/yyyy): ");
            string StartString = Console.ReadLine();
            try
            {
                StartDate = DateTime.ParseExact(StartString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                CorrectsDate = true;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Wrong date format!");
            }
        }

        bool CorrecteDate = false;
        Console.Clear();
        while (!CorrecteDate)
        {
            Console.WriteLine("What is the end date of the period you want a report for? (dd/mm/yyyy): ");
            string Endstring = Console.ReadLine();
            try
            {
                EndDate = DateTime.ParseExact(Endstring, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                CorrecteDate = true;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Wrong date format!");
            }
        }
        List<List<int>> RLog = Logger.ReportList(StartDate, EndDate);
        string Report = $"In the period from {StartDate.Day} to {EndDate.Day}/{EndDate.Month}/{EndDate.Year} the following happened:";
        foreach (var M in RLog)
        {
            Report += $"\n{ML.GetById(M[0]).Title}:\n{M[1]} reservation(s) have been made\n";
            Report += $"{M[2]} reservation(s) have been updated\n{M[3]} reservation(s) have been removed\n";
        }
        Console.WriteLine(Report);

    }
}