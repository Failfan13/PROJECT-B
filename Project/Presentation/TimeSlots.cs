using System.Globalization;

static class TimeSlots
{

    public static void ShowAllTimeSlotsForMovie(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
        List<TimeSlotModel>? tsms = timeSlotsLogic.GetByMovieId(movieid);
        MoviesLogic ML = new MoviesLogic();
        TheatreLogic TL = new TheatreLogic();

        Console.ReadKey();
        if (tsms?.Count == 0) // Movie exists but there is no timeslot for it
        {
            Console.WriteLine("There are no timeslots for that movie.\nPress Enter to return");
            string? a = Console.ReadLine();
        }
        else
        {
            string Question = $"Availible timeslots for {ML.GetById(movieid)?.Title}";
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
        List<TimeSlotModel>? tsms = timeSlotsLogic.GetByMovieId(movieid);

        Console.Clear();
        if (tsms?.Count == 0) // Movie exists but there is no timeslot for it
        {
            Console.WriteLine("There are no timeslots for that movie.\nPress Enter to return");
            string? a = Console.ReadLine();
        }
        else
        {
            string Question = $"Availible timeslots for {ML.GetById(movieid)?.Title}";
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

    public static void NewTimeSlot(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        MovieModel? movie = ML.GetById(movieid);
        TimeSlotModel TM = new TimeSlotModel();
        TheatreLogic TL = new TheatreLogic();

        TM.MovieId = movie.Id;
        TM.Theatre.Id = TL.GetNewestId();

        Console.Clear();

        TimeSlotStartTime(TM);

        Console.WriteLine("Would you like to change the seat layout? (y/n)");
        if (Console.ReadLine() == "y")
        {
            Theatre.EditMenu(TM.Theatre);
        }

        Console.WriteLine("Would you like to add a new format? (y/n)");
        if (Console.ReadLine() == "y")
        {
            Format.ChangeFormats(TM);
        }

        TimeSlotsLogic.NewTimeSlot(TM.MovieId, TM.Start, TM.Theatre, TM.Format);
    }

    public static void EditTimeSlot(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        List<TimeSlotModel>? tsms = TimeSlotsLogic.GetByMovieId(movieid);
        TimeSlotModel? tsm = null;

        List<string> Options = new List<string>();
        int? awnser;

        awnser = TimeSlots.SelectTimeSlot(movieid, IsEdited);

        if (awnser != null) tsm = tsms?[(int)awnser]; // if SelectTimeSlot returned null

        EditTimeSlotChangeMenu(tsm, IsEdited);
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
        Options.Add("Change Theatre arrangement");
        Actions.Add(() => Theatre.EditMenu(tsm.Theatre, () => EditTimeSlotChangeMenu(tsm)));
        Options.Add("Change view format");
        Actions.Add(() => Format.ChangeFormats(tsm, () => EditTimeSlotChangeMenu(tsm)));

        Options.Add("Return");
        Actions.Add(() => Parallel.Invoke(
            () => TheatreLogic.UpdateToTheatre(tsm),
            () => TimeSlotsLogic.UpdateList(tsm)
        ));

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void TimeSlotStartTime(TimeSlotModel tsm, Action returnTo = null)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        tsm.Start = DateTime.MinValue;

        while (tsm.Start == DateTime.MinValue)
        {
            try
            {
                Console.WriteLine("Enter a new start date: dd/mm/yy");
                string date = Console.ReadLine().Replace(" ", "");

                Console.WriteLine("Enter a new start time: hh:mm");
                string time = Console.ReadLine().Replace(" ", "");

                tsm.Start = DateTime.ParseExact((date + " " + time), "dd/MM/yy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (System.Exception)
            {
                Console.Clear();
                Console.WriteLine("Wrong date/time format, try again");
            }
        }
        TimeSlotsLogic.UpdateList(tsm);
        if (returnTo != null) returnTo();
    }
}