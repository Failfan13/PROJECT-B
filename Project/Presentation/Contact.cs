public static class Contact
{
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("CONTACT INFO HERE\n");
        Console.WriteLine("Press any key to return to the menu");
        Console.ReadLine();

        LocationsLogic.ViewAllLocations();

        Menu.Start();
    }
}