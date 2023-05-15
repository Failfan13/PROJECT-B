public static class LocationsLogic
{
    private static List<LocationModel> _locations;

    static LocationsLogic()
    {
        _locations = LocationsAccess.ReadDataList();
    }

    public static List<LocationModel> AllLocations()
    {
        return _locations;
    }

    public static void UpdateLocations()
    {
        _locations.Sort();

        LocationsAccess.Writer(_locations);
    }

    public static void NewLocation()
    {
        string location = "";
        string address = "";
        string description = "";
        string gmapsUrl = "";

        location = QuestionLogic.AskString("Enter the location name");
        address = QuestionLogic.AskString("Enter the address");
        description = QuestionLogic.AskString("Paste the cinema description");
        gmapsUrl = QuestionLogic.AskString("Paste the google maps link");

        LocationModel newLocation = new LocationModel(location, address, description, gmapsUrl);

        _locations.Add(newLocation);

        UpdateLocations();
    }

    public static void RemoveLocation()
    {
        Console.Clear();

        // selection menu
        if (AllLocations().Count > 0)
        {
            string Question = "Which location would you like to delete?\n";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            foreach (LocationModel location in AllLocations())
            {
                Options.Add($"{location.Name}");
                Actions.Add(() => LocationsLogic.Remove(location));
            }
            Options.Add($"Return");
            Actions.Add(() => Admin.ChangeData());

            // lists all users and returns there id;
            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Console.WriteLine("There are no Locations to delete");
            QuestionLogic.AskEnter();
        }
    }

    public static void Remove(LocationModel location)
    {
        Console.Clear();

        _locations.Remove(location);

        Console.WriteLine("Location has been deleted");
        UpdateLocations();
        QuestionLogic.AskEnter();
    }

    public static void ViewAllLocations()
    {
        Console.Clear();

        string Question = "Which location would you like to view?\n";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (LocationModel location in AllLocations())
        {
            Options.Add($"{location.Name}");
            Actions.Add(() => ViewLocationMenu(location));
        }

        Options.Add($"Return");
        Actions.Add(() => Contact.Start());

        MenuLogic.Question(Question, Options, Actions);

        Contact.Start();
    }

    public static void ViewLocationMenu(LocationModel location)
    {
        string Question = "What would you like to view?\n";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Location on google maps");
        Options.Add("Location details");
        Actions.Add(() => ViewMapLocation(location));
        Actions.Add(() => ViewLocationDetails(location));

        MenuLogic.Question(Question, Options, Actions);
    }

    private static void ViewMapLocation(LocationModel location)
    {
        if (OpenWebsiteLogic.ValidUrl(location.GmapsUrl))
        {
            OpenWebsiteLogic.OpenWebBrowser(location.GmapsUrl);
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"The google maps link is invalid\n\nThe address is: {location.Address}");
            QuestionLogic.AskEnter();
        }
    }

    private static void ViewLocationDetails(LocationModel location)
    {
        Console.Clear();
        Console.WriteLine($"Location: {location.Name}\nAddress: {location.Address}\nDescription: {location.Description}");
        QuestionLogic.AskEnter();
    }
}