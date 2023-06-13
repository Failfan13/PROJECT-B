using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Data;
public class TheatreLogic
{
    public async Task<List<TheatreModel>> GetAllTheatres()
    {
        return await DbLogic.GetAll<TheatreModel>();
    }
    // pass model to update
    public async Task UpdateList(TheatreModel theatre)
    {
        await DbLogic.UpdateItem(theatre);
    }

    public async Task UpsertList(TheatreModel theatre)
    {
        await DbLogic.UpsertItem(theatre);
    }

    public async Task<TheatreModel>? GetById(int id)
    {
        return await DbLogic.GetById<TheatreModel>(id);
    }

    public async Task<TheatreModel> NewTheatre(TheatreModel theatre)
    {
        await DbLogic.InsertItem<TheatreModel>(theatre);
        return theatre;
    }

    public async Task<TheatreModel> NewTheatre(int width, int height, double SeatPrice)
    {
        TheatreModel TheatreModel = new TheatreModel();
        TheatreModel = TheatreModel.NewTheatreModel(SeatPrice, width, height);
        await DbLogic.UpsertItem<TheatreModel>(TheatreModel);
        return TheatreModel;
    }
    public async Task<TheatreModel> NewTheatre(int width, int height, double SeatPrice, double MidleSeatPrice, double OutSeatPrice)
    {
        TheatreModel theatreModel = new TheatreModel();
        theatreModel = theatreModel.NewTheatreModel(SeatPrice, width, height);
        theatreModel.SeatPrices.Standard = MidleSeatPrice;
        theatreModel.SeatPrices.Luxury = OutSeatPrice;
        return await DbLogic.InsertItem<TheatreModel>(theatreModel);
    }

    public async Task UpdateTheatre(TheatreModel theatre) //Adds or changes category to list of categories
    {
        await UpdateList(theatre);
    }

