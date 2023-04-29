using System;
using System.Text.Json;
using System.Data;
public class TheatreLogic
{
    private List<TheatreModel> _theatres;

    public TheatreLogic()
    {
        _theatres = TheatreAccess.LoadAll();
    }

    public void AddTheatre(TheatreModel theatre)
    {
        // update if exists else add
        if (ExistingId(theatre.Id)) _theatres[_theatres.FindIndex(i => i.Id == theatre.Id)] = theatre;
        else _theatres.Add(theatre);
        UpdateList(theatre);
    }

    public void RemoveTheatre(int id)
    {
        _theatres.RemoveAll(t => t.Id == id);
        UpdateList();
    }

    public void UpdateList() => this.UpdateList(null!);
    public void UpdateList(TheatreModel theatre)
    {
        if (theatre != null)
        {
            //Find if there is already an model with the same id
            int index = _theatres.FindIndex(s => s.Id == theatre.Id);

            if (index != -1)
            {
                //update existing model
                _theatres[index] = theatre;
                Logger.LogDataChange<TheatreModel>(theatre.Id, "Updated");
            }
            else
            {
                //add new model
                _theatres.Add(theatre);
                Logger.LogDataChange<TheatreModel>(theatre.Id, "Added");
            }
        }
        TheatreAccess.WriteAll(_theatres);
    }

    public TheatreModel? GetById(int id)
    {
        return _theatres.Find(i => i.Id == id);
    }

    public bool ExistingId(int id) => AllTheatres().Exists(i => i.Id == id);

    public int GetNewestId()
    {
        return (_theatres.OrderByDescending(item => item.Id).First().Id) + 1;
    }

    public List<TheatreModel> AllTheatres()
    {
        return _theatres;
    }

    public TheatreModel MakeTheatre(int width, int height, double outSeatPrice, double midSeatPrice = 0, double innSeatPrice = 0, int OldId = -1)
    {
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

        TheatreModel theatre = new TheatreModel(newId, outSeatPrice, width, height);

        if (midSeatPrice != 0)
        {
            theatre.StandardSeatPrice = midSeatPrice;
        }
        if (innSeatPrice != 0)
        {
            theatre.LuxurySeatPrice = innSeatPrice;
        }

        AddTheatre(theatre);

        if (_theatres.Exists(i => i.Id == theatre.Id))
        {
            return _theatres.Find(i => i.Id == theatre.Id)!;
        }
        else
        {
            return theatre;
        }
    }

    public string SeatNumber(int width, int seatNum)
    {
        List<char> letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        var row = (Math.Floor((double)(seatNum / width)));
        if (seatNum % width == 0) row -= 1;

        return $"{seatNum}{letters[(int)row]}";
    }

    public void ShowLegend() // Legend for theatre model
    {
        Console.WriteLine();
        MenuLogic.ColorString("☐", ConsoleColor.Black, newLine: false);
        Console.Write(" Not reservable\n");
        MenuLogic.ColorString("■", ConsoleColor.Green, newLine: false);
        Console.Write(" Selected seat\n");
        MenuLogic.ColorString("■", ConsoleColor.Red, newLine: false);
        Console.Write(" Taken seat\n");
        MenuLogic.ColorString("■", ConsoleColor.White, newLine: false);
        Console.Write(" Basic seat\n");
        MenuLogic.ColorString("■", ConsoleColor.DarkYellow, newLine: false);
        Console.Write(" Standard seat\n");
        MenuLogic.ColorString("■", ConsoleColor.Magenta, newLine: false);
        Console.Write(" Luxury seat\n");
        MenuLogic.ColorString("■", ConsoleColor.Blue, newLine: false);
        Console.Write(" Handicap seat\n");
    }

