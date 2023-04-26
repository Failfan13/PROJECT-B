using System;
using System.Text.Json;
public class TheatreLogic
{
    private List<TheatreModel> _theatres;

    public TheatreLogic()
    {
        _theatres = TheatreAccess.LoadAll();
    }

    public void AddTheatre(TheatreModel theatre)
    {
        _theatres.Add(theatre);
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

    public int MakeTheatre(int width, int height, int OldId = -1)
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

        TheatreModel theatre = new TheatreModel(newId, width, height);
        AddTheatre(theatre);

        return theatre.Id;
    }

    // public void UpdateToTheatre(TimeSlotModel timeSlot)
    // {
    //     TheatreModel? theatre = GetById(timeSlot.Theatre.Id);
    //     if (theatre != null)
    //     {
    //         timeSlot.Theatre = theatre;
    //     }
    // }

    // Class to store to values from the function below
    // public class Helper
    // {
    //     public TheatreModel Theatre { get; set; }
    //     public List<SeatModel> Seats { get; set; }

    //     public Helper(TheatreModel theatre, List<SeatModel> seats)
    //     {
    //         Theatre = theatre;
    //         Seats = seats;
    //     }
    // }

    // Function to show seats based on a Theatre model
    // MaxLength is used to limit seat selection
    public void ShowSeats(TimeSlotModel currTimeSlot, bool select = false)
    {
        int theatreId = 0;

        SeatModel seatM = new SeatModel(0, 0);

        TheatreModel currRessTheatre = null!;
        List<SeatModel> currRessSeats = null!;

        // Desect theatre properties current time slot
        try
        {
            theatreId = currTimeSlot.Theatre.TheatreId;
            currRessTheatre = GetById(theatreId)!;
            currRessSeats = currTimeSlot.Theatre.Seats;
        }
        catch (System.InvalidCastException)
        {
            return;
        }

        // // build theatre
        int seatAmount = currRessTheatre.Width * currRessTheatre.Height;
        List<Tuple<string, object>> AllSeats = new List<Tuple<string, object>>() { };

        // Creates AllSeats on time slot order
        // (object)1 bv is allocated placeholder
        // AllSeats: List<("A1", (object)1), ("A2", SeatModel) etc.>
        for (int i = 0; i < seatAmount; i++)
        {
            if (currRessSeats.Exists(s => s.Id == i))
            {
                AllSeats.Add(new Tuple<string, object>(
                    seatM.SeatRow(currRessTheatre.Width),
                    currRessSeats.Find(s => s.Id == i)!));
            }
            else
            {
                AllSeats.Add(new Tuple<string, object>(
                    seatM.SeatRow(currRessTheatre.Width),
                    i));
            }
        }

        // Writes all seats or sends to selector
        if (select)
        {
            SeatSelector();
        }
        else
        {
            foreach (var seat in AllSeats)
            {
                Console.WriteLine($"seatNum: {seat.Item1}, type: {seat.Item2}");
            }
        }
    }

    public void SeatSelector()
    {

    }
    // public Helper? ShowSeats(TheatreModel theatre, int MaxLength = 1000)
    // {
    //     var AllSeats = theatre.Seats;
    //     List<SeatModel> selectedSeats = new List<SeatModel>();

    //     int i = 1;
    //     int selectedSeatIndex = 0;

    //     while (true)
    //     {
    //         Console.Clear();
    //         Console.WriteLine("Use arrow keys to navigate and press Enter to select a seat:\nPress C to confirm and reserve selected seats\nX to Cancel\nR to reset selections and start over:");

    //         for (int j = 0; j < AllSeats.Count; j++)
    //         {
    //             SeatModel seat = AllSeats[j];

    //             if (j == selectedSeatIndex)
    //             {
    //                 Console.BackgroundColor = ConsoleColor.Yellow;
    //                 Console.ForegroundColor = ConsoleColor.Black;
    //             }
    //             else if (selectedSeats.Contains(seat))
    //             {
    //                 Console.BackgroundColor = ConsoleColor.Blue;
    //                 Console.ForegroundColor = ConsoleColor.White;
    //             }
    //             else if (seat.Reserved)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Red;
    //             }
    //             else if (seat.Handicapped)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Blue;
    //             }
    //             else if (seat.Luxury)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Magenta;
    //             }
    //             else
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Green;
    //             }

    //             Console.Write($" {seat.SeatRow(theatre.Width)} ");
    //             Console.ResetColor();

    //             if (i == theatre.Width)
    //             {
    //                 Console.WriteLine();
    //                 i = 0;
    //             }
    //             i += 1;
    //         }

    //         Console.WriteLine();
    //         Console.WriteLine($"Selected Seats: {string.Join(", ", selectedSeats.Select(s => $"{s.SeatRow(theatre.Width)}"))}");