    public void DeleteTheatre(int theatreInt) // Deletes category from list of categories
    {
        // account exists and is admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            DbLogic.RemoveItemById<TheatreModel>(theatreInt);
        }
    }

    public string SeatNumber(int width, int seatNum)
    {
        List<char> letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        var seat = (seatNum % width);
        var row = (Math.Floor((double)(seatNum / width)));
        if (seatNum % width == 0) row -= 1;

        if (seat == 0)
        {
            return $"{width}{letters[(int)row]}";
        }
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
        MenuLogic.ColorString("☐", ConsoleColor.Blue, newLine: false);
        Console.Write(" Luxury seat\n");
        MenuLogic.ColorString("☐", ConsoleColor.Magenta, newLine: false);
        Console.Write(" Handicap seat\n");
        MenuLogic.ColorString("ꟷ //", ConsoleColor.Black, newLine: false);
        Console.Write(" Walk paths\n");
    }

    public void ShowControls(bool configure = false)
    {
        Console.Write(@$"In the following screen you can change the seat configuration
Press the following buttons to apply changes:
         
Press [ ");
        if (configure)
        {

            MenuLogic.ColorString("↑ → ↓ ←", newLine: false);
            Console.Write(" ] Keys to move around the menu\r\nPress [ ");
            MenuLogic.ColorString("B", newLine: false);
            Console.Write(" ] Key to block or unblock seatr\nPress [ ");
            MenuLogic.ColorString("H", newLine: false);
            Console.Write(" ] Key to add or remove handicap seat\r\nPress [ ");
            MenuLogic.ColorString("P", newLine: false);
            Console.Write(" ] Key to add pathway\r\nPress [ ");
            MenuLogic.ColorString("R", newLine: false);
            Console.Write(" ] Key to remove pathway\r\nPress [ ");
            MenuLogic.ColorString("S", newLine: false);
            Console.Write(" ] Key to save\r\n");
            MenuLogic.ColorString(new String('‗', 59));
        }
        else
        {
            
            MenuLogic.ColorString("↑ → ↓ ←", newLine: false);
            Console.Write(" ] Keys to move around the menu\r\nPress [ ");
            MenuLogic.ColorString("Enter", newLine: false);
            Console.Write(" ] Key to select or unselect a seat\r\nPress [ ");
            MenuLogic.ColorString("S", newLine: false);
            Console.Write(" ] Key to save current selection\r\n");
            MenuLogic.ColorString(new String('‗', 59));
        }
    }

    public List<SeatModel> ShowSeats(TheatreModel theatre, TimeSlotModel timeSlot = null!)
    {
        Console.Clear();
        // load logics
        TimeSlotsLogic TL = new TimeSlotsLogic();

        //load predefined data for model
        Tuple<string, int>[] pathwaysArray = theatre.LayoutSpecs.PathwayIndexes;
        int[] blockedSeatsArray = theatre.LayoutSpecs.BlockedSeatIndexes;
        int[] handicapedArray = theatre.LayoutSpecs.HandiSeatIndexes;
        List<Tuple<string, int>> seatTypes = GetSeatTypes(theatre.Width, theatre.Height);
        List<SeatModel> reservedSeats = new List<SeatModel>();

        // cast arrays to lists

        List<Tuple<string, int>> pathways = pathwaysArray.ToList();
        List<int> blockedSeats = blockedSeatsArray.ToList();
        List<int> handicaped = handicapedArray.ToList();

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
            /// Show controls
            ShowControls(timeSlot == null);
            // Legend
            ShowLegend();

            int heightCounter = 0;
            int widthCounter = 0;

            // Sort lists
            pathways.Sort();
            blockedSeats.Sort();
            handicaped.Sort();
            selectedSeats.Sort();

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
                        seatIcon = MenuLogic.BoldString("▮");
                        break;
                    case var x when reservedSeats.Exists(s => s.Id == x): // Reserved seat
                        if (handicaped.Contains(x))
                        {
                            seatColor = ConsoleColor.Magenta;
                            seatIcon = MenuLogic.BoldString("▮");
                            break;
                        }
                        else if (seatTypes != null && seatTypes.Exists(s => s.Item1 == "standard" && s.Item2 == x))
                        {
                            seatColor = ConsoleColor.Yellow;
                            seatIcon = MenuLogic.BoldString("▮");
                            break;
                        }
                        else if (seatTypes != null && seatTypes.Exists(s => s.Item1 == "luxury" && s.Item2 == x))
                        {
                            seatColor = ConsoleColor.Blue;
                            seatIcon = MenuLogic.BoldString("▮");
                            break;
                        }
                        else
                        {
                            seatColor = ConsoleColor.White;
                            seatIcon = MenuLogic.BoldString("▮");
                            break;
                        }
                    case var x when blockedSeats.Exists(s => s == x): // Blocked seat
                        seatColor = ConsoleColor.Black;
                        seatIcon = " ";
                        break;
                    case var x when handicaped.Exists(s => s == x): // Handicaped seat
                        seatColor = ConsoleColor.Magenta;
                        seatIcon = "☐";
                        break;
                    case var x when seatTypes != null && seatTypes.Exists(s => s.Item1 == "standard" && s.Item2 == x): // Standard seat
                        seatColor = ConsoleColor.Yellow;
                        seatIcon = "☐";
                        break;
                    case var x when seatTypes != null && seatTypes.Exists(s => s.Item1 == "luxury" && s.Item2 == x): // Luxury seat
                        seatColor = ConsoleColor.Blue;
                        seatIcon = "☐";
                        break;
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
                        else if (selectedSeats.Count >= timeSlot.MaxSeats) // check if more then 9 selected
                        {
                            MaximumSeats(timeSlot.MaxSeats);
                        }
                        else if (!selectedSeats.Contains(selectedSeat) && !reservedSeats.Exists(s => s.Id == selectedSeat))
                        {
                            selectedSeats.Add(selectedSeat);
                        }
                        break;
                    case var x when x == ConsoleKey.S || // Save settings and quit
                        x == ConsoleKey.Escape ||
                        x == ConsoleKey.Q:
                        if (selectedSeats.Count == 0)
                        {
                            MenuLogic.ColorString(">>", newLine: false);
                            Console.WriteLine(" Please select atleast one seat");
                            MenuLogic.ClearLastLines(2, true);
                            break;
                        }
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
            }
            // if (timeSlot != null) MenuLogic.ClearFromTop(7);
            // else MenuLogic.ClearFromTop(10);
        }

        List<SeatModel> returnSelectedSeats = new List<SeatModel>();

        Console.Clear();
        theatre.LayoutSpecs.PathwayIndexes = pathways.ToArray();
        theatre.LayoutSpecs.BlockedSeatIndexes = blockedSeats.ToArray();
        theatre.LayoutSpecs.HandiSeatIndexes = handicaped.ToArray();

        // UpdateList(theatre).ConfigureAwait(false);
        Action a = new(async () => await UpdateList(theatre));
        a.Invoke();

        if (timeSlot != null)
        {
            SeatModel newSeat = null!;
            foreach (int seatIndex in selectedSeats)
            {
                switch (seatIndex)
                {
                    case var x when handicaped.Contains(x):
                        newSeat = new SeatModel(seatIndex, "handicap");
                        timeSlot.Theatre.Seats.Add(newSeat);
                        returnSelectedSeats.Add(newSeat);
                        break;
                    case var x when seatTypes != null && seatTypes.Exists(s => s.Item1 == "standard" && s.Item2 == x):
                        newSeat = new SeatModel(seatIndex, "standard");
                        timeSlot.Theatre.Seats.Add(newSeat);
                        returnSelectedSeats.Add(newSeat);
                        break;
                    case var x when seatTypes != null && seatTypes.Exists(s => s.Item1 == "luxury" && s.Item2 == x):
                        newSeat = new SeatModel(seatIndex, "luxury");
                        timeSlot.Theatre.Seats.Add(newSeat);
                        returnSelectedSeats.Add(newSeat);
                        break;
                    default:
                        newSeat = new SeatModel(seatIndex, "basic");
                        timeSlot.Theatre.Seats.Add(newSeat);
                        returnSelectedSeats.Add(newSeat);
                        break;
                }
            }
            TL.UpdateList(timeSlot).ConfigureAwait(false);
            return returnSelectedSeats;
        }
        return null!;
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

    private List<Tuple<string, int>> GetSeatTypes(int width, int height)
    {
        List<Tuple<string, int>> seatTypes = new List<Tuple<string, int>>();
        int heigthSpacing = (int)Math.Floor((decimal)height / 5);
        int widthSpacing = (int)Math.Floor((decimal)width / 5);

        int heightCounter = 0;
        int widthCounter = 0;
        int skipSeat = 0;

        for (int i = 1; i <= (width * height); i++)
        {
            if (heightCounter >= heigthSpacing * 2 && heightCounter < height - heigthSpacing * 2 &&
                widthCounter >= widthSpacing * 2 && widthCounter < width - widthSpacing * 2)
            {
                seatTypes.Add(new Tuple<string, int>("luxury", i));
            }
            else if (heightCounter >= heigthSpacing && heightCounter < height - heigthSpacing &&
                widthCounter >= widthSpacing && widthCounter < width - widthSpacing + skipSeat)
            {
                seatTypes.Add(new Tuple<string, int>("standard", i));
            }

            widthCounter++;
            if (i % width == 0)
            {
                heightCounter++;
                widthCounter = 0;
                skipSeat = 0;
            }
        }

        return seatTypes;
    }

    private void MaximumSeats(int Max)
    {
        Console.Clear();
        Console.WriteLine($@"There is a maximum of {Max} seats per account.");
        MenuLogic.ColorString(">>", newLine: false);
        Console.WriteLine(" Please contact the support team for more information.");
        MenuLogic.ColorString(new String('‗', 59));

        Console.WriteLine("\nEnter any key to continue to contact page or R to return to seat scedule");
        ConsoleKeyInfo inpKey = Console.ReadKey();
        if (inpKey.Key != ConsoleKey.R) Contact.Start();
    }

    public async Task BlockSeat<T>(TheatreModel theatre, T seatIndex) // block and unblock seat
    {
        int seatNumber = 0;

        if (typeof(T) == typeof(int))
        {
            seatNumber = Convert.ToInt32(seatIndex);
        }
        else if (typeof(T) == typeof(string))
        {
            try
            {
                seatNumber = Convert.ToInt32(seatIndex);
                if (seatNumber < 1 || seatNumber > (theatre.Width * theatre.Height)) return; // out of range seats
            }
            catch (System.Exception)
            {
                return;
            }
        }
        else return;

        List<int> blockedSeats = theatre.LayoutSpecs.BlockedSeatIndexes.ToList();

        if (blockedSeats.Contains(seatNumber))
        {
            blockedSeats.Remove(seatNumber);
        }
        else
        {
            blockedSeats.Add(seatNumber);
        }

        theatre.LayoutSpecs.BlockedSeatIndexes = blockedSeats.ToArray();

        await UpdateList(theatre);
    }

    public double PriceOfSeatType(string type, int theatreId)
    {
        TheatreLogic TheatreLogic = new TheatreLogic();

        var theatre = TheatreLogic.GetById(theatreId)!.Result;

        if (theatre != null)
        {
            switch (type)
            {
                case "standard":
                    return theatre.SeatPrices.Standard;
                case "luxury":
                    return theatre.SeatPrices.Luxury;
                default:
                    return theatre.SeatPrices.Standard;
            }
        }
        return 0;
    }

    public int DupeTheatreToNew(int oldTheatreId)
    {
        TheatreModel oldTheatre = GetAllTheatres().Result.Find(t => t.Id == oldTheatreId)!;
        TheatreModel newTheatre = null!;

        if (oldTheatre == null) return -1;

        newTheatre = (TheatreModel)oldTheatre.DeepClone();

        newTheatre.CopyRoomId = oldTheatre.Id;

        return NewTheatre(newTheatre).Result.Id;
    }
}