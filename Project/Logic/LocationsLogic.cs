public class LocationsLogic
{
    public static async Task<List<LocationModel>> GetAllLocations()
    {
        return await DbLogic.GetAll<LocationModel>();
    }
    // pass model to update
    public static async Task UpdateList(LocationModel location)
    {
        await DbLogic.UpsertItem(location);
    }

    public static void DeleteCategory(int LocationInt) // Deletes category from list of categories
    {
        // account exists and is admin
        if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Admin == true)
        {
            DbLogic.RemoveItemById<LocationModel>(LocationInt);
        }
    }

    public async static void NewLocation()
    {
        string location = "";
        string address = "";
        string description = "";
        string gmapsUrl = "";

        location = QuestionLogic.AskString("Enter the location name");
        address = QuestionLogic.AskString("Enter the address").Replace(',', ' ');
        description = QuestionLogic.AskString("Paste the cinema description");
        gmapsUrl = QuestionLogic.AskString("Paste the google maps link");

        LocationModel newLocation = new LocationModel();
        newLocation.NewLocationModel(location, description, gmapsUrl, address);

        await UpdateList(newLocation);
    }

    public static void RemoveLocation()
    {
        Console.Clear();

        List<LocationModel> locations = GetAllLocations().Result;

        // selection menu
        if (locations.Count > 0)
        {
            string Question = "Which location would you like to delete?\n";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            foreach (LocationModel location in locations)
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

        DbLogic.RemoveItemById<LocationModel>(location.Id);

        Console.WriteLine("Location has been deleted");
        QuestionLogic.AskEnter();
    }

    public static void ViewAllLocations()
    {
        Console.Clear();

        string Question = "Which location would you like to view?\n";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        foreach (LocationModel location in GetAllLocations().Result)
        {
            Options.Add($"{location.Name} - {location.Address}");
            Actions.Add(() => ViewLocationMenu(location));
        }

        Options.Add($"Return");
        Actions.Add(() => Contact.Start());

        MenuLogic.Question(Question, Options, Actions);

        Contact.Start();
    }

    public static void ViewLocationMenu(LocationModel location)
    {
        string Question = ViewLocationDetails(location);

        Question += "\n\nWhat would you like to do?\n";
        List<string> Options = new List<string>();
        List<Action> Actions = new List<Action>();

        Options.Add("Open google maps");
        Actions.Add(() => ViewMapLocation(location));

        Options.Add("Return");
        Actions.Add(() => Contact.Start());

        MenuLogic.Question(Question, Options, Actions);

        Contact.Start();
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

    private static string ViewLocationDetails(LocationModel location)
    {
        return $"Location: {location.Name}\nAddress: {location.Address}\nDescription: {location.Description}";
    }
}