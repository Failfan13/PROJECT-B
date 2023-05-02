public static class Theatre
{
    private static TheatreLogic TL = new TheatreLogic();
    public static void SelectSeats(TimeSlotModel TimeSlot, bool IsEdited = false)
    {
        TimeSlotsLogic TS = new TimeSlotsLogic();
        ReservationLogic RL = new ReservationLogic();
        var theatre = TL.GetById(TimeSlot.Theatre.TheatreId)!;

        // Colored instruction menu
        Console.Clear();
        Console.Write(@$"In the following screen you can change the seat configuration
Press the following buttons to apply changes:
         
Press [ ");
        MenuLogic.ColorString("↑ → ↓ ←", newLine: false);
        Console.Write(" ] Keys to move around the menu\r\nPress [ ");
        MenuLogic.ColorString("Enter", newLine: false);
        Console.Write(" ] Key to select or unselect a seat\r\nPress [ ");
        MenuLogic.ColorString("S", newLine: false);
        Console.Write(" ] Key to save current selection\r\n");
        MenuLogic.ColorString(new String('‗', 59));

        var help = TL.ShowSeats(theatre, TimeSlot);

        if (help != null)
        {
            var selectedSeats = help;

            string Question = "Would you like to order snacks?";
            List<string> Options = new List<string>() { "Yes", "No" };
            List<Action> Actions = new List<Action>();

            if (FormatsLogic.GetByFormat(TimeSlot.Format) != null)
            {
                Actions.Add(() => Snacks.Start(TimeSlot, selectedSeats, IsEdited));
                Actions.Add(() => Format.Start(TimeSlot, selectedSeats));
            }
            else
            {
                Actions.Add(() => Snacks.Start(TimeSlot, selectedSeats, IsEdited));
                Actions.Add(() => RL.MakeReservation(TimeSlot, selectedSeats, IsEdited: IsEdited));
            }

            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Menu.Start();
        }
    }
    public static void DeselectCurrentSeats(TimeSlotModel timeSlot, ReservationModel currReservation)
    {
        TimeSlotsLogic TSL = new TimeSlotsLogic();
        ReservationLogic RL = new ReservationLogic();

        List<SeatModel> seatsToRemove = currReservation.Seats;

        currReservation.Seats = new List<SeatModel>();

        foreach (SeatModel seat in seatsToRemove)
        {
            timeSlot.Theatre.Seats.RemoveAll(s => s.Id == seat.Id);
        }
        TSL.UpdateList(timeSlot);
        RL.UpdateList(currReservation);
    }

    public static void WhatTheatre()
    {
        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (var item in TL.AllTheatres())
        {
            Options.Add($"{item.Id}");
            Actions.Add(() => EditMenu(item));
        }

        Options.Add("\nReturn");
        Actions.Add(() => Admin.Start());

        MenuLogic.Question(Question, Options, Actions);

    }
    public static void EditMenu(TheatreModel theatre, Action returnTo = null!)
    {
        TheatreLogic TL = new TheatreLogic();

        string Question = "What would you like to change?";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Change theatre configuration");
        // Actions.Add(() => TL.ReconfigureTheatre(theatre, () => Theatre.EditMenu(theatre, returnTo)));

        Options.Add("Block a seat");
        // Actions.Add(() => TL.BlockSeat(theatre, () => Theatre.EditMenu(theatre, returnTo)));

        Options.Add("Unblock a seats");
        // Actions.Add(() => TL.UnBlockSeat(theatre, () => Theatre.EditMenu(theatre, returnTo)));

        Options.Add("\nReturn");
        if (returnTo != null)
        {
            Actions.Add(returnTo.Invoke);
        }

        MenuLogic.Question(Question, Options, Actions);
    }

    public static void MakeNewTheatre()
    {
        int width = 0;
        int height = 0;
        double baseSeatPrice = 0; // basic
        double stanSeatPrice = 0; // standard
        double luxeSeatPrice = 0; // luxury

        Console.Clear();
        TheatreLogic TL = new TheatreLogic();
        TimeSlotsLogic TSL = new TimeSlotsLogic();

        Console.WriteLine($"Welcome to the Theatre creator!\n\nPlease enter the requested information below");
        MenuLogic.ColorString(new String('˭', 59)); // prints colored string

        // Set width & height theatre
        string SizeInp = "";

        Console.WriteLine("The theatre Width & Height ( in seats ex: 12x16 )");
        MenuLogic.ColorString(">>", newLine: false);
        Console.WriteLine(" Enter between 10x10 and 26x26, usage of x is required"); // command the user

        // while Size is not available for theatre
        while (SizeInp == "")
        {
            var input = Console.ReadLine()!.ToLower();
            var splitInput = input.Split('x');
            try
            {
                if (input.Contains('x') && // input contains x
                Int32.Parse(splitInput[0]) >= 10 && Int32.Parse(splitInput[1]) >= 10 &&// check if numbers more then 10
                Int32.Parse(splitInput[0]) <= 26 && Int32.Parse(splitInput[1]) <= 26) // check if numbers less then 100
                {
                    SizeInp = input;
                    width = Int32.Parse(splitInput[0]);
                    height = Int32.Parse(splitInput[1]);
                    break;
                }
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true);
            }
            catch (System.Exception)
            {
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true);
            }
        }

        // basic seat prices
        Console.WriteLine("\nTheatre basic seat price");
        MenuLogic.ColorString(">>", newLine: false);
        Console.WriteLine(" Enter number between 0 and 999"); // command the user
        while (baseSeatPrice == 0)
        {
            var input = Console.ReadLine()!.ToLower();
            try
            {
                var seatPrice = Double.Parse(input);
                if (seatPrice >= 0 && seatPrice <= 999) baseSeatPrice = seatPrice; // between 0 and 999
                break;
            }
            catch (System.Exception)
            {
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true);
            }
        }

        // standard seat prices (between basic and luxury)
        Console.WriteLine("Theatre standard seat price");
        MenuLogic.ColorString(">>", newLine: false);
        Console.WriteLine($" Enter number between {baseSeatPrice} and 999"); // command the user
        while (stanSeatPrice == 0)
        {
            var input = Console.ReadLine()!.ToLower();
            try
            {
                stanSeatPrice = Double.Parse(input);
                if (stanSeatPrice >= baseSeatPrice) break; // check if price less then outer seat
                stanSeatPrice = 0;

                Console.WriteLine("The input is lower then the outer seat, please try again");
                MenuLogic.ClearLastLines(2, true);
            }
            catch (System.Exception)
            {
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true);
            }
        }

        // luxury seat prices
        Console.WriteLine("Theatre luxury seat price");
        MenuLogic.ColorString(">>", newLine: false);
        Console.WriteLine($" Enter number between {stanSeatPrice} and 999"); // command the user
        while (luxeSeatPrice == 0)
        {
            var input = Console.ReadLine()!.ToLower();
            try
            {
                luxeSeatPrice = Double.Parse(input);
                if (luxeSeatPrice >= stanSeatPrice) break;// check if price less then middle seat
                luxeSeatPrice = 0;

                Console.WriteLine("The input is lower then the middle seat, please try again");
                MenuLogic.ClearLastLines(2, true);
            }
            catch (System.Exception)
            {
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true);
            }
        }

        TheatreModel newTheatre = TL.MakeTheatre(width, height, baseSeatPrice, stanSeatPrice, luxeSeatPrice);

        Console.WriteLine("\nWould you like to change seat configuration? (y/n)");
        ConsoleKeyInfo inputKey = Console.ReadKey(true);
        // Y key to config menu
        if (inputKey.Key == ConsoleKey.Y)
        {
            // Colored instruction menu
            MenuLogic.ClearLastLines(12);
            Console.Write(@$"In the following screen you can change the seat configuration
Press the following buttons to apply changes:
         
Press [ ");
            MenuLogic.ColorString("↑ → ↓ ←", newLine: false);
            Console.Write(" ] Keys to move around the menu\r\nPress [ ");
            MenuLogic.ColorString("Enter", newLine: false);
            Console.Write(" ] Key to select a seat\r\nPress [ ");
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

            TL.ShowSeats(newTheatre);
        }
    }
}