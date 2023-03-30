static class TimeSlots
{
    static private TimeSlotsLogic timeslotslogic = new TimeSlotsLogic();

    public static void WhatMovie()
    {
        Console.WriteLine("What movie do you want to see the timeslots for?");
        string input = Console.ReadLine();
        MoviesLogic tempmvl = new MoviesLogic();
        foreach (MovieModel mv in tempmvl.AllMovies())
        {
            if (input == mv.Title) // Movie exists in database
            {
                ShowAllTimeSlotsForMovie(mv.Id, mv.Title);
            }
        }
        // Below is for when the movie does not exist in the database
        Console.WriteLine("Invalid input\nPlease enter a valid movie");
        WhatMovie();
    }

    public static List<object> ShowAllTimeSlotsForMovie(int movieid, string moviename)
    {
        List<TimeSlotModel> tsms = timeslotslogic.GetByMovieId(movieid);
        TheatherLogic TL = new TheatherLogic();
        Console.Clear();
        if (tsms.Count == 0) // Movie exists but there is no timeslot for it
        {
            Console.WriteLine("There are no timeslots for that movie.\nPress a key to return");
            string a = Console.ReadLine();
            Reservation.Start();
        }
        else
        {
            Console.WriteLine($"Availible timeslots for {moviename}");
            foreach (TimeSlotModel tsm in tsms)
            {
                Console.WriteLine($"{tsm.Id + 1}. {tsm.Start}");
            }
            int awnser = QuestionLogic.AskNumber("What timeslot would you like to see the seats for?") - 1;
            foreach (TimeSlotModel tsm in tsms)
            {
                if (tsm.Id == awnser)
                {

                    /* De function om de stoelen te zien komt hier.
                    TL.ShowSeats(TL.GetById(tsm.Theater));
                    tijdelijk doorsturen:
                    */
                    //ReservationLogic RL = new ReservationLogic();
                    List<SeatModel> Seats = new List<SeatModel>() {
                        new SeatModel(1,'A',10,true,false),
                        new SeatModel(2,'A',10,true,false),
                    };


                    //RL.MakeReservation(tsm.Id, Seats);
                    return new List<object>() { tsm.Id, Seats };
                    // Console.WriteLine("Press enter to return to main menu");
                    // Console.ReadLine();
                    //Menu.Start();
                }
            }
        }
        return null;
    }
}