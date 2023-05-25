using System.Globalization;

static class TimeSlots
{

    public static void ShowAllTimeSlotsForMovie(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
        List<TimeSlotModel> tsms = timeSlotsLogic.GetByMovieId(movieid)!;
        MoviesLogic ML = new MoviesLogic();
        TheatreLogic TL = new TheatreLogic();

        if (Filter.AppliedFilters != null && Filter.AppliedFilters.ReleaseDate != null)
        {
            tsms = tsms.FindAll(d => d.Start.Date == Filter.AppliedFilters.ReleaseDate.Date);
        }


        Console.Clear();
        if (tsms.Count == 0) // Movie exists but there is no timeslot for it
        {
            Console.WriteLine("There are no timeslots for that movie.\nPress Enter to return");
            string? a = Console.ReadLine();
        }
        else
        {
            string Question = $"Availible timeslots for {ML.GetById(movieid)?.Result.Title}";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            foreach (TimeSlotModel time in tsms)
            {
                if (time.Format != "" && time.Format != "standard")
                {
                    Options.Add($"{time.Start} -Type : {time.Format}");
                    Actions.Add(() => Reservation.FormatPrompt(() => Theatre.SelectSeats(time, IsEdited)));
                }
                else
                {
                    Options.Add($"{time.Start}");
                    Actions.Add(() => Theatre.SelectSeats(time, IsEdited));
                }
            }

            MenuLogic.Question(Question, Options, Actions);
        }
    }

    public static int? SelectTimeSlot(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        List<TimeSlotModel> tsms = timeSlotsLogic.GetByMovieId(movieid)!;

        Console.Clear();
        if (tsms.Count == 0) // Movie exists but there is no timeslot for it
        {
            Console.WriteLine("There are no timeslots for that movie.\nPress Enter to return");
            string? a = Console.ReadLine();
        }
        else
        {
            string Question = $"Availible timeslots for {ML.GetById(movieid)?.Result.Title}";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            foreach (TimeSlotModel time in tsms)
            {
                Options.Add($"{time.Start}");
                Actions.Add(() => Theatre.SelectSeats(time));
            }

            return MenuLogic.Question(Question, Options);
        }
        return null;
    }

    public static void NewTimeSlot(int movieId, bool IsEdited = false)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        TheatreLogic TL = new TheatreLogic();

        // make new timeslot
        TimeSlotModel TM = new TimeSlotModel(TimeSlotsLogic.GetNewestId());
        // Get room to add
        int newTheatreId = Theatre.WhatTheatre();

        TM.MovieId = movieId;
        TM.Theatre.TheatreId = newTheatreId;

        Console.Clear();

        TimeSlotStartTime(TM, newTimeSlot: true);
        Console.WriteLine("Would you like to change the seat layout? (y/n)");
        if (Console.ReadKey().KeyChar == 'y')
        {
            TM.Theatre.TheatreId = TL.DupeTheatreToNew(newTheatreId);
            if (TL.AllTheatres().Any(t => t.Id == TM.Theatre.TheatreId))
            {
                TL.ShowSeats(TL.GetById(TM.Theatre.TheatreId)!);
            }
        }

        Console.WriteLine("\nWould you like to add a new format? (y/n)");
        if (Console.ReadKey().KeyChar == 'y')
        {
            TimeSlotsLogic.UpdateList(TM);
            Format.ViewFormatMenu(ML.GetById(TM.MovieId)!.Result, TM);
        }

        TimeSlotsLogic.UpdateList(TM);
    }

    public static void WhatMovieTimeSlot(bool isEdited = false)
    {
        var movies = new MoviesLogic().AllMovies();

        if (isEdited)
        {
            movies = new MoviesLogic().AllMovies(true);
        }

        string Question = "which movie would you like to change the timeslots for?";
        List<string> Movies = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (MovieModel movie in movies)
        {
            Movies.Add(movie.Title);
            if (isEdited) Actions.Add(() => TimeSlots.EditTimeSlot(movie.Id, false));
            else Actions.Add(() => TimeSlots.NewTimeSlot(movie.Id));
        }

        Movies.Add("Return");
        Actions.Add(() => Admin.Start());

        MenuLogic.Question(Question, Movies, Actions);
    }

    public static void EditTimeSlot(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        List<TimeSlotModel> tsms = TimeSlotsLogic.GetByMovieId(movieid)!;
        TimeSlotModel tsm = null!;

        string Question = "What TimeSlot do you want to edit?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (TimeSlotModel t in tsms)
        {
            Options.Add($"{t.Start}");
            Actions.Add(() => EditTimeSlotChangeMenu(t, IsEdited));
        }

        Options.Add("Add new TimeSlot");
        Actions.Add(() => NewTimeSlot(movieid, IsEdited));

        Options.Add("Return");
        Actions.Add(() => WhatMovieTimeSlot());

        MenuLogic.Question(Question, Options, Actions);

    }

    public static void EditTimeSlotChangeMenu(TimeSlotModel tsm, bool IsEdited = false)
    {
        TheatreLogic TheatreLogic = new TheatreLogic();
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();

        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Change start time");
        Actions.Add(() => TimeSlotStartTime(tsm, () => EditTimeSlotChangeMenu(tsm)));
        // Add remove seat from theatre & reservation
        // Add seat to theatre & reservation
        Options.Add("Change view format");
        Actions.Add(() => Format.ChangeFormats(tsm, () => EditTimeSlotChangeMenu(tsm)));

        Options.Add("Return");
        Actions.Add(() => Parallel.Invoke(
            //() => TheatreLogic.UpdateList(TheatreLogic.GetById(tsm.Theatre.TheatreId)!),
            () => TimeSlotsLogic.UpdateList(tsm)
        ));

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void TimeSlotStartTime(TimeSlotModel tsm, Action returnTo = null!, bool newTimeSlot = false)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        tsm.Start = DateTime.MinValue;

        while (tsm.Start == DateTime.MinValue)
        {
            try
            {
                Console.WriteLine("Enter a new start date: dd/mm/yy");
                string date = Console.ReadLine()!.Replace(" ", "");

                Console.WriteLine("Enter a new start time: hh:mm");
                string time = Console.ReadLine()!.Replace(" ", "");

                tsm.Start = DateTime.ParseExact((date + " " + time), "dd/MM/yy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (System.Exception)
            {
                Console.Clear();
                Console.WriteLine("Wrong date/time format, try again");
            }
        }

        if (!newTimeSlot)
        {
            TimeSlotsLogic.UpdateList(tsm);
        }

        if (returnTo != null) returnTo();
    }
}