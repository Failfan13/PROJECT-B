// public void UpdateToTheatre(TimeSlotModel timeSlot)
// {
//     TheatreModel? theatre = GetById(timeSlot.Theatre.Id);
//     if (theatre != null)
//     {
//         timeSlot.Theatre = theatre;
//     }
// }

// public Dictionary<int, string> SeatPositions(int width, int height)
// {
//     Dictionary<int, string> seatPos = new Dictionary<int, string>();

//     double median = (double)(width * height) / 2;
//     if (median % 1 == 0)
//     {

//     }
//     else
//     {
//         Console.WriteLine(median);
//     }

//     return null;
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
// public void ShowSeats(TimeSlotModel currTimeSlot, bool select = false)
// {
//         int theatreId = 0;

//         SeatModel seatM = new SeatModel(0, 0);

//         TheatreModel currRessTheatre = null!;
//         List<SeatModel> currRessSeats = null!;

//         // Desect theatre properties current time slot
//         try
//         {
//             theatreId = currTimeSlot.Theatre.TheatreId;
//             currRessTheatre = GetById(theatreId)!;
//             currRessSeats = currTimeSlot.Theatre.Seats;
//         }
//         catch (System.InvalidCastException)
//         {
//             return;
//         }

//         // // build theatre
//         int seatAmount = currRessTheatre.Width * currRessTheatre.Height;
//         List<SeatModel> AllSeats = new List<SeatModel>() { };

//         // Creates AllSeats on time slot order
//         for (int i = 0; i < seatAmount; i++)
//         {
//             if (currRessSeats.Exists(s => s.Id == i))
//             {
//                 AllSeats.Add(currRessSeats.Find(s => s.Id == i)!);
//             }
//             else
//             {
//                 AllSeats.Add(new SeatModel(i, currRessTheatre));
//             }
//         }

//         // Writes all seats or sends to selector
//         if (select)
//         {
//             SeatSelector(AllSeats, currRessTheatre);
//         }
//         else
//         {
//             foreach (var seat in AllSeats)
//             {
//                 Console.WriteLine($"seatNum: {seat.Item1}, type: {seat.Item2}");
//             }
//         }
//     }

//     public void SeatSelector(List<SeatModel> seatList, TheatreModel theatre)
//     {
//         TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();
//         List<SeatModel> selectedSeats = new List<SeatModel>() { };

//         int selectedSeatIndex = 0;
//         int i = 1;

//         while (true)
//         {
//             Console.Clear();
//             // instruction and long lines in between
//             Console.WriteLine(@$"Use arrow keys to navigate and press Enter to select a seat
// {new String('˭', 59) + "\r"}
// Press [ C ] to confirm and reserve selected seats
// Press [ R ] to reset selections and start over
// Press [ X ] to Cancel and return to main menu
// {new String('‗', 59) + "\r"}
// ");

//             foreach (var seat in seatList)
//             {
//                 if (seat.Item2 is int)
//                 {
//                     if ((int)seat.Item2 == selectedSeatIndex) // select block sNum
//                     {
//                         Console.BackgroundColor = ConsoleColor.Yellow;
//                         Console.ForegroundColor = ConsoleColor.Black;
//                     }
//                     Console.ForegroundColor = ConsoleColor.Green;
//                 }
//                 else if (seat.Item2 is SeatModel)
//                 {
//                     if ((seat.Item2 as SeatModel)!.Id == selectedSeatIndex)  // select block sMod
//                     {
//                         Console.BackgroundColor = ConsoleColor.Yellow;
//                         Console.ForegroundColor = ConsoleColor.Black;
//                     }
//                     switch (seat.Item2 as SeatModel)
//                     {
//                         case var s when selectedSeats.Contains(seat.Item2):
//                             Console.BackgroundColor = ConsoleColor.Blue;
//                             Console.ForegroundColor = ConsoleColor.White;
//                             break;
//                         case var s when s!.Reserved:
//                             Console.ForegroundColor = ConsoleColor.Red;
//                             break;
//                         case var s when s!.Handicapped:
//                             Console.ForegroundColor = ConsoleColor.Blue;
//                             break;
//                         case var s when s!.Luxury:
//                             Console.ForegroundColor = ConsoleColor.Magenta;
//                             break;
//                         default:
//                             Console.ForegroundColor = ConsoleColor.Green;
//                             break;
//                     };
//                 }


//                 Console.Write($" {seat.Item1} ");
//                 Console.ResetColor();

//                 if (i == theatre.Width)
//                 {
//                     Console.WriteLine();
//                     i = 0;
//                 }
//                 i += 1;
//             }

//             Console.WriteLine($"\nSelected Seats: {string.Join(", ", selectedSeats.Select(s => $"{SeatNumber(theatre.Width, s.Id)}"))}");

//             ConsoleKeyInfo keyInfo = Console.ReadKey(true);
//             switch (keyInfo.Key)
//             {
//                 case ConsoleKey.UpArrow:
//                     selectedSeatIndex = Math.Max(0, selectedSeatIndex - theatre.Width);
//                     break;
//                 case ConsoleKey.DownArrow:
//                     selectedSeatIndex = Math.Min(seatList.Count - 1, selectedSeatIndex + theatre.Width);
//                     break;
//                 case ConsoleKey.LeftArrow:
//                     selectedSeatIndex = Math.Max(0, selectedSeatIndex - 1);
//                     break;
//                 case ConsoleKey.RightArrow:
//                     selectedSeatIndex = Math.Min(seatList.Count - 1, selectedSeatIndex + 1);
//                     break;
//                 case ConsoleKey.Enter:
//                     SeatModel selectedSeat = seatList[selectedSeatIndex].Item2 as SeatModel;
//                     if (selectedSeats.Contains(selectedSeat))
//                     {
//                         selectedSeats.Remove(selectedSeat);
//                     }
//                     else if (!selectedSeat.Reserved)
//                     {
//                         selectedSeats.Add(selectedSeat);
//                     }
//                     break;
//                 default:
//                     Console.WriteLine("sus");
//                     break;
//             }
//         }
// show seats with right color applied

// }

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



// (object)1 bv is placeholder
// AllSeats: List<("A1", (object)1), ("A2", SeatModel) etc.>
// for (int i = 0; i < seatAmount; i++)
// {
//     if (currRessSeats.Exists(s => s.Id == i))
//     {
//         AllSeats.Add(new Tuple<string, object>(
//             SeatNumber(currRessTheatre.Width, i),
//             currRessSeats.Find(s => s.Id == i)!));
//     }
//     else
//     {
//         AllSeats.Add(new Tuple<string, object>(
//             SeatNumber(currRessTheatre.Width, i),
//             i));
//     }
// }