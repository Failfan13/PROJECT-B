public class TheatherLogic
{
    private List<TheaterModel> _theathers;

    public TheatherLogic()
    {
        _theathers = TheaterAccess.LoadAll();
    }

    public void UpdateList(TheaterModel theather)
    {
        //Find if there is already an model with the same id
        int index = _theathers.FindIndex(s => s.Id == theather.Id);

        if (index != -1)
        {
            //update existing model
            _theathers[index] = theather;
        }
        else
        {
            //add new model
            _theathers.Add(theather);
        }
        TheaterAccess.WriteAll(_theathers);

    }

    public TheaterModel? GetById(int id)
    {
        return _theathers.Find(i => i.Id == id);
    }
    public int GetNewestId()
    {
        return (_theathers.OrderByDescending(item => item.Id).First().Id) + 1;
    }
    public void MakeTheather(int width, int height, int OldId = -1)
    {
        List<SeatModel> Seats = new List<SeatModel>();

        for (int i = 0; i < width * height; i++)
        {
            Seats.Add(new SeatModel(i, 10));
        }
        int newId = 0;
        if (OldId == -1)
        {
            try
            {
                newId = GetNewestId();
            }
            catch (System.InvalidOperationException)
            {
                newId = 0;
            }
        }
        else
        {
            newId = OldId;
        }

        TheaterModel theater = new TheaterModel(newId, Seats, width, height);

        UpdateList(theater);
    }
    // Class to store to values from the function below
    public class Helper
    {
        public TheaterModel Theather { get; set; }
        public List<SeatModel> Seats { get; set; }

        public Helper(TheaterModel theater, List<SeatModel> seats)
        {
            Theather = theater;
            Seats = seats;
        }
    }

    // Function to show seats based on a theather model
    // MaxLength is used to limit seat selection
    public Helper ShowSeats(TheaterModel theater, int MaxLength = 1000)
    {
        var AllSeats = theater.Seats;
        List<SeatModel> selectedSeats = new List<SeatModel>();

        int i = 1;
        int selectedSeatIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Use arrow keys to navigate and press Enter to select a seat:\nPress C to confirm and reserve selected seats, R to reset selections and start over:");
            Console.WriteLine();

            for (int j = 0; j < AllSeats.Count; j++)
            {
                SeatModel seat = AllSeats[j];

                if (j == selectedSeatIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (selectedSeats.Contains(seat))
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (seat.Reserved)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write($" {seat.SeatRow(theater.Width)} ");
                Console.ResetColor();

                if (i == theater.Width)
                {
                    Console.WriteLine();
                    i = 0;
                }
                i += 1;
            }

            Console.WriteLine();
            Console.WriteLine($"Selected Seats: {string.Join(", ", selectedSeats.Select(s => $"{s.SeatRow(theater.Width)}"))}");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedSeatIndex = Math.Max(0, selectedSeatIndex - theater.Width);
                    break;
                case ConsoleKey.DownArrow:
                    selectedSeatIndex = Math.Min(AllSeats.Count - 1, selectedSeatIndex + theater.Width);
                    break;
                case ConsoleKey.LeftArrow:
                    selectedSeatIndex = Math.Max(0, selectedSeatIndex - 1);
                    break;
                case ConsoleKey.RightArrow:
                    selectedSeatIndex = Math.Min(AllSeats.Count - 1, selectedSeatIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    SeatModel selectedSeat = AllSeats[selectedSeatIndex];
                    if (selectedSeats.Contains(selectedSeat))
                    {
                        selectedSeats.Remove(selectedSeat);
                    }
                    else if (selectedSeats.Count < MaxLength && !selectedSeat.Reserved)
                    {
                        selectedSeats.Add(selectedSeat);
                    }
                    break;
                case ConsoleKey.R:
                    selectedSeats.Clear();
                    Console.WriteLine("Selection cleared.");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.C:
                    if (selectedSeats.Count == 0)
                    {
                        Console.WriteLine("Please select at least one seat.");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"You have selected {selectedSeats.Count} seats: {string.Join(", ", selectedSeats.Select(s => $"{s.SeatRow(theater.Width)}"))}");
                        Console.WriteLine("Press Y to confirm or any other key to cancel:");

                        ConsoleKeyInfo confirmKeyInfo = Console.ReadKey(true);

                        if (confirmKeyInfo.Key == ConsoleKey.Y)
                        {
                            foreach (var seat in selectedSeats)
                            {
                                seat.Reserved = true;
                            }
                            foreach (SeatModel seat in theater.Seats)
                            {
                                foreach (var seats in selectedSeats)
                                {
                                    if (seat.Id == seats.Id)
                                    {
                                        seat.Reserved = seats.Reserved;
                                    }
                                }
                            }
                            return new Helper(theater, selectedSeats);
                        }
                        else
                        {
                            selectedSeats.Clear();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void SelectSeats(TimeSlotModel timeSLot, bool IsEdited = false)
    {
        var theater = timeSLot.Theater;
        var help = ShowSeats(theater, 9);
        var selectedSeats = help.Seats;

        new TimeSlotsLogic().UpdateList(timeSLot);
        string Question = "Would you like to order snacks?";
        List<string> Options = new List<string>() { "Yes", "No" };
        List<Action> Actions = new List<Action>();
        ReservationLogic RL = new ReservationLogic();


        Actions.Add(() => Snacks.Start(timeSLot.Id, selectedSeats, IsEdited));
        Actions.Add(() => RL.MakeReservation(timeSLot.Id, selectedSeats, IsEdited: IsEdited));

        MenuLogic.Question(Question, Options, Actions);
    }

    public void BlockSeats(TheaterModel theater)
    {
        var Help = ShowSeats(theater);
        UpdateList(Help.Theather);
    }

    public void ChangeTheaterSize(TheaterModel theater, int width, int height)
    {
        MakeTheather(width, height, theater.Id);
    }
}
