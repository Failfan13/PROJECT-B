static class TimeSlots
{
    static private TimeSlotsLogic timeslotslogic = new TimeSlotsLogic();

    public static void WhatMovie()
    {
        Console.WriteLine("What movie do you whant to see the timeslots for?");
        string input = Console.ReadLine();
        MoviesLogic tempmvl = new MoviesLogic();
        foreach(MovieModel mv in tempmvl.AllMovies())
        {
            if (input == mv.Title)
            {
                ShowAllTimeSlotsForMovie(mv.Id);
            }
        }
        Console.WriteLine("Invalid input\nPlease enter a valid movie");
        WhatMovie();
    }

    public static void ShowAllTimeSlotsForMovie(int movieid)
    {
        List<TimeSlotModel> tsms = timeslotslogic.GetByMovieId(movieid);
        Console.Clear();
        foreach (TimeSlotModel tsm in tsms)
        {
            tsm.Info();
        }
        Menu.Start();
    }
}