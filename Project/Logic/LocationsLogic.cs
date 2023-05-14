public static class LocationsLogic
{
    private static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/locations.csv"));

    public static void NewLocation()
    {
        string location = "";
        string description = "";
        string number = "";
        string email = "";

        location = QuestionLogic.AskString("Please enter the location");
        description = QuestionLogic.AskString("Please enter the cinema description");
        number = QuestionLogic.AskString("Please enter the cinema phone number");
        email = QuestionLogic.AskString("Please enter the cinema email address");

        string newdata = $"Location: {location}, description: {description}, phonenumber: {number}, email: {email} ";

        List<string> data = LocationAccess.ReadDataList(path); // Read the data into a List<string>

        data.Add(newdata);

        string newDataString = string.Join("\n", data); // Join the data list using newline character

        LocationAccess.Writer(newDataString, path); // Write the data to the CSV file
    }

    public static void RemoveLocation()
    {
        Console.Clear();
        List<string> data = new List<string>();
        data = LocationAccess.ReadDataList(path);

        // selection menu
        if (data.Count > 0)
        {
            string Question = "Which location would you like to delete?\n";
            List<string> Options = new List<string>();
            List<Action> Actions = new List<Action>();

            foreach (string location in data)
            {
                Options.Add($"{location}");
                Actions.Add(() => LocationsLogic.Delete(data,location));
            }
                Options.Add($"Return");
                Actions.Add(() => Admin.ChangeData());
        
            // lists all users and returns there id;
            MenuLogic.Question(Question, Options, Actions);
        }
        else
        {
            Console.WriteLine("There are no Locations to delete");
            Console.ReadKey();
        }
    }

    public static void Delete(List<string> data, string location)
    {
        Console.Clear();
        data.Remove(location);

        string newDataString = string.Join("\n", data); 
        LocationAccess.Writer(newDataString, path); 
        Console.WriteLine("Location has been deleted");
        Console.ReadKey();
    }
}