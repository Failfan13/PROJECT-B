static class TimeSlots
{
    static private TimeSlotsLogic timeslotslogic = new TimeSlotsLogic();

    public static void ShowAllTimeSlotsForMovie(int movieid, string moviename, bool IsEdited = false)
    {
        List<TimeSlotModel> tsms = timeslotslogic.GetByMovieId(movieid);
        TheatherLogic TL = new TheatherLogic();

        Console.Clear();
        if (tsms.Count == 0) // Movie exists but there is no timeslot for it
        {
            Console.WriteLine("There are no timeslots for that movie.\nPress Enter to return");
            string a = Console.ReadLine();
        }
        else
        {
            string Question = $"Availible timeslots for {moviename}";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            foreach (TimeSlotModel time in tsms)
            {
                Options.Add($"{time.Start}");
                Actions.Add(() => TL.ShowSeats(TL.GetById(time.Theater),time));
            }

            MenuLogic.Question(Question,Options,Actions);
        }
    }
}