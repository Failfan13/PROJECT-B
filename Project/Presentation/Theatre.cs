public static class Theatre
{
    private static TheatreLogic TL = new TheatreLogic();
    public static void SelectSeats(TimeSlotModel TimeSlot, bool IsEdited = false)
    {
        TimeSlotsLogic TS = new TimeSlotsLogic();
        var theatre = TimeSlot.Theatre;
        var size = 9;
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin)
        {
            size = 10000;
        }
        var help = TL;//.ShowSeats(theatre, size);
        ReservationLogic RL = new ReservationLogic();

        if (help != null)
        {
            var selectedSeats = help;//.Seats;

            string Question = "Would you like to order snacks?";
            List<string> Options = new List<string>() { "Yes", "No" };
            List<Action> Actions = new List<Action>();

            if (FormatsLogic.GetByFormat(TimeSlot.Format) != null)
            {
                // Actions.Add(() => Snacks.Start(TimeSlot, selectedSeats, IsEdited));
                // Actions.Add(() => Format.Start(TimeSlot, selectedSeats));
            }
            else
            {
                // Actions.Add(() => Snacks.Start(TimeSlot, selectedSeats, IsEdited));
                // Actions.Add(() => RL.MakeReservation(TimeSlot, selectedSeats, IsEdited: IsEdited));
            }

            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Menu.Start();
        }
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

        Options.Add("Change size");
        // Actions.Add(() => TL.ChangeTheatreSize(theatre, () => Theatre.EditMenu(theatre, returnTo)));

        // Options.Add("Block seats");
        // Actions.Add(() => TL.BlockSeats(theatre, () => Theatre.EditMenu(theatre, returnTo)));

        // Options.Add("Unblock seats");
        // Actions.Add(() => TL.UnBlockSeats(theatre, () => Theatre.EditMenu(theatre, returnTo)));

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
        double outSeatPrice = 0;
        double midSeatPrice = 0;
        double innSeatPrice = 0;

        Console.Clear();
        TheatreLogic TL = new TheatreLogic();

        Console.WriteLine($"Welcome to the Theatre creator!\n\nPlease enter the requested information below");
        MenuLogic.ColorString(new String('˭', 59)); // prints colored string

        // Set width & height theatre
        string SizeInp = "";

        Console.WriteLine("The theatre Width & Height ( in seats ex: 12x16 )");
        MenuLogic.ColorString(">>", wholeLine: false);
        Console.WriteLine(" Enter between 10x10 and 100x100, usage of x is required");
        // while Size is not available for theatre
        while (SizeInp == "")
        {
            var input = Console.ReadLine()!.ToLower();
            var splitInput = input.Split('x');
            try
            {
                if (input.Contains('x') && // input contains x
                Int32.Parse(splitInput[0]) >= 10 && Int32.Parse(splitInput[1]) >= 10 &&// check if numbers more then 10
                Int32.Parse(splitInput[0]) <= 100 && Int32.Parse(splitInput[1]) <= 100) // check if numbers less then 100
                {
                    SizeInp = input;
                    width = Int32.Parse(splitInput[0]);
                    height = Int32.Parse(splitInput[1]);
                    break;
                }
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true); // clear last 3 lines
            }
            catch (System.Exception)
            {
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true); // clear last 2 lines
            }
        }

        // Outer seat prices
        Console.WriteLine("\nTheatre outer seat price");
        while (outSeatPrice == 0)
        {
            var input = Console.ReadLine()!.ToLower();
            try
            {
                outSeatPrice = Double.Parse(input);
                break;
            }
            catch (System.Exception)
            {
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true); // clear last 2 lines
            }
        }

        // Middle seat prices (between outer and inner)
        Console.WriteLine("Theatre middle seat price");
        while (midSeatPrice == 0)
        {
            var input = Console.ReadLine()!.ToLower();
            try
            {
                midSeatPrice = Double.Parse(input);
                if (midSeatPrice >= outSeatPrice) break; // check if price less then outer seat
                midSeatPrice = 0;

                Console.WriteLine("The input is lower then the outer seat, please try again");
                MenuLogic.ClearLastLines(2, true); // clear last 2 lines
            }
            catch (System.Exception)
            {
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true); // clear last 2 lines
            }
        }

        // Inner seat prices
        Console.WriteLine("Theatre inner seat price");
        while (innSeatPrice == 0)
        {
            var input = Console.ReadLine()!.ToLower();
            try
            {
                innSeatPrice = Double.Parse(input);
                if (innSeatPrice >= midSeatPrice) break;// check if price less then middle seat
                innSeatPrice = 0;

                Console.WriteLine("The input is lower then the middle seat, please try again");
                MenuLogic.ClearLastLines(2, true); // clear last 2 lines
            }
            catch (System.Exception)
            {
                Console.WriteLine("This is not a valid input, please try again");
                MenuLogic.ClearLastLines(2, true); // clear last 2 lines
            }
        }


        // To draw theatre and remove the seats where needed
        // These seat indexes need to be added to theatre model

        // The show seats will need to able to be added as new theatre or choose seats from it

        // Also make new theatre model will need to check if previous theatre is not empty
        // otherwise save as that id
    }
}