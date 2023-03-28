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



    public void ShowSeats(TheaterModel theater)
    {
        var AllSeats = theater.Seats;

        int i = 1;
        int selectedSeatIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Use arrow keys to navigate and press Enter to select a seat:");
            Console.WriteLine();

            for (int j = 0; j < AllSeats.Count; j++)
            {
                SeatModel seat = AllSeats[j];

                if (j == selectedSeatIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (seat.Reserved)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write($" {seat.Id} ");
                Console.ResetColor();

                if (i == theater.Width)
                {
                    Console.WriteLine();
                    i = 0;
                }
                i += 1;
            }

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
                    if (selectedSeat.Reserved)
                    {
                        Console.WriteLine("Seat is already taken. Press any key to continue.");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        selectedSeat.Reserved = true;
                        Console.WriteLine($"Seat {selectedSeat.Id} has been Selected. Press any key to continue.");
                        Console.ReadKey(true);
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
