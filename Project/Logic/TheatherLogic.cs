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
        Console.CursorVisible = false;
        ConsoleKeyInfo key;
        int selectedOption = 0;
        int numRows = theater.Width;
        int numCols = theater.Height;
        List<SeatModel> seats = theater.Seats;

        // Populate the seats list

        List<int> selectedSeats = new List<int>();
        List<SeatModel> selSeat = new List<SeatModel>();

        do
        {
            Console.Clear();
            Console.WriteLine("Use arrow keys to navigate, press Enter to select a seat, and press Space to confirm:\n");

            int count = 0;
            foreach (SeatModel seat in seats)
            {
                if (selectedSeats.Contains(count))
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.Write($"{seat.Row}{seat.Id,-5}");

                Console.ResetColor();

                if ((count + 1) % numCols == 0)
                {
                    Console.WriteLine();
                }

                count++;
            }

            key = Console.ReadKey();

            if (key.Key == ConsoleKey.UpArrow)
            {
                if (selectedSeats.Count == 0)
                {
                    selectedSeats.Add(seats.Count - numCols + selectedSeats.Count);
                }
                else
                {
                    selectedSeats[selectedSeats.Count - 1] -= numCols;
                    if (selectedSeats[selectedSeats.Count - 1] < 0)
                    {
                        selectedSeats[selectedSeats.Count - 1] += numCols * numRows;
                    }
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (selectedSeats.Count == 0)
                {
                    selectedSeats.Add(0);
                }
                else
                {
                    selectedSeats[selectedSeats.Count - 1] += numCols;
                    if (selectedSeats[selectedSeats.Count - 1] >= numCols * numRows)
                    {
                        selectedSeats[selectedSeats.Count - 1] -= numCols * numRows;
                    }
                }
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (selectedSeats.Count > 0)
                {
                    selectedSeats[selectedSeats.Count - 1]--;
                    if (selectedSeats[selectedSeats.Count - 1] < 0)
                    {
                        if (selectedSeats.Count == 1)
                        {
                            selectedSeats[selectedSeats.Count - 1] = seats.Count - 1;
                        }
                        else
                        {
                            selectedSeats.RemoveAt(selectedSeats.Count - 1);
                        }
                    }
                }
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                if (selectedSeats.Count > 0)
                {
                    selectedSeats[selectedSeats.Count - 1]++;
                    if (selectedSeats[selectedSeats.Count - 1] >= numCols * numRows)
                    {
                        if (selectedSeats.Count == 1)
                        {
                            selectedSeats[selectedSeats.Count - 1] = 0;
                        }
                        else
                        {
                            selectedSeats.RemoveAt(selectedSeats.Count - 1);
                        }
                    }
                }
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (!selectedSeats.Contains(selectedSeats.Count))
                {
                    selectedSeats.Add(selectedSeats.Count);
                }
            }
            else if (key.Key == ConsoleKey.Spacebar)
            {
                break;
            }

        } while (true);

        Console.Clear();
        for (int i = 0; i < selectedSeats.Count - 1; i++)
        {
            selSeat.Add(seats[selectedSeats[i]]);
        }

        foreach (var seat in selSeat)
        {
            Console.WriteLine($"{seat.Row}{seat.Id}");
        }
    }
}

