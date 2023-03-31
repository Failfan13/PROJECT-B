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
    public void MakeTheather(int width, int height)
    {
        List<SeatModel> Seats = new List<SeatModel>();
        List<char> chars = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L' };


        if (height > 0 && height <= chars.Count() && width > 0)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 1; j <= width; j++)
                {
                    Seats.Add(new SeatModel(j, chars[i], 15, false, false));
                }
            }
        }

        int newId = 0;
        try
        {
            newId = GetNewestId();
        }
        catch (System.InvalidOperationException)
        {
            newId = 0;
        }
        TheaterModel theater = new TheaterModel(newId, Seats, width, height);

        UpdateList(theater);
    }



    public void ShowSeats(TheaterModel theater, TimeSlotModel timeSLot, bool IsEdited = false)
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

                Console.Write($" {seat.Row}{seat.Id} ");
                Console.ResetColor();

                if (i == theater.Width)
                {
                    Console.WriteLine();
                    i = 0;
                }
                i += 1;
            }

            Console.WriteLine();
            Console.WriteLine($"Selected Seats: {string.Join(", ", selectedSeats.Select(s => $"{s.Row}{s.Id}"))}");

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
                    else if (selectedSeats.Count < 9 && !selectedSeat.Reserved)
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
                        Console.WriteLine($"You have selected {selectedSeats.Count} seats: {string.Join(", ", selectedSeats.Select(s => $"{s.Row}{s.Id}"))}");
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
                                    if (seat.Id == seats.Id && seat.Row == seats.Row)
                                    {
                                        seat.Reserved = seats.Reserved;
                                    }
                                }
                            }

                            UpdateList(theater);
                            Console.WriteLine($"Selected Seats: {string.Join(", ", selectedSeats.Select(s => $"{s.Row}{s.Id}"))} have been reserved.");
                            string Question = "Would you like to order snacks?";
                            List<string> Options = new List<string>() { "Yes", "No" };
                            List<Action> Actions = new List<Action>();
                            ReservationLogic RL = new ReservationLogic();


                            Actions.Add(() => Snacks.Start());
                            Actions.Add(() => RL.MakeReservation(timeSLot.Id, selectedSeats,IsEdited));


                            MenuLogic.Question(Question, Options, Actions);
                            return;
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
}
