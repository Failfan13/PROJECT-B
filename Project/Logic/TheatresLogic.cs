using System;
using System.Text.Json;
using System.Text.RegularExpressions;
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
        var seat = (seatNum % width);
        var row = (Math.Floor((double)(seatNum / width)));
        if (seatNum % width == 0) row -= 1;

        return $"{seat}{letters[(int)row]}";
    }

    public void ShowLegend() // Legend for theatre model
    {
        Console.WriteLine();
        MenuLogic.ColorString("■", ConsoleColor.Green, newLine: false);
        Console.Write(" Selected seat\n");
        MenuLogic.ColorString("■", ConsoleColor.White, newLine: false);
        Console.Write(" Taken seat\n");
        MenuLogic.ColorString("☐", ConsoleColor.White, newLine: false);
        Console.Write(" Available seat\n\n");
        MenuLogic.ColorString("☐", ConsoleColor.Yellow, newLine: false);
        Console.Write(" Standard seat\n");
        MenuLogic.ColorString("☐", ConsoleColor.Cyan, newLine: false);
        Console.Write(" Luxury seat\n");
        MenuLogic.ColorString("☐", ConsoleColor.Blue, newLine: false);
        Console.Write(" Handicap seat\n");
        MenuLogic.ColorString("ꟷ //", ConsoleColor.Black, newLine: false);
        Console.Write(" Walk paths\n");
    }

    public void ShowSeats(TheatreModel theatre, TimeSlotModel timeSlot = null!)
    {
        // load logics
        TimeSlotsLogic TL = new TimeSlotsLogic();

        //load predefined data for model
        List<Tuple<string, int>> pathways = theatre.LayoutSpecs.PathwayIndexes;
        List<int> blockedSeats = theatre.LayoutSpecs.BlockedSeatIndexes;
        List<int> handicaped = theatre.LayoutSpecs.HandiSeatIndexes;
        List<SeatModel> reservedSeats = new List<SeatModel>();

        if (timeSlot != null)
        {
            reservedSeats = timeSlot.Theatre.Seats;
        }

        List<int> selectedSeats = new List<int>();
        int selectedSeat = 1;

        //show seats
        int seatAmount = theatre.Width * theatre.Height;

        bool runMenu = true;
        while (runMenu)
        {
            Console.Clear();
            int heightCounter = 0;
            int widthCounter = 0;

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

            for (int i = 1; i < seatAmount + 1; i++)
            {
                ConsoleColor seatColor = ConsoleColor.White;
                //string seatIcon = "▮";
                string seatIcon = "☐";
                Action selection = null!;
                widthCounter++;

                // show pathways
                for (int j = 0; j < pathways.Count; j++)
                {
                    if (pathways[j].Item1 == "column" && widthCounter == pathways[j].Item2 + 1) // paths in column
                    {
                        MenuLogic.ColorString($"ꟷ", ConsoleColor.Black, newLine: false);
                    }
                    else if (pathways[j].Item1 == "row" && (heightCounter + 96) == pathways[j].Item2 && widthCounter <= 1) // paths in row
                    {
                        MenuLogic.ColorString($"   {new String('/', theatre.Width * 3)}", ConsoleColor.Black, newLine: true);
                    }
                }

                // Adds row letters
                if (i == 1 || (i - 1) % theatre.Width == 0) MenuLogic.ColorString($"{(char)(heightCounter + 65)} ", newLine: false);

                // Coloring seats
                switch (i)
                {
                    case var x when selectedSeat == x: // Curr selected seat
                        selection = () => Parallel.Invoke(
                            () => Console.BackgroundColor = ConsoleColor.Yellow,
                            () => Console.ForegroundColor = ConsoleColor.Black
                        );
                        break;
                    case var x when selectedSeats.Contains(x): // All selected
                        seatColor = ConsoleColor.Green;
                        seatIcon = "▮";
                        break;
                    case var x when reservedSeats.Exists(s => s.Id == x): // Reserved seat
                        if (handicaped.Contains(x))
                        {
                            seatColor = ConsoleColor.Blue;
                            seatIcon = "▮";
                            break;
                        }
                        else
                        {
                            seatColor = ConsoleColor.White;
                            seatIcon = "▮";
                            break;
                        }
                    case var x when blockedSeats.Exists(s => s == x): // Blocked seat
                        seatColor = ConsoleColor.Black;
                        seatIcon = " ";
                        break;
                    case var x when handicaped.Exists(s => s == x): // Handicaped
                        seatColor = ConsoleColor.Blue;
                        seatIcon = "☐";
                        break;
                    // needs standard & luxury
                    default:
                        break;
                }

                Console.Write(" ");
                if (selection != null) // color selection
                    selection();
                // Prints seat icons in color
                MenuLogic.ColorString($"{seatIcon} ", seatColor, newLine: false);

                // new line if theathre width reached
                if (i % theatre.Width == 0)
                {
                    Console.WriteLine();
                    heightCounter++;
                    widthCounter = 0;
                }
            }

            // Adds column numbers
            Console.Write("\n  ");
            for (int i = 1; i < (theatre.Width + 1); i++)
            {
                if (pathways.Any(p => p.Item1 == "column" && p.Item2 + 1 == i)) Console.Write(" "); // spacing numbers pathways
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
            if (timeSlot != null)
            {
                Console.WriteLine($"\n\nSelected Seats: {string.Join(", ", selectedSeats.Select(s => $"{SeatNumber(theatre.Width, s)}"))}");
            }
            else Console.WriteLine(); // clearance

            // Legend
            ShowLegend();

            // Key mappings
            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            if (timeSlot != null)
            {
                switch (keyPressed.Key)
                {
                    // Keys & next available seats
                    case ConsoleKey.LeftArrow: // Left
                        selectedSeat = SelectableSeat(selectedSeat, theatre.Width, seatAmount, 'L', blockedSeats, reservedSeats);
                        break;
                    case ConsoleKey.RightArrow: // Right
                        selectedSeat = SelectableSeat(selectedSeat, theatre.Width, seatAmount, 'R', blockedSeats, reservedSeats);
                        break;
                    case ConsoleKey.UpArrow: // Up
                        selectedSeat = SelectableSeat(selectedSeat, theatre.Width, seatAmount, 'U', blockedSeats, reservedSeats);
                        break;
                    case ConsoleKey.DownArrow: // Down
                        selectedSeat = SelectableSeat(selectedSeat, theatre.Width, seatAmount, 'D', blockedSeats, reservedSeats);
                        break;
                    case ConsoleKey.Enter:
                        if (selectedSeats.Contains(selectedSeat))
                        {
                            selectedSeats.Remove(selectedSeat);
                        }
                        else if (selectedSeats.Count >= 8) // check if more then 9 selected
                        {
                            MaximumSeats();
                        }
                        else if (!selectedSeats.Contains(selectedSeat) && !reservedSeats.Exists(s => s.Id == selectedSeat))
                        {
                            selectedSeats.Add(selectedSeat);
                        }
                        break;
                    case var x when x == ConsoleKey.S || // Save settings and quit
                        x == ConsoleKey.Escape ||
                        x == ConsoleKey.Q:
                        runMenu = false;
                        break;
                    default:
                        break;
                }
            }
            else // for configuring theatre
            {
                switch (keyPressed.Key)
                {
                    // Keys & next available seats
                    case ConsoleKey.LeftArrow: // Left
                        selectedSeat = Math.Clamp(selectedSeat -= 1, 1, seatAmount);
                        break;
                    case ConsoleKey.RightArrow: // Right
                        selectedSeat = Math.Clamp(selectedSeat += 1, 1, seatAmount);
                        break;
                    case ConsoleKey.UpArrow: // Up
                        selectedSeat = Math.Clamp(selectedSeat - theatre.Width, 1, seatAmount);
                        break;
                    case ConsoleKey.DownArrow: // Down
                        selectedSeat = Math.Clamp(selectedSeat + theatre.Width, 1, seatAmount);
                        break;
                    case ConsoleKey.B: // Block seat & Unblock seat
                        if (!blockedSeats.Contains(selectedSeat) &&
                            !reservedSeats.Exists(s => s.Id == selectedSeat)) blockedSeats.Add(selectedSeat); // block
                        else blockedSeats.Remove(selectedSeat); // unblock
                        break;
                    case ConsoleKey.H: // Handicap seat & Unhandicap seat
                        if (!handicaped.Contains(selectedSeat) &&
                            !reservedSeats.Exists(s => s.Id == selectedSeat)) handicaped.Add(selectedSeat); // add handicap
                        else handicaped.Remove(selectedSeat); // remove handicap
                        break;
                    case ConsoleKey.P: // Add pathway -> pathway menu
                        Tuple<string, int> pathway = AddPathway(theatre.Width, theatre.Height);
                        if (pathway != null) pathways.Add(pathway);
                        break;
                    case ConsoleKey.R: // Remove pathway -> pathway menu
                        pathways = RemovePathway(pathways);
                        break;
                    case var x when x == ConsoleKey.S || // Save settings and quit
                        x == ConsoleKey.Escape ||
                        x == ConsoleKey.Q:
                        runMenu = false;
                        break;
                    default:
                        break;
                }
                if (timeSlot != null) MenuLogic.ClearFromTop(13);
                else MenuLogic.ClearLastLines(10);
            }
        }
        //Console.Clear(); tempo removed
        theatre.LayoutSpecs.PathwayIndexes = pathways;
        theatre.LayoutSpecs.BlockedSeatIndexes = blockedSeats;
        theatre.LayoutSpecs.HandiSeatIndexes = handicaped;

        UpdateList(theatre);
        if (timeSlot != null)
        {
            foreach (int seatIndex in selectedSeats)
            {
                switch (seatIndex)
                {
                    case var x when handicaped.Contains(x):
                        timeSlot.Theatre.Seats.Add(new SeatModel(seatIndex, "handicap"));
                        break;
                    default:
                        timeSlot.Theatre.Seats.Add(new SeatModel(seatIndex, "basic"));
                        break;
                }
            }
            TL.UpdateList(timeSlot);
        }
    }

    public Tuple<string, int> AddPathway(int width, int height)
    {
        string input = "";
        Console.Clear();

        Console.WriteLine($"Here you can add your pathways to the theatre!\n\nPlease enter the requested information below");
        MenuLogic.ColorString(new String('˭', 59));

        Console.WriteLine("Index for the pathway");
        MenuLogic.ColorString(">>", newLine: false);
        Console.WriteLine($" Enter a letter or number, row will be placed after input"); // command the user

        while (true)
        {
            input = Console.ReadLine()!.ToLower();

            if (input == null || input == "" || input == "q") return null!;

            if (input.Length < 3 && Int32.TryParse(input, out int x) && Int32.Parse(input) < height) // number input < 3 numbers
            {
                return new Tuple<string, int>("column", Int32.Parse(input));
            }
            else if (input.Length == 1 && Regex.IsMatch(input, $"[a-{(char)(97 + (width - 2))}]", RegexOptions.IgnoreCase)) // letter input
            {
                return new Tuple<string, int>("row", (int)(Convert.ToChar(input)));
            }
            else
            {
                Console.WriteLine("Invalid input, please try again");
                MenuLogic.ClearLastLines(2, true);
            }
        }
    }

    public List<Tuple<string, int>> RemovePathway(List<Tuple<string, int>> listPathways)
    {
        Console.WriteLine($"Here you can remove a pathway from the theatre!\n\nPlease enter the requested information below");
        MenuLogic.ColorString(new String('˭', 59));

        string Question = "Choose one of the following pathways to remove:";
        List<string> Options = new List<string>() { };
        List<Action> Actions = new List<Action>() { };

        foreach (var pathway in listPathways)
        {
            if (pathway.Item1 == "row") Options.Add($"{pathway.Item1} At {((char)pathway.Item2).ToString().ToUpper()}");
            else Options.Add($"{pathway.Item1} At {pathway.Item2}");
            Actions.Add(() => listPathways.Remove(pathway));
        }

        MenuLogic.Question(Question, Options, Actions);

        return listPathways;
    }

    private static int SelectableSeat(int currSelectedSeat, int width, int seatAmount, char direction, List<int> blockedSeats, List<SeatModel> reservedSeats)
    {
        int logicalIndex = currSelectedSeat;
        bool selectable = false;

        while (!selectable && logicalIndex >= 1 && logicalIndex <= seatAmount)
        {
            switch (direction)
            {
                case 'L': // Left
                    if (blockedSeats.Contains(logicalIndex - 1) || reservedSeats.Exists(s => s.Id == logicalIndex - 1)) logicalIndex -= 1;
                    else
                    {
                        logicalIndex--;
                        return Math.Clamp(logicalIndex, 1, seatAmount);
                    }
                    break;
                case 'R': // Right
                    if (blockedSeats.Contains(logicalIndex + 1) || reservedSeats.Exists(s => s.Id == logicalIndex + 1)) logicalIndex += 1;
                    else
                    {
                        logicalIndex++;
                        return Math.Clamp(logicalIndex, 1, seatAmount);
                    }
                    break;
                case 'U': // Up
                    if (blockedSeats.Contains(logicalIndex - width) || reservedSeats.Exists(s => s.Id == logicalIndex - width)) logicalIndex -= width;
                    else
                    {
                        logicalIndex -= width;
                        return Math.Clamp(logicalIndex, 1, seatAmount);
                    }
                    break;
                case 'D': // Down
                    if (blockedSeats.Contains(logicalIndex + width) || reservedSeats.Exists(s => s.Id == logicalIndex + width)) logicalIndex += width;
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

    private string GetSeatTypes(int width, int height, int seatNum)
    {
        List<int> standard = new List<int>() { };
        List<int> luxury = new List<int>() { };
        return null!;
    }

    private void MaximumSeats()
    {
        Console.Clear();
        Console.WriteLine($@"There is a maximum of 9 seats per account.");
        MenuLogic.ColorString(">>", newLine: false);
        Console.WriteLine("Please contact the support team for more information.");
        MenuLogic.ColorString(new String('‗', 59));

        Console.WriteLine("\nEnter any key to continue to contact page or R to return to seat scedule");
        ConsoleKeyInfo inpKey = Console.ReadKey();
        if (inpKey.Key != ConsoleKey.R) Contact.start();
    }
}