    public void ShowSeats(TheatreModel theatre, TimeSlotModel timeSlot = null!, bool select = false)
    {
        Console.Clear();
        //load predefined data for model
        List<int> pathways = theatre.LayoutSpecs.PathwayIndexes;
        List<int> blockedSeats = theatre.LayoutSpecs.BlockedSeatIndexes;
        List<int> handicaped = theatre.LayoutSpecs.HandiSeatIndexes;
        List<SeatModel> reservedSeats = timeSlot.Theatre.Seats;

        MenuLogic.ClearLastLines(12);
        Console.Write(@$"In the following screen you can change the seat configuration
Press the following buttons to apply changes:
         
Press [ ");
        MenuLogic.ColorString("↑ → ↓ ←", newLine: false);
        Console.Write(" ] Keys to move around the menu\r\nPress [ ");
        MenuLogic.ColorString("Enter", newLine: false);
        Console.Write(" ] Key to select a seat\r\nPress [ ");
        MenuLogic.ColorString("B", newLine: false);
        Console.Write(" ] Key to block seat position\r\nPress [ ");
        MenuLogic.ColorString("A", newLine: false);
        Console.Write(" ] Key to unblock seat position\r\nPress [ ");
        MenuLogic.ColorString("P", newLine: false);
        Console.Write(" ] Key to add pathway\r\nPress [ ");
        MenuLogic.ColorString("R", newLine: false);
        Console.Write(" ] Key to remove pathway\r\nPress [ ");
        MenuLogic.ColorString("S", newLine: false);
        Console.Write(" ] Key to save\r\n");
        MenuLogic.ColorString(new String('‗', 59));

        List<int> selectedSeats = new List<int>();
        int selectedSeat = 1;

        // screen position
        Console.Write("   ");
        Console.Write($"{new String('▁', (int)theatre.Width / 3)}");
        Console.Write($"{new String('▂', (int)theatre.Width / 2)}");
        Console.Write($"{new String('▃', (int)theatre.Width / 2)}");
        Console.Write($"{new String('▅', (int)theatre.Width / 3)}");
        Console.Write($"{new String('▃', (int)theatre.Width / 2)}");
        Console.Write($"{new String('▂', (int)theatre.Width / 2)}");
        Console.WriteLine($"{new String('▁', (int)theatre.Width / 3)}");
        MenuLogic.ColorString($"{new String(' ', (int)(theatre.Width * 1.45))}SCREEN\n");

        //show seats
        int seatAmount = theatre.Width * theatre.Height;

        while (true)
        {
            int charIndex = 0; // letter indexing

            for (int i = 1; i < seatAmount + 1; i++)
            {
                ConsoleColor seatColor = ConsoleColor.White;
                string seatIcon = "▮";

                // Adds row letters
                if (i == 1 || (i - 1) % theatre.Width == 0) MenuLogic.ColorString($"{(char)(charIndex + 65)} ", newLine: false);

                // Coloring seats
                switch (i)
                {
                    case var x when selectedSeat == x: // Curr selected seat
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case var x when reservedSeats.Exists(s => s.Id == x): // Reserved seat
                        seatColor = ConsoleColor.Red;
                        break;
                    case var x when blockedSeats.Exists(s => s == x): // Blocked seat
                        seatColor = ConsoleColor.Black;
                        seatIcon = "☐";
                        break;
                    case var x when handicaped.Exists(s => s == x): // Handicaped
                        seatColor = ConsoleColor.Blue;
                        break;
                    case var x when selectedSeats.Contains(x): // All selected
                        seatColor = ConsoleColor.Green;
                        break;
                    // needs standard & luxury
                    default:
                        break;
                }

                // Prints seat icons in color
                MenuLogic.ColorString($" {seatIcon} ", seatColor, newLine: false);

                // new line if theathre width reached
                if (i % theatre.Width == 0)
                {
                    Console.WriteLine();
                    charIndex++;
                }
            }

            // Adds column numbers
            Console.Write("\n  ");
            for (int i = 1; i < (theatre.Width + 1); i++)
            {
                if (i.ToString().Length == 1)
                {
                    MenuLogic.ColorString($" {i} ", newLine: false);
                }
                else
                {
                    MenuLogic.ColorString($" {i}", newLine: false);
                }
            }

            // Show selected numbers seats & price total
            Console.WriteLine($"\n\nSelected Seats: {string.Join(", ", selectedSeats.Select(s => $"{SeatNumber(theatre.Width, s)}"))}");

            // Legend
            ShowLegend();

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            if (select)
            {

            }
            else // for configuring theatre
            {
                switch (keyPressed.Key)
                {
                    // Keys & next available seats
                    case ConsoleKey.LeftArrow:
                        selectedSeat = SelectableSeat(selectedSeat, theatre.Width, seatAmount, 'L', blockedSeats);
                        break;
                    case ConsoleKey.RightArrow:
                        selectedSeat = SelectableSeat(selectedSeat, theatre.Width, seatAmount, 'R', blockedSeats);
                        break;
                    case ConsoleKey.UpArrow:
                        selectedSeat = SelectableSeat(selectedSeat, theatre.Width, seatAmount, 'U', blockedSeats);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedSeat = SelectableSeat(selectedSeat, theatre.Width, seatAmount, 'D', blockedSeats);
                        break;
                    case ConsoleKey.Enter:
                        if (selectedSeats.Count >= 8) // check if more then 9 selected
                        {
                            // forward to 9 or more seats
                            return;
                        }
                        else if (selectedSeats.Contains(selectedSeat))
                        {
                            selectedSeats.Remove(selectedSeat);
                        }
                        else if (!selectedSeats.Contains(selectedSeat))
                        {
                            selectedSeats.Add(selectedSeat);
                        }
                        break;
                    case ConsoleKey.B: // Block seat
                        if (!blockedSeats.Contains(selectedSeat)) blockedSeats.Add(selectedSeat);
                        break;
                    case ConsoleKey.A: // Unblock seat
                        blockedSeats.Remove(selectedSeat);
                        break;
                    case ConsoleKey.P: // Add pathway -> pathway menu
                        break;
                    case ConsoleKey.R: // Remove pathway -> pathway menu
                        break;
                    case var x when x == ConsoleKey.S || // Save settings and quit
                        x == ConsoleKey.Escape ||
                        x == ConsoleKey.Q:
                        return;
                    default:
                        break;
                }
            }

            MenuLogic.ClearFromTop(14); // clears console after lines
        }
        // TODO
        // - Add color standard & luxury seats by indexing nums
        // - Make sure data selected preserved and updated

    }

    private static int SelectableSeat(int currSelectedSeat, int width, int seatAmount, char direction, List<int> blockedSeats)
    {
        int logicalIndex = currSelectedSeat;
        bool selectable = false;

        while (!selectable && logicalIndex >= 1 && logicalIndex <= seatAmount)
        {
            switch (direction)
            {
                case 'L': // Left
                    if (blockedSeats.Contains(logicalIndex - 1))
                    {
                        logicalIndex -= 1;
                    }
                    else
                    {
                        logicalIndex--;
                        return Math.Clamp(logicalIndex, 1, seatAmount);
                    }
                    break;
                case 'R': // Right
                    if (blockedSeats.Contains(logicalIndex + 1))
                    {
                        logicalIndex += 1;
                    }
                    else
                    {
                        logicalIndex++;
                        return Math.Clamp(logicalIndex, 1, seatAmount);
                    }
                    break;
                case 'U': // Up
                    if (blockedSeats.Contains(logicalIndex - width))
                    {
                        logicalIndex -= width;
                    }
                    else
                    {
                        logicalIndex -= width;
                        return Math.Clamp(logicalIndex, 1, seatAmount);
                    }
                    break;
                case 'D': // Down
                    if (blockedSeats.Contains(logicalIndex + width))
                    {
                        logicalIndex += width;
                    }
                    else
                    {
                        logicalIndex += width;
                        return Math.Clamp(logicalIndex, 1, seatAmount);
                    }
                    break;
                default:
                    break;
            }
        }
        return logicalIndex;
    }
}