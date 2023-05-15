public static class Contact
{
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("CONTACT INFO HERE\n");

        Console.WriteLine("Test bericht, wil je alle locaties zien? (y/n)");

        if (Console.ReadKey().Key == ConsoleKey.Y)
        {
            LocationsLogic.ViewAllLocations();
        }

        Menu.Start();
    }
}