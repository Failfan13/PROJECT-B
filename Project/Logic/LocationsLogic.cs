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
        string description = "";
        string gmapsLink = "";

        location = QuestionLogic.AskString("Please enter the location");
        description = QuestionLogic.AskString("Please enter the cinema description");
        gmapsLink = QuestionLogic.AskString("Please enter the google maps link");

        LocationModel newLocation = new LocationModel(location, description, gmapsLink);

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
}