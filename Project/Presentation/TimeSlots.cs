static class TimeSlots
{

    public static void ShowAllTimeSlotsForMovie(int movieid, bool IsEdited = false)
    {
        TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
        List<TimeSlotModel> tsms = timeSlotsLogic.GetByMovieId(movieid);
        MoviesLogic ML = new MoviesLogic();
        TheatherLogic TL = new TheatherLogic();

        Console.Clear();
        if (tsms.Count == 0) // Movie exists but there is no timeslot for it
        {
            Console.WriteLine("There are no timeslots for that movie.\nPress Enter to return");
            string a = Console.ReadLine();
        }
        else
        {
            string Question = $"Availible timeslots for {ML.GetById(movieid).Title}";
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
        TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
        MoviesLogic ML = new MoviesLogic();
        MovieModel movie = ML.GetById(movieid);
        TheatherLogic TL = new TheatherLogic();
        TheatherModel TM = new TheatherModel();
        TM.MovieId = movie.Id;
        DateTime DT = DateTime.MinValue;

        while (DT == DateTime.MinValue)
        {
            Console.WriteLine("Enter a new time slot: dd/mm/yy hh:mm");
            string time = Console.ReadLine();
            try
            {
                DT = DateTime.ParseExact(time, "dd/MM/yyyy HH:mm", new CultureInfo("nl-NL"));
            }
            catch (System.Exception)
            {
                Console.Clear();
                Console.WriteLine("Wrong date format, try again");
            }
        }
        while (TL == new TheatherLogic())
        {
            Console.WriteLine("Would you like to change the seat layout? (y/n)");
            if (Console.ReadLine() == "y")
            {
                Theater.EditMenu(TL);
            }
            break;
        }

        Console.WriteLine("Would you like to add a new format? (y/n)");
        if (Console.ReadLine() == "y")
        {

        }
    }
}