public class Seat
{
    public char Row;
    public int Number;
    public string Name;
    public bool Taken;

    public Seat(char row, int number)
    {
        Row = row;
        Number = number;
        Name = $"{row}{number}";
        Taken = false;
    }
}

public class Room
{
    public List<Seat> AllSeats;
    public int Height;
    public int Width;
    public List<char> chars = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L' };

    public Room(int height, int width)
    {
        Height = height;
        Width = width;
        AllSeats = new List<Seat>();

        if (height > 0 && height <= chars.Count() && Width > 0)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 1; j <= Width; j++)
                {
                    Seat temp = new Seat(chars[i], j);
                    AllSeats.Add(temp);
                }
            }
        }
    }

    public void ShowSeats()
    {
        int i = 1;
        int selectedSeatIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Use arrow keys to navigate and press Enter to select a seat:");
            Console.WriteLine();

            for (int j = 0; j < AllSeats.Count; j++)
            {
                Seat seat = AllSeats[j];

                if (j == selectedSeatIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (seat.Taken)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write($" {seat.Name} ");
                Console.ResetColor();

                if (i == Width)
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
                    selectedSeatIndex = Math.Max(0, selectedSeatIndex - Width);
                    break;
                case ConsoleKey.DownArrow:
                    selectedSeatIndex = Math.Min(AllSeats.Count - 1, selectedSeatIndex + Width);
                    break;
                case ConsoleKey.LeftArrow:
                    selectedSeatIndex = Math.Max(0, selectedSeatIndex - 1);
                    break;
                case ConsoleKey.RightArrow:
                    selectedSeatIndex = Math.Min(AllSeats.Count - 1, selectedSeatIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    Seat selectedSeat = AllSeats[selectedSeatIndex];
                    if (selectedSeat.Taken)
                    {
                        Console.WriteLine("Seat is already taken. Press any key to continue.");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        selectedSeat.Taken = true;
                        Console.WriteLine($"Seat {selectedSeat.Name} has been Selected. Press any key to continue.");
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