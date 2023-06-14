using System.Globalization;

static class TimeSlots
{

    public static void ShowAllTimeSlotsForMovie(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
        List<TimeSlotModel> tsms = timeSlotsLogic.GetTimeslotByMovieId(movieid)!;
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
            string Question = $"Available timeslots for {ML.GetById(movieid)?.Result.Title}";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            foreach (TimeSlotModel time in tsms)
            {
                if (time.Start > DateTime.Now) // Only consider future time slots
                {
                    if (time.Format != "" && time.Format != "standard")
                    {
                        Options.Add($"{time.Start} - Type: {time.Format}");
                        Actions.Add(() => Reservation.FormatPrompt(() => Theatre.SelectSeats(time, IsEdited)));
                    }
                    else
                    {
                        Options.Add($"{time.Start}");
                        Actions.Add(() => Theatre.SelectSeats(time, IsEdited));
                    }
                }
            }

            MenuLogic.Question(Question, Options, Actions);
        }
    }

    public static int? SelectTimeSlot(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        List<TimeSlotModel> tsms = timeSlotsLogic.GetTimeslotByMovieId(movieid)!;

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

    public async static Task NewTimeSlot(int movieId, bool IsEdited = false)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        TheatreLogic TL = new TheatreLogic();
        // make new timeslot
        TimeSlotModel TM = new TimeSlotModel();
        TM = TM.NewTimeSlotModel();
        // Get room to add
        int newTheatreId = Theatre.WhatTheatre();

        TM.MovieId = movieId;
        TM.Theatre.TheatreId = newTheatreId;

        Console.Clear();

        await TimeSlotStartTime(TM, newTimeSlot: true);

        Console.WriteLine("\nWould you like to add a new format? (y/n)");
        if (Console.ReadKey().KeyChar == 'y')
        {
            //TimeSlotsLogic.UpdateList(TM);
            Format.ViewFormatTimeslotMenu(ML.GetById(TM.MovieId)!.Result, TM);
        }

        await TimeSlotsLogic.NewTimeSlot(TM);
    }

    public static void WhatMovieTimeSlot(bool IsEdited = false)
    {
        var movies = new MoviesLogic().AllMovies();

        if (IsEdited)
        {
            movies = new MoviesLogic().AllMovies(true);
        }

        string Question = "which movie would you like to change the timeslots for?";
        List<string> Movies = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (MovieModel movie in movies)
        {
            Movies.Add(movie.Title);
            if (IsEdited) Actions.Add(() => TimeSlots.EditTimeSlot(movie.Id, false));
            else Actions.Add(async () => await TimeSlots.NewTimeSlot(movie.Id).ConfigureAwait(false));
        }

        Movies.Add("Return");
        if (IsEdited) Actions.Add(() => Admin.ChangeData());
        else Actions.Add(() => Admin.Start());

        MenuLogic.Question(Question, Movies, Actions);
    }

    public static void EditTimeSlot(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        List<TimeSlotModel> tsms = TimeSlotsLogic.GetTimeslotByMovieId(movieid)!;
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
        Actions.Add(async () => await NewTimeSlot(movieid, IsEdited).ConfigureAwait(false));

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
        Actions.Add(async () => TimeSlotStartTime(tsm, () => EditTimeSlotChangeMenu(tsm)).Wait());

        Options.Add("Change view format");
        Actions.Add(() => Format.ChangeFormats(tsm, () => EditTimeSlotChangeMenu(tsm)));

        Options.Add("Change maximum seats per reservation");
        Actions.Add(() => ChangeMaxSeats(tsm));

        Options.Add("Return");
        Actions.Add(() => Parallel.Invoke(
            () => TheatreLogic.UpdateList(TheatreLogic.GetById(tsm.Theatre.TheatreId).Result!),
            async () => await TimeSlotsLogic.UpdateList(tsm).ConfigureAwait(false),
            () => WhatMovieTimeSlot()
        ));

        MenuLogic.Question(Question, Options, Actions);
    }

    private async static Task TimeSlotStartTime(TimeSlotModel tsm, Action returnTo = null!, bool newTimeSlot = false)
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
        Console.Clear();
        if (tsm.Start < DateTime.Now)
        {
            Console.WriteLine("This date has passed already");
            TimeSlotStartTime(tsm, returnTo, newTimeSlot);
        }
        QuestionLogic.AskEnter();
        if (!newTimeSlot)
        {
            await TimeSlotsLogic.UpdateList(tsm).ConfigureAwait(false);
        }

        if (returnTo != null) returnTo();
    }
    static public void ChangeMaxSeats(TimeSlotModel tsm)
    {
        TimeSlotsLogic TL = new();
        double max = QuestionLogic.AskNumber("What will be the new maximum bookable seats in 1 reservation?");
        int _max =Convert.ToInt32(max);
        if (_max <= 1)
        {
            _max = 1;

        }
        TL.ChangeMaxSeats(tsm, _max);
        QuestionLogic.AskEnter();
        Admin.ChangeData();
    }
}