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
            if (input == mv.Title) // Movie exists in database
            {
                ShowAllTimeSlotsForMovie(mv.Id,mv.Title);
            }
        }
        // Below is for when the movie does not exist in the database
        Console.WriteLine("Invalid input\nPlease enter a valid movie");
        WhatMovie();
    }

    public static void ShowAllTimeSlotsForMovie(int movieid, string moviename)
    {
        List<TimeSlotModel> tsms = timeslotslogic.GetByMovieId(movieid);
        Console.Clear();
        if (tsms.Count == 0) // Movie exists but there is no timeslot for it
        {
            Console.WriteLine("There are no timeslots for that movie");
            int milliseconds = 1500;
            Thread.Sleep(milliseconds);
            Menu.Start();
        }
        Console.WriteLine($"Availible timeslots for {moviename}");
        foreach (TimeSlotModel tsm in tsms)
        {
            Console.WriteLine($"{tsm.Id}. {tsm.Start}");
        }
    }
}