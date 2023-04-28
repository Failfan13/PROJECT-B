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
        var Seat = (seatNum % width) + 1;
        var Row = (Math.Floor((double)(seatNum / width)));

        return $"{Seat}{letters[(int)Row]}";
    }

    public void ShowLegend() // Legend for theatre model
    {
        Console.WriteLine();
        MenuLogic.ColorString("■", ConsoleColor.Yellow, newLine: false);
        Console.Write(" Taken seat\n");
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
        MenuLogic.ColorString("☐", ConsoleColor.Black, newLine: false);
        Console.Write(" Not selectable (empty)\n");
    }

    public void ShowSeats(TheatreModel theatre, TimeSlotModel timeSlot = null!)
    {
        Console.Clear();
        //load predefined data for model
        List<int> pathways = theatre.LayoutSpecs.PathwayIndexes;
        List<int> blockedSeats = theatre.LayoutSpecs.BlockedSeatIndexes;
        List<int> handicaped = theatre.LayoutSpecs.HandiSeatIndexes;
        List<SeatModel> reservedSeats = timeSlot.Theatre.Seats;

        List<SeatModel> selectedSeats = new List<SeatModel>();
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
        MenuLogic.ColorString($"{new String(' ', (int)(theatre.Width * 1.45))}screen\n");

        //show seats
        int seatAmount = theatre.Width * theatre.Height;
        int charIndex = 0;

        for (int i = 1; i < seatAmount + 1; i++)
        {
            ConsoleColor seatColor = ConsoleColor.White;
            string seatIcon = "▮";

            if (i == 1 || (i - 1) % theatre.Width == 0) MenuLogic.ColorString($"{(char)(charIndex + 65)} ", newLine: false); // Adds row letters

            // show standard seats

            // show luxury seats

            if (reservedSeats.Exists(s => s.Id == i)) seatColor = ConsoleColor.Red; // reserved seats

            if (theatre.LayoutSpecs.HandiSeatIndexes.Exists(s => s == i)) seatColor = ConsoleColor.Blue; // handicaped seats

            if (blockedSeats.Exists(s => s == i)) // blocked seats
            {
                seatColor = ConsoleColor.Black;
                seatIcon = "☐";
            }

            if (selectedSeat == i)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            MenuLogic.ColorString($" {seatIcon} ", seatColor, newLine: false);

            if (i % theatre.Width == 0) // new line if theathre width reached
            {
                Console.WriteLine();
                charIndex++;
            }
        }
        // Adds column numbers
        Console.WriteLine("");
        Console.Write("  ");
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


        // Continue by adding the functionaliry for the creator and non creator
        // TODO
        // - Add color standard & luxury seats by indexing nums
        // - Add button functionality
        // - Add selected seats & seat price (change depend on menu type)
        // - Make sure data selected preserved and updated

    }

}