    //         Console.WriteLine();
    //         Console.ForegroundColor = ConsoleColor.Green;
    //         Console.Write("■");
    //         Console.ResetColor();
    //         Console.Write(" Regular seat\n");
    //         Console.ForegroundColor = ConsoleColor.Magenta;
    //         Console.Write("■");
    //         Console.ResetColor();
    //         Console.Write(" Luxury seat\n");
    //         Console.ForegroundColor = ConsoleColor.Blue;
    //         Console.Write("■");
    //         Console.ResetColor();
    //         Console.Write(" Handicap seat\n");
    //         Console.ForegroundColor = ConsoleColor.Red;
    //         Console.Write("■");
    //         Console.ResetColor();
    //         Console.Write(" Taken seat\n");
    //         Console.WriteLine();
    //         ConsoleKeyInfo keyInfo = Console.ReadKey(true);

    //         switch (keyInfo.Key)
    //         {
    //             case ConsoleKey.UpArrow:
    //                 selectedSeatIndex = Math.Max(0, selectedSeatIndex - theatre.Width);
    //                 break;
    //             case ConsoleKey.DownArrow:
    //                 selectedSeatIndex = Math.Min(AllSeats.Count - 1, selectedSeatIndex + theatre.Width);
    //                 break;
    //             case ConsoleKey.LeftArrow:
    //                 selectedSeatIndex = Math.Max(0, selectedSeatIndex - 1);
    //                 break;
    //             case ConsoleKey.RightArrow:
    //                 selectedSeatIndex = Math.Min(AllSeats.Count - 1, selectedSeatIndex + 1);
    //                 break;
    //             case ConsoleKey.Enter:
    //                 SeatModel selectedSeat = AllSeats[selectedSeatIndex];
    //                 if (selectedSeats.Contains(selectedSeat))
    //                 {
    //                     selectedSeats.Remove(selectedSeat);
    //                 }
    //                 else if (selectedSeats.Count < MaxLength && !selectedSeat.Reserved)
    //                 {
    //                     selectedSeats.Add(selectedSeat);
    //                 }
    //                 break;
    //             case ConsoleKey.X:
    //                 return null;
    //             case ConsoleKey.R:
    //                 selectedSeats.Clear();
    //                 Console.WriteLine("Selection cleared.");
    //                 Console.ReadKey(true);
    //                 break;
    //             case ConsoleKey.C:
    //                 if (selectedSeats.Count == 0)
    //                 {
    //                     Console.WriteLine("Please select at least one seat.");
    //                     Console.ReadKey(true);
    //                 }
    //                 else
    //                 {
    //                     Console.Clear();
    //                     Console.WriteLine($"You have selected {selectedSeats.Count} seats: {string.Join(", ", selectedSeats.Select(s => $"{s.SeatRow(theatre.Width)}"))}");
    //                     Console.WriteLine("Press Y to confirm or any other key to cancel:");

    //                     ConsoleKeyInfo confirmKeyInfo = Console.ReadKey(true);

    //                     if (confirmKeyInfo.Key == ConsoleKey.Y)
    //                     {
    //                         foreach (var seat in selectedSeats)
    //                         {
    //                             seat.Reserved = true;
    //                         }
    //                         foreach (SeatModel seat in theatre.Seats)
    //                         {
    //                             foreach (var seats in selectedSeats)
    //                             {
    //                                 if (seat.Id == seats.Id)
    //                                 {
    //                                     seat.Reserved = seats.Reserved;
    //                                 }
    //                             }
    //                         }
    //                         return new Helper(theatre, selectedSeats);
    //                     }
    //                     else
    //                     {
    //                         selectedSeats.Clear();
    //                     }
    //                 }
    //                 break;
    //             default:
    //                 break;
    //         }
    //     }
    // }

    // public void BlockSeats(TheatreModel theatre, Action returnTo = null!)
    // {
    //     TheatreLogic TL = new TheatreLogic();
    //     var Help = ShowSeats(theatre);
    //     if (Help != null)
    //     {
    //         UpdateList(Help.Theatre);
    //     }

    //     returnTo();
    // }

    // public void UnBlockSeats(TheatreModel theatre, Action returnTo = null!)
    // {
    //     TheatreLogic TL = new TheatreLogic();
    //     foreach (var seat in theatre.Seats)
    //     {
    //         seat.Reserved = false;
    //     }
    //     UpdateList(theatre);
    //     Console.WriteLine("All seats have been unblocked");
    //     QuestionLogic.AskEnter();

    //     returnTo();
    // }

    // public void ChangeTheatreSize(TheatreModel theatre, Action returnTo = null!)
    // {
    //     TheatreLogic TL = new TheatreLogic();
    //     int width = (int)QuestionLogic.AskNumber("Enter the width of the Theatre");
    //     int height = (int)QuestionLogic.AskNumber("Enter the height of the Theatre");
    //     MakeTheatre(width, height, theatre.Id);

    //     returnTo();
    // }

}
