static class Snacks
{
    static private SnacksLogic SnacksLogic = new();

    public static void ShowAllSnacks()
    {
        foreach (SnackModel snack in SnacksLogic.AllMovies())
        {
            Console.WriteLine($"{snack.Id} {snack.Name}");
        }
    }

    public static SnackModel SelectASnack()
    {
        System.Console.Clear();
        Console.WriteLine(@"Here you will be able to select what you would like to
eat or drink while watching the movie");

        Console.WriteLine(@"Make a choice from the menu by entering the number
associated by the snack name");
        ShowAllSnacks();
        while (true)
        {
            var awnser = Console.ReadLine();
            SnackModel snack = null;
            // Return snackmodel to select a snack caller
            try
            {
                snack = SnacksLogic.GetById(Convert.ToInt32(awnser));
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input");
            }
            if (snack != null)
            {
                Console.WriteLine("Snack has been added");
                return snack;
            }
            else
            {
                Console.WriteLine("Snack not found");
            }
        }
    }
}