public static class Theather
{
    public static void Start()
    {
        var slots = TimeSlotAccess.LoadAll();
        var slot = slots[0];

        MoviesLogic movieL = new MoviesLogic();
        TheatherLogic theatherL = new TheatherLogic();

        Console.Clear();
        Console.WriteLine($"Movie Title: {movieL.GetById(slot.MovieId).Title}");
        Console.WriteLine($"Start time: {slot.Start}");



        foreach (var row in theatherL.GetById(slot.Theater).Rows)
        {
            Console.WriteLine($"{row.Id}");
            foreach (var seat in row.Seats)
            {
                if (seat.Reserved)
                {
                    Console.Write("x");
                }
                else
                {
                    Console.Write("o");
                }
            }

        }
    }

}
