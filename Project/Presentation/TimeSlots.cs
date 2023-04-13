using System.Globalization;

static class TimeSlots
{

    public static void ShowAllTimeSlotsForMovie(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
        List<TimeSlotModel>? tsms = timeSlotsLogic.GetByMovieId(movieid);
        MoviesLogic ML = new MoviesLogic();
        TheatherLogic TL = new TheatherLogic();

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
                Actions.Add(() => Theater.SelectSeats(time));
            }

            MenuLogic.Question(Question, Options, Actions);
        }
    }

    public static void NewTimeSlot(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic TimeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        MovieModel? movie = ML.GetById(movieid);
        TimeSlotModel TM = new TimeSlotModel();
        TheatherLogic TL = new TheatherLogic();

        TM.MovieId = movie.Id;
        TM.Theater.Id = TL.GetNewestId();

        Console.Clear();

        while (TM.Start == new DateTime())
        {
            try
            {
                Console.WriteLine("Enter a new start date: dd/mm/yy");
                string? date = Console.ReadLine()?.Replace(" ", "");

                Console.WriteLine("Enter a new start time: hh:mm");
                string? time = Console.ReadLine()?.Replace(" ", "");

                TM.Start = DateTime.ParseExact((date + " " + time), "dd/MM/yy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (System.Exception)
            {
                Console.Clear();
                Console.WriteLine("Wrong date/time format, try again");
            }
        }

        Console.WriteLine("Would you like to change the seat layout? (y/n)");
        if (Console.ReadLine() == "y")
        {
            Theater.EditMenu(TM.Theater, false);
        }

        Console.WriteLine("Would you like to add a new format? (y/n)");
        if (Console.ReadLine() == "y")
        {
            Format.ChangeFormats(TM);
        }

        TimeSlotsLogic.NewTimeSlot(TM.MovieId, TM.Start, TM.Theater, TM.Format);
    }
}