static class Snacks
{
    static private SnacksLogic SnacksLogic = new();

    public static void ShowAll()
    {
        foreach (SnackModel snack in SnacksLogic.AllSnacks())
        {
            Console.WriteLine($"{snack.Id} {snack.Name}");
        }
    }

    // Start request to user
    public static SnackModel Start()
    {
        Console.Clear();
        Console.WriteLine(@"Here you will be able to select what you would like to
eat or drink while watching the movie");

        Console.WriteLine(@"Make a choice from the menu by entering the number
associated by the snack name");
        ShowAll();
        while (true)
        {
            // User input
            var awnser = Console.ReadLine();
            SnackModel snack = null;
            // try convert entree to int
            try
            {
                // get snack from array
                snack = SnacksLogic.GetById(Convert.ToInt32(awnser));
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input");
            }
            // if snack not null
            if (snack != null)
            {
                Console.WriteLine("Snack has been added");
                return snack;
            }
            // if snack is null
            else
            {
                Console.WriteLine("Snack not found");
            }
        }
    }
}